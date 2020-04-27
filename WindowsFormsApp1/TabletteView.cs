using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BouncingBall {
	public partial class TabletteView : Form {

		private Tablet tab;

		public delegate void TabletPositionChangedHandler(Point newPosition);
		event TabletPositionChangedHandler TabletPositionChanged;

		private int room_width;
		private int room_lenght;
		private Matrix matrix;
		private PointF scale;

		private IMqttClient client;

		private GameObject preBuilt;

		private List<Wall> lstWall;

		public TabletteView(int room_width, int room_lenght) {
			this.room_width = room_width;
			this.room_lenght = room_lenght;
			this.tab = new Tablet(0, 0, 0, ScreenFormat._24PC);
			this.tab.ball = new Ball(room_width, room_lenght);
			this.lstWall = new List<Wall>();
			this.matrix = new Matrix();
			this.scale = new PointF();
			// - - - - - - - - - -
			initMqttClientAsync("Client1", "broker.hivemq.com");
			// - - - - - - - - - -
			InitializeComponent();
			this.lbl_format.Text = String.Format("Largeur : {0}, Hauteur {1}", tab.getWidth(), tab.getHeight());
			this.pictureBox1.MouseWheel += new MouseEventHandler(onMouseWheel);
			this.TabletPositionChanged += new TabletPositionChangedHandler(this.onPositionChanged);
		}

		private void onPositionChanged(Point position) {

		}

		private async void initMqttClientAsync(string username, string url) {
			this.client = MqttWrapper.CreateClient();
			MqttWrapper.SetClientSubs(this.client);

			var options = new MqttClientOptionsBuilder()
				.WithClientId(username)
				.WithTcpServer(url)
				.Build();
			await this.client.ConnectAsync(options, System.Threading.CancellationToken.None);

			this.client.UseApplicationMessageReceivedHandler(e => {
				Invoke(new Action(() => {
					string message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
					if (e.ApplicationMessage.Topic.Equals(MqttWrapper.getTopicList()[(int)MqttWrapper.Topic.BALL_POS])) {
						string[] coord = message.Split(';');
						this.tab.ball.center.X = (float.Parse(coord[0]));
						this.tab.ball.center.Y = (float.Parse(coord[1]));
					} else if (e.ApplicationMessage.Topic.Equals(MqttWrapper.getTopicList()[(int)MqttWrapper.Topic.NEW_WALL])) {
						// nothing
					} else if (e.ApplicationMessage.Topic.Equals(MqttWrapper.getTopicList()[(int)MqttWrapper.Topic.BUILD_WALL])) {
						Wall w = new Wall(message);
						w.setBuilt();
						this.lstWall.Add(w);
					} else {
						//this.lbl.Text = message;
						Invoke(new Action(() => {
							this.lbl.Text = "Can't handle message";
						}));
					}
				}));
			});

			this.client.UseDisconnectedHandler(async e => {
				Invoke(new Action(() => {
					this.lbl.Text = "Disconnected";
				}));
				MqttWrapper.connectClient(this.client, "Client1", "broker.hivemq.com");
			});
		}


		#region Painting
		private void timer_Tick(object sender, EventArgs e) {
			this.pictureBox1.Invalidate();
			/*Invoke(new Action(() =>
			{
				this.lbl.Text = String.Format("{0:#.#} | {1:#.#}", this.tab.getPosX(), this.tab.getPosY());
			}));*/
		}

		private void pictureBox1_Paint(object sender, PaintEventArgs e) {
			Graphics gfx = e.Graphics;
			this.matrix.Reset();
			this.scale.X = (float)room_width / e.ClipRectangle.Width;
			this.scale.Y = (float)room_lenght /e.ClipRectangle.Height;
			// drawing pen
			Pen redPen = new Pen(Color.FromArgb(255, 255, 0, 0), 2);
			//
			int dim_x = this.tab.getWidth();
			int dim_y = this.tab.getHeight();
			int x = this.tab.getPosX();
			int y = this.tab.getPosY();

			// Cross at the center screen
			gfx.DrawLine(Pens.Black, (dim_x / 2) - 5, dim_y / 2, (dim_x / 2) + 5, dim_y / 2);
			gfx.DrawLine(Pens.Black, dim_x / 2, (dim_y / 2) - 5, dim_x / 2, (dim_y / 2) + 5);

			/*
			Matrix m1 = new Matrix();
			m1.Translate(dim_x / 2, dim_y/2);
			m1.Rotate(this.tab.getAngle());
			*/

			// preBuild
			if (this.preBuilt != null) {
				this.preBuilt.draw(gfx, this.room_width, this.room_lenght);
			}

			// Rectangle rouge englobant au format de la tablette
			gfx.DrawRectangle(redPen, 0, 0, dim_x, dim_y);
			// place le centre de l'image au centre de la tablette
			gfx.TranslateTransform(-(x - dim_x / 2), -(y - dim_y / 2));
			this.matrix.Translate(-(x - dim_x / 2), -(y - dim_y / 2));


			// rotation par le centre de la tablette
			gfx.TranslateTransform(x, y);
			this.matrix.Translate(x, y);
			gfx.RotateTransform(-this.tab.getAngle());
			this.matrix.Rotate(-this.tab.getAngle());
			gfx.TranslateTransform(-x, -y);
			this.matrix.Translate(-x, -y);


			// cadre entourant la salle de jeux
			gfx.DrawLine(Pens.Blue, 0, 0, room_width, 0);
			gfx.DrawLine(Pens.Blue, 0, room_lenght, room_width, room_lenght);
			gfx.DrawLine(Pens.Blue, 0, 0, 0, room_lenght);
			gfx.DrawLine(Pens.Blue, room_width, 0, room_width, room_lenght);

			// balle
			this.tab.ball.draw(gfx, room_width, room_lenght);
			foreach (Wall w in this.lstWall) {
				w.draw(gfx, room_width, room_lenght);
			}

			this.matrix.Invert();
		}
		#endregion Painting

		public Tablet getTablette() {
			return this.tab;
		}

		private int prev_x = 0;
		private int prev_y = 0;

		private bool clickIsDown = false;

		#region DEV TOOLS BELOW, SHOULD BE UNUSED IN RELEASE

		private enum Mode {
			drawing,
			moving
		}

		private Mode mode = Mode.moving;

		public void labelToMousePos(MouseEventArgs e) {
			/*Invoke(new Action(() => {
				this.lbl.Text = String.Format("{0:#.#} | {1:#.#}", e.X - this.tab.getWidth() / 2, e.Y - this.tab.getHeight() / 2);
			}));*/
		}

		private void onMouseWheel(object sender, MouseEventArgs e) {
			this.tab.setAngle(this.tab.getAngle() + e.Delta / 10);
		}

		private void TabletteView_KeyUp(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.T) {
				mode = mode == Mode.moving ? Mode.drawing : Mode.moving;
			}
		}

		#endregion DEV TOOLS BELOW, SHOULD BE UNUSED IN RELEASE (TODO)

		private void pictureBox1_MouseUp(object sender, MouseEventArgs e) {
			#region DEV TOOLS BELOW, SHOULD BE UNUSED IN RELEASE
			if (mode == Mode.moving) {
			} else {
				#endregion DEV TOOLS BELOW, SHOULD BE UNUSED IN RELEASE (TODO)
				if (this.preBuilt is Wall wall) {
					this.preBuilt = null;
					wall.tranform(this.matrix);
					Invoke(new Action(() => {
						this.lbl.Text = String.Format("{0} | {1}", wall.getOrigine(), wall.getEnd());
					}));
					MqttWrapper.SendMqttMessageTo(this.client,
						MqttWrapper.getTopicList()[(int)MqttWrapper.Topic.NEW_WALL],
						String.Format("{0};{1};{2};{3}", wall.getOrigine().X, wall.getOrigine().Y, wall.getEnd().X, wall.getEnd().Y)
						);
				}
			}
			clickIsDown = false;

		}

		private void pictureBox1_MouseMove(object sender, MouseEventArgs e) {
			labelToMousePos(e);
			#region DEV TOOLS BELOW, SHOULD BE UNUSED IN RELEASE
			if (clickIsDown && mode == Mode.moving) {
				this.tab.moveBy((e.X - this.tab.getWidth() / 2) - prev_x, (e.Y - this.tab.getHeight() / 2) - prev_y);
				prev_x = e.X - this.tab.getWidth() / 2;
				prev_y = e.Y - this.tab.getHeight() / 2;
				TabletPositionChanged(this.tab.getPosition());
			} else {
				#endregion DEV TOOLS BELOW, SHOULD BE UNUSED IN RELEASE (TODO)
				if (this.preBuilt is Wall) {
					((Wall)this.preBuilt).setEnd(e.X, e.Y);
				}
			}

		}

		private void pictureBox1_MouseDown(object sender, MouseEventArgs e) {
			#region DEV TOOLS BELOW, SHOULD BE UNUSED IN RELEASE
			if (mode == Mode.moving) {
				prev_x = e.X - this.tab.getWidth() / 2;
				prev_y = e.Y - this.tab.getHeight() / 2;

			} else {
				#endregion DEV TOOLS BELOW, SHOULD BE UNUSED IN RELEASE (TODO)
				this.preBuilt = new Wall(e.X, e.Y, e.X, e.Y);
			}
			clickIsDown = true;
		}



	}
}

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
		private String id = "";
		private string preferredTopic;

		public delegate void TabletPositionChangedHandler(Point newPosition);
		event TabletPositionChangedHandler TabletPositionChanged;

		public delegate void TabletAngleChangedHandler(float newAngle);
		event TabletAngleChangedHandler TabletAngleChanged;

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
			initMqttClientAsync("Client1", "localhost");
			// - - - - - - - - - -
			InitializeComponent();
			this.lbl_format.Text = String.Format("Largeur : {0}, Hauteur {1}", tab.getWidth(), tab.getHeight());
			this.pictureBox1.MouseWheel += new MouseEventHandler(onMouseWheel);
			this.TabletPositionChanged += new TabletPositionChangedHandler(this.onPositionChanged);
			this.TabletAngleChanged += new TabletAngleChangedHandler(this.onAngleChanged);
		}

		private void onPositionChanged(Point position) {
			string topic = MqttWrapper.getTopicList()[(int)MqttWrapper.Topic.TABS_ID_POS];
			topic = topic.Split('+')[0] + this.id + topic.Split('+')[1];

			MqttWrapper.SendMqttMessageTo(this.client, topic, string.Format("{0};{1};{2}", position.X, position.Y, (int)this.tab.format));
		}

		private void onAngleChanged(float angle) {
			string topic = MqttWrapper.getTopicList()[(int)MqttWrapper.Topic.TABS_ID_ANG];
			topic = topic.Split('+')[0] + this.id + topic.Split('+')[1];

			MqttWrapper.SendMqttMessageTo(this.client, topic, string.Format("{0}", angle));
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
					} else if (e.ApplicationMessage.Topic.Equals(MqttWrapper.getTopicList()[(int)MqttWrapper.Topic.TABS_IDS])) {
						string[] ids = message.Split(';');
						if (this.id.Length == 0 && ids.Length > 1) {
							this.id = ids[1];
							Invoke(new Action(() => {
								this.lbl.Text = this.id;
							}));
						}
					} else {
						//this.lbl.Text = message;
						Invoke(new Action(() => {
							this.lbl.Text = "Can't handle message from "+ e.ApplicationMessage.Topic;
						}));
					}
				}));
			});

			this.client.UseDisconnectedHandler(async e => {
				Invoke(new Action(() => {
					this.lbl.Text = "Disconnected from Broker";
				}));
				MqttWrapper.connectClient(this.client, "Client1", "localhost");
			});
		}


		#region Painting
		private void timer_Tick(object sender, EventArgs e) {
			this.pictureBox1.Invalidate();
		}

		private void pictureBox1_Paint(object sender, PaintEventArgs e) {

			#region Setting up drawing vars
			Graphics gfx = e.Graphics;
			this.matrix.Reset();
			this.scale.X = 1 / ((float)this.tab.getWidth() / e.ClipRectangle.Width);
			this.scale.Y = 1 / ((float)this.tab.getHeight() / e.ClipRectangle.Height);
			// drawing pen
			Pen redPen = new Pen(Color.FromArgb(255, 255, 0, 0), 2);
			//
			int dim_x = (int)(this.tab.getWidth() * scale.X);
			int dim_y = (int)(this.tab.getHeight() * scale.Y);
			int x = (int)(this.tab.getPosX() * scale.X);
			int y = (int)(this.tab.getPosY() * scale.Y);


			// rotate arround tablet center
			this.matrix.Translate(-(x - dim_x / 2), -(y - dim_y / 2));
			this.matrix.Translate(x, y);
			this.matrix.Rotate(-this.tab.getAngle());
			this.matrix.Translate(-x, -y);
			gfx.Transform = this.matrix;
			#endregion Setting up drawing vars

			#region Drawing room content
			// Background
			gfx.FillRectangle(Brushes.Bisque, 0, 0, room_width * scale.X, room_lenght * scale.Y);
			// ball
			this.tab.ball.draw(gfx, scale);
			// walls
			foreach (Wall w in this.lstWall) {
				w.draw(gfx, scale);
			}
			#endregion Drawing room content

			#region Drawing screen relative content
			this.matrix.Invert();
			Matrix m = new Matrix();
			gfx.Transform = m;

			// Cross at the center screen
			gfx.DrawLine(Pens.Black, (dim_x / 2) - 5, dim_y / 2, (dim_x / 2) + 5, dim_y / 2);
			gfx.DrawLine(Pens.Black, dim_x / 2, (dim_y / 2) - 5, dim_x / 2, (dim_y / 2) + 5);

			// preBuild
			if (this.preBuilt != null) {
				this.preBuilt.draw(gfx, scale);
			}
			#endregion Drawing screen relative content

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
			TabletAngleChanged?.Invoke(this.tab.getAngle());
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
					MqttWrapper.SendMqttMessageTo(this.client,
						MqttWrapper.getTopicList()[(int)MqttWrapper.Topic.NEW_WALL],
						String.Format("{0};{1};{2};{3}", wall.getOrigine().X / scale.X, wall.getOrigine().Y / scale.Y, wall.getEnd().X / scale.X, wall.getEnd().Y / scale.Y)
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

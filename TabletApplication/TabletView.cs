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
using Emgu.CV.UI;
using System.Configuration;
using ObjectLibrary;
using BouncingBall.Properties;

namespace TabletApplication {
	public partial class TabletView : Form {

		#region Variables
		/// <summary>
		/// The tablet instance of this view
		/// </summary>
		private Tablet tablet { get; }
		/// <summary>
		/// The id of this player, used in MQTT
		/// </summary>
		private string id = "";
		/// <summary>
		/// The width of the playing area / room
		/// </summary>
		private int roomWidth;
		/// <summary>
		/// The lenght of the playing area / room
		/// </summary>
		private int roomLenght;
		/// <summary>
		/// The transformation matrix of the view in the room
		/// </summary>
		private Matrix matrix;
		/// <summary>
		/// Contains the processed scale size of the table within the room
		/// </summary>
		/// <remarks>
		/// (TODO) Must probably be (pre)processed in full screen
		/// </remarks>
		private PointF scale;
		/// <summary>
		/// Any object that could be built within the room by the player
		/// </summary>
		private GameObject preBuilt;
		/// <summary>
		/// A List of every wall that should be drawn
		/// </summary>
		private List<Wall> lstWall;
		/// <summary>
		/// The MQTT client
		/// </summary>
		private IMqttClient client;
		/// <summary>
		/// The ball
		/// </summary>
		private Ball ball;

		private Goal goal;

		private string brokerurl = Settings.Default.sBrokerUrl;
		#endregion Variables

		#region Constructor
		/// <summary>
		/// Construct a Tablet view in a room
		/// </summary>
		/// <param name="room_width">The width of the playing area / room</param>
		/// <param name="room_lenght">The lenght of the playing area / room</param>
		public TabletView(int room_width, int room_lenght) {
			this.roomWidth = room_width;
			this.roomLenght = room_lenght;
			this.tablet = new Tablet(0, 0, 0, EnumFormat._24PC, true);
			this.ball = new Ball(room_width, room_lenght);
			this.lstWall = new List<Wall>();
			this.matrix = new Matrix();
			this.scale = new PointF();
			// - - - - - - - - - -
			initMqttClientAsync(brokerurl);
			// - - - - - - - - - -
			InitializeComponent();
			this.pictureBox1.MouseWheel += new MouseEventHandler(onMouseWheel);
			this.tablet.TabletPositionChanged += new Tablet.TabletPositionChangedHandler(this.onPositionChanged);
			this.tablet.TabletAngleChanged += new Tablet.TabletAngleChangedHandler(this.onAngleChanged);
			this.timer.Interval = Settings.Default.iGameTick;
		}
		#endregion Constructor

		#region MQTT protocol
		private void onPositionChanged(Point position) {
			string topic = MqttWrapper.GetFullTopicList()[(int)MqttWrapper.Topic.TABS_ID_POS];
			topic = topic.Split('+')[0] + this.id + topic.Split('+')[1];
			MqttWrapper.SendMqttMessageTo(this.client, topic, string.Format("{0};{1};{2}", position.X, position.Y, (int)this.tablet.format));
		}

		private void onAngleChanged(float angle) {
			string topic = MqttWrapper.GetFullTopicList()[(int)MqttWrapper.Topic.TABS_ID_ANG];
			topic = topic.Split('+')[0] + this.id + topic.Split('+')[1];
			MqttWrapper.SendMqttMessageTo(this.client, topic, string.Format("{0}", angle));
		}

		private async void initMqttClientAsync(string url) {
			this.client = MqttWrapper.CreateClient();
			MqttWrapper.SetClientSubs(this.client);

			var lstId = Settings.Default.sAvailableIds;
			int idSelector = 0;
			bool connected = false;
			string userId;
			while (!connected) {
				userId = lstId[idSelector];
				try {
					var options = new MqttClientOptionsBuilder()
						.WithClientId(userId)
						.WithTcpServer(url)
						.Build();
					await this.client.ConnectAsync(options, System.Threading.CancellationToken.None);
					this.id = userId;
					connected = true;
				} catch (Exception) {
					idSelector++;
					if (idSelector == lstId.Count) {
						MessageBox.Show("There is no more available place in the lobby or there is no broker to connect with. \nPlease try again later");
						System.Environment.Exit(1);
					}
				}
				System.Threading.Thread.Sleep(2000);
			}

			this.lbl.Text = this.id;

			this.client.UseApplicationMessageReceivedHandler(e => {
				try {
					Invoke(new Action(() => {
						string message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);


						if (e.ApplicationMessage.Topic.Equals(MqttWrapper.GetFullTopicList()[(int)MqttWrapper.Topic.BALL_POS])) {
							string[] coord = message.Split(';');
							PointF p = new PointF(
								float.Parse(coord[0]),
								float.Parse(coord[1])
								);
							this.ball.center = p;
						} else if (e.ApplicationMessage.Topic.Equals(MqttWrapper.GetFullTopicList()[(int)MqttWrapper.Topic.BUILD_WALL])) {
							this.lstWall.Clear();
							string[] walls = message.Split('!');
							int wall_count = int.Parse(walls[0]);
							for (int i = 1; i <= wall_count; i++) {
								Wall w = new Wall(walls[i]);
								w.setBuilt();
								lstWall.Add(w);
							}
						} else if (e.ApplicationMessage.Topic.Equals(MqttWrapper.GetFullTopicList()[(int)MqttWrapper.Topic.GOAL])) {
							this.lbl_message.Text = message;
							this.goal = new Goal(message);
							this.lbl.Text = this.id +" " + this.goal.anchor;
						}else {
							this.lbl_message.Text = "Can't handle message from " + e.ApplicationMessage.Topic;
						}
						if (e.ApplicationMessage.Retain || e.ApplicationMessage.Topic.StartsWith("goal")) {
							this.lbl_message.Text = message + (e.ApplicationMessage.Retain ? "\ntrue" : "\nfalse");
						}
					}));
				} catch {
					Application.Exit();
				}
			});

			this.client.UseDisconnectedHandler(e => {
				Invoke(new Action(() => {
					this.lbl_message.Text = "Disconnected from Broker";
				}));
				try {
					MqttWrapper.ConnectClient(this.client, this.id, brokerurl);
				} finally {
					MessageBox.Show("Can't reconnecte to broker. Connection lost");
					Environment.Exit(1);
				}
			});
		}
		#endregion

		#region Painting / Drawing
		// Launch drawing
		private void timer_Tick(object sender, EventArgs e) {
			this.pictureBox1.Invalidate();
			foreach (Wall w in lstWall) {
				w.tick(Settings.Default.iGameTick);
			}
			updateCameraView();
		}

		internal void updateCameraView() {
			this.pictureBox2.Image = this.tablet.diplayableframe;
			Invoke(new Action(() => {
				//this.lbl_message.Text = this.tablet.message;
			}));
		}

		/// <summary>
		/// Draw the scene
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureBox1_Paint(object sender, PaintEventArgs e) {

			#region Setting up drawing vars
			Graphics gfx = e.Graphics;
			this.matrix.Reset();
			this.scale.X = 1 / ((float)this.tablet.getWidth() / e.ClipRectangle.Width);
			this.scale.Y = 1 / ((float)this.tablet.getHeight() / e.ClipRectangle.Height);

			// drawing pen
			Pen pen = new Pen(Color.FromArgb(255, 150, 80, 120), 4);
			// scaled pos / dimmension
			int dim_x = (int)(this.tablet.getWidth() * scale.X);
			int dim_y = (int)(this.tablet.getHeight() * scale.Y);

			int x = (int)(this.tablet.getPosX() * scale.X);
			int y = (int)(this.tablet.getPosY() * scale.Y);

			// rotate arround tablet center
			this.matrix.Translate(-(x - dim_x / 2), -(y - dim_y / 2));
			this.matrix.Translate(x, y);
			this.matrix.Rotate(-this.tablet.getAngle());
			this.matrix.Translate(-x, -y);
			gfx.Transform = this.matrix;
			#endregion Setting up drawing vars

			#region Drawing room content
			// Background
			gfx.FillRectangle(Brushes.Bisque, 0, 0, roomWidth * scale.X, roomLenght * scale.Y);
			// Line between center screen end player
			PointF relative_ball_pos = new PointF(this.ball.center.X * scale.X, this.ball.center.Y * scale.Y);
			gfx.DrawLine(pen, relative_ball_pos, new Point(x, y));
			// ball
			this.ball.draw(gfx, scale);
			// walls
			foreach (Wall w in this.lstWall) {
				w.draw(gfx, scale);
			}
			// goal
			this.goal?.draw(gfx, scale);
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

		#endregion Painting / Drawing

		#region DEV TOOLS BELOW, SHOULD BE UNUSED IN RELEASE (controle with mouse)

		private int prev_x = 0;
		private int prev_y = 0;

		private bool clickIsDown = false;

		private enum Mode {
			drawing,
			moving
		}

		private Mode mode = Mode.drawing;

		private void onMouseWheel(object sender, MouseEventArgs e) {
			this.tablet.setAngle(this.tablet.getAngle() + e.Delta / 10);
		}

		private void TabletteView_KeyUp(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.T) {
				mode = mode == Mode.moving ? Mode.drawing : Mode.moving;
			}
		}

		#endregion DEV TOOLS BELOW, SHOULD BE UNUSED IN RELEASE (controle with mouse) (TODO)

		private void pictureBox1_MouseUp(object sender, MouseEventArgs e) {
			#region DEV TOOLS BELOW, SHOULD BE UNUSED IN RELEASE
			if (mode == Mode.moving) {
			} else {
				#endregion DEV TOOLS BELOW, SHOULD BE UNUSED IN RELEASE (TODO)
				if (this.preBuilt is Wall wall) {
					this.preBuilt = null;
					wall.tranform(this.matrix);
					MqttWrapper.SendMqttMessageTo(this.client,
						MqttWrapper.GetFullTopicList()[(int)MqttWrapper.Topic.NEW_WALL],
						string.Format("{0};{1};{2};{3}", wall.getOrigine().X / scale.X, wall.getOrigine().Y / scale.Y, wall.getEnd().X / scale.X, wall.getEnd().Y / scale.Y)
						);
				}
			}
			clickIsDown = false;
		}

		private void pictureBox1_MouseMove(object sender, MouseEventArgs e) {
			#region DEV TOOLS BELOW, SHOULD BE UNUSED IN RELEASE
			if (clickIsDown && mode == Mode.moving) {
				this.tablet.moveBy((e.X - this.tablet.getWidth() / 2) - prev_x, (e.Y - this.tablet.getHeight() / 2) - prev_y);
				prev_x = e.X - this.tablet.getWidth() / 2;
				prev_y = e.Y - this.tablet.getHeight() / 2;
				onPositionChanged(this.tablet.getPosition());
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
				prev_x = e.X - this.tablet.getWidth() / 2;
				prev_y = e.Y - this.tablet.getHeight() / 2;

			} else {
				#endregion DEV TOOLS BELOW, SHOULD BE UNUSED IN RELEASE (TODO)
				this.preBuilt = new Wall(e.X, e.Y, e.X, e.Y, 5);
			}
			clickIsDown = true;
		}

		private void useHough_CheckedChanged(object sender, EventArgs e) {
			this.tablet.useHough = this.useHough.Checked;
		}
	}
}

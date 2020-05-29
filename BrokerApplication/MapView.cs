using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Server;
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
using ObjectLibrary;
using BouncingBall.Properties;

namespace BrokerApplication {
	public partial class MapView : Form {

		#region Variables
		/// <summary>
		/// Width of the playing area in millimeters
		/// </summary>
		private int roomWidth;
		/// <summary>
		/// lenght of the playing area in millimeters
		/// </summary>
		private int roomLenght;
		/// <summary>
		/// List of all tablets in the game
		/// </summary>
		private Dictionary<String, Tablet> lstTab;
		/// <summary>
		/// The ball
		/// </summary>
		private Ball ball;
		/// <summary>
		/// The processed scale of the room
		/// </summary>
		private PointF scale;
		/// <summary>
		/// A List of wall
		/// </summary>
		private List<Wall> lstWall;
		/// <summary>
		/// The MQTT broker / server
		/// </summary>
		private IMqttServer broker;
		#endregion Variables

		#region Constructor
		/// <summary>
		/// Set a Map where a ball evolve, TODO try connection of new tablet
		/// </summary>
		/// <param name="t"></param>
		public MapView(int room_width, int room_lenght) {
			this.roomWidth = room_width;
			this.roomLenght = room_lenght;
			this.scale = new PointF();
			// - - - - - - - - - -
			initBroker();
			// - - - - - - - - - -
			this.ball = new Ball(room_width, room_lenght, 1);
			this.ball.onBallMoved += new Ball.BallMovedHandler(onBallMoved);
			lstTab = new Dictionary<String, Tablet>();
			this.lstWall = new List<Wall>();
			// - - - - - - - - - -
			InitializeComponent();
		}
		#endregion Constructor

		#region MQTT protocol
		private void initBroker() {
			this.broker = MqttWrapper.CreateBroker();
			MqttWrapper.StartMqttBroker(this.broker);

			// Call when a new message is received
			this.broker.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(
				e => {
					string message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
					string topic = e.ApplicationMessage.Topic;
					if (topic.Equals(MqttWrapper.GetFullTopicList()[(int)MqttWrapper.Topic.BALL_POS])) {
					} else if (topic.Equals(MqttWrapper.GetFullTopicList()[(int)MqttWrapper.Topic.NEW_WALL])) {
						Wall w = new Wall(message);
						if (!this.ball.isColliding(w)) {
							this.lstWall.Add(w);
							w.setBuilt();
						}
						sendWalls();
					} else if (topic.StartsWith("tablet/")) {
						string id = topic.Split('/')[1];
						string[] datas = message.Split(';');
						if (this.lstTab.ContainsKey(id)) {
							if (this.lstTab[id] == null && topic.EndsWith("pos")) {
								Tablet t = new Tablet(int.Parse(datas[0]), int.Parse(datas[1]), 0, (EnumFormat)int.Parse(datas[2]));
								this.lstTab[id] = t;
							} else if (topic.EndsWith("pos")) {
								this.lstTab[id].setPosX(int.Parse(datas[0]));
								this.lstTab[id].setPosY(int.Parse(datas[1]));
							} else if (topic.EndsWith("angle")) {
								this.lstTab[id].setAngle(float.Parse(datas[0]));
							}
						}
					}
					try {
						Invoke(new Action(() => {
							string text = "ids :";
							foreach (string ch in this.lstTab.Keys) {
								if (lstTab[ch] is Tablet t) {
									text += string.Format("\n{0}_{1}_{2:#.##}", ch, t.getPosition(), t.getAngle());
								} else {
									text += string.Format("{0} pos:{1};", ch, "noDatas");
								}
							}
							this.lbl_angle.Text = text;
						}));
					} catch {
						Environment.Exit(0);
					}
				});

			this.broker.ClientConnectedHandler = new MqttServerClientConnectedHandlerDelegate(
			e => {
				if (!this.lstTab.ContainsKey(e.ClientId)) {
					lock (lstTab) {
						this.lstTab.Add(e.ClientId, null);
					}
				}
			});

			this.broker.ClientDisconnectedHandler = new MqttServerClientDisconnectedHandlerDelegate(
			e => {
				lock (lstTab) {
					this.lstTab.Remove(e.ClientId);
				}
			});
		}

		/// <summary>
		/// Call when the ball move
		/// </summary>
		/// <param name="pos"></param>
		private void onBallMoved(PointF pos) {
			string topic = MqttWrapper.GetFullTopicList()[(int)MqttWrapper.Topic.BALL_POS];
			MqttWrapper.SendMqttMessageTo(this.broker, topic, String.Format("{0:#.##};{1:#.##}", pos.X, pos.Y));
		}

		public void sendWalls() {
			string message = "";
			string topic = MqttWrapper.GetFullTopicList()[(int)MqttWrapper.Topic.BUILD_WALL];
			message += lstWall.Count;
			foreach (Wall w in lstWall) {
				message += "!" + w;
			}
			MqttWrapper.SendMqttMessageTo(this.broker, topic, message, true);
		}

		#endregion MQTT protocol

		#region Painting / Drawing
		/// <summary>
		/// Trigger a game loop
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timer_Tick(object sender, EventArgs e) {
			this.pictureBox1.Invalidate();
			this.ball.move(lstWall.ToArray());
			List<Wall> tmp_lst = new List<Wall>();
			foreach (Wall w in lstWall) {
				w.tick(Settings.Default.iGameTick);
				if (w.timeToLive <= 0) {
					tmp_lst.Add(w);
				}
			}
			foreach (Wall w in tmp_lst) {
				this.lstWall.Remove(w);
			}
			if (tmp_lst.Count != 0) {
				sendWalls();
			}
		}

		/// <summary>
		/// Handle draw in the main map
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureBox1_Paint(object sender, PaintEventArgs e) {
			Graphics gfx = e.Graphics;
			this.scale.X = (float)e.ClipRectangle.Width / roomWidth;
			this.scale.Y = (float)e.ClipRectangle.Height / roomLenght;
			lock (lstTab) {
				foreach (Tablet tab in lstTab.Values) {
					if (tab != null) {
						int dim_x = (int)(this.scale.X * tab.getWidth());
						int dim_y = (int)(this.scale.Y * tab.getHeight());
						int x = (int)(this.scale.X * tab.getPosX());
						int y = (int)(this.scale.Y * tab.getPosY());
						//
						Pen blackPen = new Pen(Color.FromArgb(255, 0, 0, 0), 2);
						Pen redPen = new Pen(Color.FromArgb(255, 255, 0, 0), 2);
						Pen bluePen = new Pen(Color.FromArgb(255, 0, 0, 255), 10);
						//
						gfx.DrawLine(redPen, 0, 0, x, y);
						gfx.DrawRectangle(redPen, x - dim_x / 2, y - dim_y / 2, dim_x, dim_y);
						// définition de l'origine de rotation
						gfx.TranslateTransform(x, y);
						gfx.RotateTransform(tab.getAngle());
						// dessine
						gfx.DrawRectangle(blackPen, -dim_x / 2, -dim_y / 2, dim_x, dim_y);
						// rétablie la position/rotation d'origine
						gfx.RotateTransform(-tab.getAngle());
						gfx.TranslateTransform(-x, -y);
					}
				}
			}

			this.ball.draw(gfx, scale);
			foreach (Wall w in lstWall) {
				w.draw(gfx, scale);
			}
			if (lstWall.Count != 0) {
				Wall w = lstWall[lstWall.Count - 1];
				Func<PointF, PointF, PointF> func = (PointF p, PointF s) => new PointF(p.X * s.X, p.Y * s.Y);
				PointF A = func(w.getOrigine(), scale);
				PointF I = func(this.ball.center, scale);
				gfx.DrawLine(Pens.Black, A, I);
			}
		}
		#endregion Painting / Drawing
	}
}

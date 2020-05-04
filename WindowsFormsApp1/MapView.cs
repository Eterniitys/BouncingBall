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

namespace BouncingBall {
	public partial class MapView : Form {
		/// <summary>
		/// Width of the playing area in millimeters
		/// </summary>
		private int room_width;
		/// <summary>
		/// lenght of the playing area in millimeters
		/// </summary>
		private int room_lenght;
		/// <summary>
		/// List of all tablets in the game
		/// </summary>
		private Dictionary<String, Tablet> lst_tab;
		/// <summary>
		/// The ball
		/// </summary>
		private Ball ball;

		private PointF scale;

		private List<Wall> lstWall;

		private IMqttServer broker;

		private static string availableId = "!;1;2;3;!";

		private void initBroker() {
			this.broker = MqttWrapper.CreateBroker();
			MqttWrapper.StartMqttBroker(this.broker);

			MqttWrapper.SendMqttMessageTo(
				this.broker,
				MqttWrapper.getTopicList()[(int)MqttWrapper.Topic.TABS_IDS],
				availableId,
				true);

			// Quand un message est reçu
			this.broker.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(
				e => {
					string message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
					string topic = e.ApplicationMessage.Topic;
					if (topic.Equals(MqttWrapper.getTopicList()[(int)MqttWrapper.Topic.BALL_POS])) {
					} else if (topic.Equals(MqttWrapper.getTopicList()[(int)MqttWrapper.Topic.NEW_WALL])) {
						// TODO checké si le mur n'est pas sur la balle
						Wall w = new Wall(message);
						this.lstWall.Add(w);
						w.setBuilt();
						MqttWrapper.SendMqttMessageTo(this.broker, MqttWrapper.getTopicList()[(int)MqttWrapper.Topic.BUILD_WALL], message);
					} else if (topic.StartsWith("tablet/id/")) {
						string id = topic.Split('/')[2];
						string[] datas = message.Split(';');
						if (!this.lst_tab.ContainsKey(id)) {
							Tablet t = new Tablet(int.Parse(datas[0]), int.Parse(datas[1]), 0, (ScreenFormat)int.Parse(datas[2]));
							this.lst_tab.Add(id, t);
						} else if (topic.EndsWith("pos")) {
							this.lst_tab[id].setPosX(int.Parse(datas[0]));
							this.lst_tab[id].setPosY(int.Parse(datas[1]));
						} else if (topic.EndsWith("angle")) {
							this.lst_tab[id].setAngle(float.Parse(datas[0]));
						}
						/*
						Invoke(new Action(() => {
							this.lbl_angle.Text = message;
						}));*/
					}
				});

			// Quand un client se connecte
			this.broker.ClientConnectedHandler = new MqttServerClientConnectedHandlerDelegate(
			e => {
				/*
				await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
					//ConnectionStatus.Text = "Client disconnected fired";
				});
				*/
			});

		}

		/// <summary>
		/// Set a Map where a ball evolve, TODO try connection of new tablet
		/// </summary>
		/// <param name="t"></param>
		public MapView(int room_width, int room_lenght) {
			this.room_width = room_width;
			this.room_lenght = room_lenght;
			this.scale = new PointF();
			// - - - - - - - - - -
			initBroker();
			// - - - - - - - - - -
			this.ball = new Ball(room_width, room_lenght, 1);
			this.ball.onBallMoved += new Ball.BallMovedHandler(onBallMoved);
			lst_tab = new Dictionary<String, Tablet>();
			this.lstWall = new List<Wall>();
			// - - - - - - - - - -
			InitializeComponent();
		}

		/// <summary>
		/// Call when the ball move
		/// </summary>
		/// <param name="pos"></param>
		public void onBallMoved(PointF pos) {
			string topic = MqttWrapper.getTopicList()[(int)MqttWrapper.Topic.BALL_POS];
			MqttWrapper.SendMqttMessageTo(this.broker, topic, String.Format("{0:#.##};{1:#.##}", pos.X, pos.Y));
		}

		/// <summary>
		/// Trigger a game loop
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timer_Tick(object sender, EventArgs e) {
			this.pictureBox1.Invalidate();
			this.ball.move(lstWall.ToArray());
		}

		/// <summary>
		/// Handle draw in the main map
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureBox1_Paint(object sender, PaintEventArgs e) {
			Graphics gfx = e.Graphics;
			this.scale.X = (float)e.ClipRectangle.Width / room_width;
			this.scale.Y = (float)e.ClipRectangle.Height / room_lenght;
			foreach (KeyValuePair<string, Tablet> kvp in lst_tab) {
				int dim_x = (int)(this.scale.X * kvp.Value.getWidth());
				int dim_y = (int)(this.scale.Y * kvp.Value.getHeight());
				int x = (int)(this.scale.X * kvp.Value.getPosX());
				int y = (int)(this.scale.Y * kvp.Value.getPosY());
				//
				Pen blackPen = new Pen(Color.FromArgb(255, 0, 0, 0), 2);
				Pen redPen = new Pen(Color.FromArgb(255, 255, 0, 0), 2);
				Pen bluePen = new Pen(Color.FromArgb(255, 0, 0, 255), 10);
				//
				gfx.DrawLine(redPen, 0, 0, x, y);
				gfx.DrawRectangle(redPen, x - dim_x / 2, y - dim_y / 2, dim_x, dim_y);
				// définition de l'origine de rotation
				gfx.TranslateTransform(x, y);
				gfx.RotateTransform(kvp.Value.getAngle());
				// dessine
				gfx.DrawRectangle(blackPen, -dim_x / 2, -dim_y / 2, dim_x, dim_y);
				// rétablie la position/rotation d'origine
				gfx.RotateTransform(-kvp.Value.getAngle());
				gfx.TranslateTransform(-x, -y);
			}
			this.ball.draw(gfx, scale);
			foreach (Wall w in lstWall) {
				w.draw(gfx, scale);
			}
			if (lstWall.Count != 0) {
				Wall w = lstWall[lstWall.Count - 1];
				Func<PointF, PointF, PointF> fuck = (PointF p, PointF s) => new PointF(p.X * s.X, p.Y * s.Y);
				PointF A = fuck(w.getOrigine(), scale);
				PointF I = fuck(this.ball.center, scale);
				gfx.DrawLine(Pens.Black, A, I);
			}
			Invoke(new Action(() => {
				if (lstWall.Count != 0) {
					Wall w = lstWall[lstWall.Count - 1];
					PointF A = w.getOrigine();
					PointF B = w.getEnd();
					PointF C = this.ball.center;

					/*
					PointF u = new PointF(A.X - B.X, A.Y - B.Y);
					PointF AC = new PointF(A.X - C.X, A.Y - C.Y);

					float numerateur = u.X * AC.Y - u.Y * AC.X;   // norme du vecteur v
					float denominateur = (float)Math.Sqrt(u.X * u.X + u.Y * u.Y);  // norme de u
					float CI = numerateur / denominateur;

					numerateur = numerateur < 0 ? -numerateur : numerateur;
					int dist = (int)Math.Sqrt(Math.Pow((double)(A.X - C.X), 2) + Math.Pow((double)(A.Y - C.Y), 2));
					*/
					this.lbl_angle.Text = string.Format("{0}", this.ball.direction);
				}
			}));
		}

		public void addWall(Wall wall) {
			this.lstWall.Add(wall);
		}


	}
}

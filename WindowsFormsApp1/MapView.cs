using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
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
		private List<Tablet> lst_tab;
		/// <summary>
		/// The ball
		/// </summary>
		private Ball ball;

		private PointF scale;

		private List<Wall> lstWall;

		private IMqttServer broker;
		private IMqttClient client;

		/// <summary>
		/// Initilize MQTT client
		/// </summary>
		/// <param name="username"></param>
		/// <param name="url"></param>
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
					/*this.lbl_angle.Text = "Message received";
						this.lbl_angle.Text = String.Format("{0}", e.ApplicationMessage.Topic);
						this.lbl_angle.Text = String.Format("{0}", e.ApplicationMessage.QualityOfServiceLevel);
						this.lbl_angle.Text = String.Format("{0}", e.ApplicationMessage.Retain);*/
					string message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
					if (e.ApplicationMessage.Topic.Equals(MqttWrapper.getTopicList()[(int)MqttWrapper.Topic.BALL_POS])) {
					} else if (e.ApplicationMessage.Topic.Equals(MqttWrapper.getTopicList()[(int)MqttWrapper.Topic.NEW_WALL])) {
						// TODO checké si le mur n'est pas sur la balle
						Wall w = new Wall(message);
						PointF p = w.getOrigine();
						p.X *= scale.X;
						p.Y *= scale.Y;
						w.setOrigine(p);
						p = w.getEnd();
						p.X *= scale.X;
						p.Y *= scale.Y;
						w.setEnd(p);
						this.lstWall.Add(w);
						w.setBuilt();
						MqttWrapper.SendMqttMessageTo(this.client, MqttWrapper.getTopicList()[(int)MqttWrapper.Topic.BUILD_WALL], message);
					} else {
						this.lbl_angle.Text = message;
					}
				}));
			});
		}

		/// <summary>
		/// Set a Map where a ball evolve, TODO handle connection of new tablet
		/// </summary>
		/// <param name="t"></param>
		public MapView(int room_width, int room_lenght) {
			this.room_width = room_width;
			this.room_lenght = room_lenght;
			this.scale = new PointF();
			// - - - - - - - - - -
			//this.broker = MqttWrapper.CreateBroker();
			//MqttWrapper.StartMqttBroker(this.broker);
			initMqttClientAsync("NextBroker", "broker.hivemq.com");
			// - - - - - - - - - -
			this.ball = new Ball(room_width, room_lenght, 1);
			this.ball.onBallMoved += new Ball.BallMovedHandler(onBallMoved);
			lst_tab = new List<Tablet>();
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
			MqttWrapper.SendMqttMessageTo(this.client, topic, String.Format("{0:#.##};{1:#.##}", pos.X, pos.Y));
		}

		/// <summary>
		/// Trigger a game loop
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timer_Tick(object sender, EventArgs e) {
			this.pictureBox1.Invalidate();
			this.ball.move();
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
			foreach (Tablet t in lst_tab) {
				int dim_x = (int)(this.scale.X * this.lst_tab[0].getWidth());
				int dim_y = (int)(this.scale.Y * this.lst_tab[0].getHeight());
				int x = (int)(this.scale.X * t.getPosX());
				int y = (int)(this.scale.Y * t.getPosY());

				//
				Pen blackPen = new Pen(Color.FromArgb(255, 0, 0, 0), 2);
				Pen redPen = new Pen(Color.FromArgb(255, 255, 0, 0), 2);
				Pen bluePen = new Pen(Color.FromArgb(255, 0, 0, 255), 10);
				//
				gfx.DrawLine(redPen, 0, 0, x, y);
				gfx.DrawRectangle(redPen, x - dim_x / 2, y - dim_y / 2, dim_x, dim_y);
				// définition de l'origine de rotation
				gfx.TranslateTransform(x, y);
				gfx.RotateTransform(t.getAngle());
				// dessine
				gfx.DrawRectangle(blackPen, -dim_x / 2, -dim_y / 2, dim_x, dim_y);
				// rétablie la position/rotation d'origine
				gfx.RotateTransform(-t.getAngle());
				gfx.TranslateTransform(-x, -y);
			}
			this.ball.draw(gfx, e.ClipRectangle.Width, e.ClipRectangle.Height, scale);
			foreach (Wall w in lstWall) {
				w.draw(gfx, e.ClipRectangle.Width, e.ClipRectangle.Height, scale);
			}
			Invoke(new Action(() => {
				this.lbl_angle.Text = string.Format("{0}", ((float)e.ClipRectangle.Width / room_width));
			}));
		}

		public void addWall(Wall wall) {
			this.lstWall.Add(wall);
		}


	}
}

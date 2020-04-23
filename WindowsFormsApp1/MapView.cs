using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
		private Tablet[] lst_tab;
		/// <summary>
		/// The ball
		/// </summary>
		private Ball ball;

		private List<Wall> lstWall;

		/// <summary>
		/// Set a Map where a ball evolve, TODO handle connection of new tablet
		/// </summary>
		/// <param name="t"></param>
		public MapView(Tablet t, int room_width, int room_lenght) {
			this.room_width = room_width;
			this.room_lenght = room_lenght;
			// - - - - - - - - - -
			lst_tab = new Tablet[1];
			lst_tab[0] = t;
			this.ball = new Ball(room_width, room_lenght, 1);
			t.ball = this.ball;

			this.lstWall = new List<Wall>();
			InitializeComponent();
		}

		/// <summary>
		/// Trigger a game loop
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timer_Tick(object sender, EventArgs e) {
			this.pictureBox1.Invalidate();
			this.ball.move();
			foreach (Tablet t in lst_tab) {
				t.refreshBall(this.ball.getPosition(), this.ball.getID());
			}
			this.lbl_angle.Text = String.Format("{0:#.#} | {1:#.#}", this.ball.center.X, this.ball.center.Y);
		}

		/// <summary>
		/// Handle draw in the main map
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureBox1_Paint(object sender, PaintEventArgs e) {
			Graphics gfx = e.Graphics;
			foreach (Tablet t in lst_tab) {
				int dim_x = (this.lst_tab[0].getWidth() * e.ClipRectangle.Width) / room_width;
				int dim_y = (this.lst_tab[0].getHeight() * e.ClipRectangle.Height) / room_lenght;
				int x = t.getPosX() * e.ClipRectangle.Width / room_width;
				int y = t.getPosY() * e.ClipRectangle.Height / room_lenght;
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
				//dessine
				gfx.DrawRectangle(blackPen, -dim_x / 2, -dim_y / 2, dim_x, dim_y);
				//rétablie la position/rotation d'origine
				gfx.RotateTransform(-t.getAngle());
				gfx.TranslateTransform(-x, -y);
			}
			this.ball.draw(gfx, e.ClipRectangle.Width, e.ClipRectangle.Height);
		}
	}
}

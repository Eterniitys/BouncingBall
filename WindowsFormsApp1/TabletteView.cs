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

		private int room_width;
		private int room_lenght;

		public TabletteView(int room_width , int room_lenght) {
			this.room_width = room_width;
			this.room_lenght = room_lenght;
			Tablet tab = new Tablet(0, 0, 0, ScreenFormat._24PC);
			this.tab = tab;
			InitializeComponent();
			this.lbl_format.Text = String.Format("Largeur : {0}, Hauteur {1}", tab.getWidth(), tab.getHeight());
			this.pictureBox1.MouseWheel += new MouseEventHandler(onMouseWheel);
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

			// Rectangle rouge englobant au format de la tablette
			gfx.DrawRectangle(redPen, 0, 0, dim_x, dim_y);
			// place le centre de l'image au centre de la tablette
			gfx.TranslateTransform(-(x - dim_x / 2), -(y - dim_y / 2));

			// rotation par le centre de la tablette
			gfx.TranslateTransform(x, y);
			gfx.RotateTransform(-this.tab.getAngle());
			gfx.TranslateTransform(-x, -y);

			// cadre entourant la salle de jeux
			gfx.DrawLine(Pens.Blue, 0, 0, room_width, 0);
			gfx.DrawLine(Pens.Blue, 0, room_lenght, room_width, room_lenght);
			gfx.DrawLine(Pens.Blue, 0, 0, 0, room_lenght);
			gfx.DrawLine(Pens.Blue, room_width, 0, room_width, room_lenght);

			// balle
			this.tab.ball.draw(gfx, room_width, room_lenght);
		}
		#endregion Painting

		public Tablet getTablette() {
			return this.tab;
		}

		#region DEV TOOLS BELOW, SHOULD BE UNUSED IN RELEASE

		private enum Mode {
			drawing,
			moving
		}

		private Mode mode = Mode.moving;

		public void labelToMousePos(MouseEventArgs e) {
			Invoke(new Action(() => {
				this.lbl.Text = String.Format("{0:#.#} | {1:#.#}", this.tab.ball.center.X, this.tab.ball.center.Y);
			}));
		}

		private int prev_x = 0;
		private int prev_y = 0;

		private bool clickIsDown = false;
		private void onMouseWheel(object sender, MouseEventArgs e) {
			this.tab.setAngle(this.tab.getAngle() + e.Delta / 10);
			Invoke(new Action(() => {
				this.lbl.Text = String.Format("{0:#.#}", e.Delta / 10);
			}));
		}

		private void pictureBox1_MouseUp(object sender, MouseEventArgs e) {
			if (mode == Mode.moving) {
				clickIsDown = false;
			}
		}

		private void pictureBox1_MouseMove(object sender, MouseEventArgs e) {
			if (clickIsDown && mode == Mode.moving) {
				this.tab.moveBy((e.X - this.tab.getWidth() / 2) - prev_x, (e.Y - this.tab.getHeight() / 2) - prev_y);
				prev_x = e.X - this.tab.getWidth() / 2;
				prev_y = e.Y - this.tab.getHeight() / 2;
			}
		}

		private void pictureBox1_MouseDown(object sender, MouseEventArgs e) {
			if (mode == Mode.moving) {
				prev_x = e.X - this.tab.getWidth() / 2;
				prev_y = e.Y - this.tab.getHeight() / 2;
				clickIsDown = true;
			}
		}

		private void TabletteView_KeyUp(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.T) {
				mode = mode == Mode.moving ? Mode.drawing : Mode.moving;
			}
		}

		#endregion DEV TOOLS BELOW, SHOULD BE UNUSED IN RELEASE

	}
}

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

namespace BouncingBall
{
	public partial class TabletteView : Form
	{
		public TabletteView()
		{
			Tablet tab = new Tablet(0, 0, 0, ScreenFormat._24PC);
			//TODO la valeur du format ne semble pas etre prise en compte partout
			this.tab = tab;
			InitializeComponent();
			this.lbl_format.Text = String.Format("Largeur : {0}, Hauteur {1}", tab.getWidth(), tab.getHeight());
		}

		#region Painting
		private void timer_Tick(object sender, EventArgs e)
		{
			this.pictureBox1.Invalidate();
			Invoke(new Action(() =>
			{
				this.lbl.Text = String.Format("{0:#.#} | {1:#.#}", this.tab.ball.center.X, this.tab.ball.center.Y);
			}));
		}

		private void pictureBox1_Paint(object sender, PaintEventArgs e)
		{
			//TODO
			int room_width = 1600;
			int room_lenght = 900;
			// - - - - - - - -
			Graphics gfx = e.Graphics;

			Pen blackPen = new Pen(Color.FromArgb(255, 0, 0, 0), 2);
			Pen redPen = new Pen(Color.FromArgb(255, 255, 0, 0), 2);
			Pen bluePen = new Pen(Color.FromArgb(255, 0, 0, 255), 2);
			//
			int dim_x = this.tab.getWidth();
			int dim_y = this.tab.getHeight();
			int x = this.tab.getPosX();
			int y = this.tab.getPosY();


			gfx.DrawLine(Pens.Black, (dim_x / 2)-5, dim_y / 2, (dim_x / 2) + 5, dim_y / 2);
			gfx.DrawLine(Pens.Black, dim_x / 2, (dim_y / 2)-5, dim_x / 2, (dim_y / 2) + 5);

			//Matrix m1 = new Matrix();
			//m1.Translate(dim_x / 2, dim_y/2);
			//m1.Rotate(this.tab.getAngle());

			/*
			gfx.TranslateTransform(x, y);
			gfx.TranslateTransform(-x, -y);
			*/

			//Rectangle rouge englobant au format de la tablette
			gfx.DrawRectangle(redPen, 0, 0, dim_x, dim_y);
			//place le centre de l'image au centre de la tablette
			gfx.TranslateTransform(-(x-dim_x/2), -(y-dim_y/2));  

			// # # # # # # # # # # # # # # # # # # # # # # # # # #

			gfx.TranslateTransform(x, y);
			gfx.RotateTransform(-this.tab.getAngle());
			gfx.TranslateTransform(-x, -y);

			// # # # # # # # # # # # # # # # # # # # # # # # # # #

			// cadre entourant la salle de jeux
			gfx.DrawLine(Pens.Blue, 0, 0, room_width, 0);
			gfx.DrawLine(Pens.Blue, 0, room_lenght, room_width, room_lenght);
			gfx.DrawLine(Pens.Blue, 0, 0, 0, room_lenght);
			gfx.DrawLine(Pens.Blue, room_width, 0, room_width, room_lenght);

			// balle
			this.tab.ball.draw(gfx, room_width, room_lenght);               
			
			/*
			// définition de l'origine de rotation

			gfx.RotateTransform(-this.tab.getAngle());
			gfx.DrawRectangle(redPen, x - dim_x / 2, y - dim_y / 2, dim_x, dim_y);

			gfx.RotateTransform(this.tab.getAngle());
			gfx.RotateTransform(this.tab.getAngle());
			*/
		}
		#endregion

		#region OtherEvents

		private void spin_btn_x_ValueChanged(object sender, EventArgs e)
		{
			this.tab.setPosX((int)this.spin_btn_x.Value);
		}

		private void spin_btn_y_ValueChanged(object sender, EventArgs e)
		{
			this.tab.setPosY((int)this.spin_btn_y.Value);
		}
		private void spin_btn_angle_ValueChanged(object sender, EventArgs e)
		{
			this.tab.setAngle(Convert.ToSingle(this.spin_btn_angle.Value));
		}

		#endregion

		public Tablet getTablette()
		{
			return this.tab;
		}
	}
}

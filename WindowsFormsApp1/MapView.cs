using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BouncingBall
{
	public partial class MapView : Form
	{
		/// <summary>
		/// Width of the playing area in millimeters
		/// </summary>
		private int room_width;
		/// <summary>
		/// Depth of the playing area in millimeters
		/// </summary>
		private int room_depth;
		/// <summary>
		/// List of all tablets in the game
		/// </summary>
		private Tablet[] lst_tab;

		public MapView(Tablet t)
		{
			//hardcoded TODO add a way to tweak them
			room_width = 1600;
			room_depth = 900;
			// - - - - - - - - - -
			lst_tab = new Tablet[1];
			lst_tab[0] = t;

			InitializeComponent();
		}

		/// <summary>
		/// Déclenche une boucle de jeu
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timer_Tick(object sender, EventArgs e)
		{
			this.pictureBox1.Invalidate();
		}

		private void pictureBox1_Paint(object sender, PaintEventArgs e)
		{
			foreach(Tablet t in lst_tab)
			{
				int dim_x = (this.lst_tab[0].getWidth() * e.ClipRectangle.Width ) / room_width;
				int dim_y = (this.lst_tab[0].getHeight() * e.ClipRectangle.Height) / room_depth;
				int x = (t.getPosX() * e.ClipRectangle.Width) / room_width;
				int y = (t.getPosY() * e.ClipRectangle.Height) / room_depth;
				Pen blackPen = new Pen(Color.FromArgb(255, 0, 0, 0), 2);
				Pen redPen = new Pen(Color.FromArgb(255, 255, 0, 0), 2);
				Pen bluePen = new Pen(Color.FromArgb(255, 0, 0, 255), 10);
				e.Graphics.DrawLine(redPen, 0, 0, x, y);
				e.Graphics.DrawRectangle(redPen, x, y, dim_x, dim_y);
				// définition du point de rotation (origine) TODO tourner autour du centre
				e.Graphics.TranslateTransform(x + dim_x / 2, y + dim_y / 2);
				// fait tourner le futur dessin
				e.Graphics.RotateTransform(-Convert.ToSingle(t.getAngle()));

				//Dimmension en X
				e.Graphics.DrawRectangle(blackPen, -dim_x / 2, -dim_y / 2, dim_x, dim_y);
			}
		}
	}
}

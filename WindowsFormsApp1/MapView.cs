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
		/// 
		/// </summary>
		private int screen_width;
		/// <summary>
		/// 
		/// </summary>
		private int screen_higth;
		/// <summary>
		/// List of all tablets in the game
		/// </summary>
		private Tablet[] lst_tab;

		public MapView(Tablet t)
		{
			//hardcoded TODO add a way to tweak them
			room_width = 8000;
			room_depth = 6000;
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
				Pen blackPen = new Pen(Color.FromArgb(255, 0, 0, 0), 2);
				Pen redPen = new Pen(Color.FromArgb(255, 255, 0, 0), 2);
				e.Graphics.DrawLine(redPen, 0, 0, t.getPosX(), t.getPosY());
				e.Graphics.DrawRectangle(redPen, t.getPosX(), t.getPosY(), 100, 50);
				//définition du point de rotation (origine) TODO tourner autour du centre
				//e.Graphics.TranslateTransform(0, 0);
				// fait tourner le futur dessin
				e.Graphics.TranslateTransform(t.getPosX(), t.getPosY());
				e.Graphics.RotateTransform(-Convert.ToSingle(t.getAngle()));
				e.Graphics.DrawRectangle(blackPen, 0, 0, 100, 50);
			}
		}
	}
}

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
	public partial class TabletteView : Form
	{
		public TabletteView()
		{
			this.tab = new Tablette(0,0,0);

			InitializeComponent();
		}

		private void spin_btn_x_ValueChanged(object sender, EventArgs e)
		{
			System.Console.Write(e);
		}

		private void spin_btn_y_ValueChanged(object sender, EventArgs e)
		{

		}
		private void spin_btn_angle_ValueChanged(object sender, EventArgs e)
		{

		}

		public Tablette getTablette()
		{
			return this.tab;
		}

	}
}

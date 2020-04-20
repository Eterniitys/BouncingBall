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
			this.tab = new Tablet(0,0,0);

			InitializeComponent();
		}

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
			this.tab.setAngle(this.spin_btn_angle.Value);
		}

		public Tablet getTablette()
		{
			return this.tab;
		}

	}
}

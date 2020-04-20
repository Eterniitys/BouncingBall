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
		private Tablette[] lst_tab;

		public MapView(Tablette t)
		{
			InitializeComponent();
		}
	}
}

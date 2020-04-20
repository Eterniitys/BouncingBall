using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BouncingBall
{
	public class Tablette
	{
		private int pos_x;
		private int pos_y;
		private double angle;

		public Tablette(int pos_x, int pos_y, double angle)
		{
			// TODO Une tablette a des dimensions
			this.pos_x = pos_x;
			this.pos_y = pos_y;
			this.angle = angle;
		}
	}
}

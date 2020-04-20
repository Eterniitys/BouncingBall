using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BouncingBall
{
	public class Tablet
	{

		private int pos_x;
		private int pos_y;
		private decimal angle;

		public Tablet(int pos_x, int pos_y, decimal angle)
		{
			// TODO Une tablette a des dimensions
			this.pos_x = pos_x;
			this.pos_y = pos_y;
			this.angle = angle;
		}

		#region Ascesseurs
		public int getPosX()
		{
			return this.pos_x;
		}

		public int getPosY()
		{
			return this.pos_y;
		}

		public decimal getAngle()
		{
			return this.angle;
		}

		public void setPosX(int pos_x)
		{
			this.pos_x = pos_x;
		}

		public void setPosY(int pos_y)
		{
			this.pos_y = pos_y;
		}

		public void setAngle(decimal angle)
		{
			this.angle = angle;
		}

		#endregion
	}
}

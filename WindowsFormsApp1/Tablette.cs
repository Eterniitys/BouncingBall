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

		private ScreenFormat format;

		public Tablet(int pos_x, int pos_y, decimal angle, ScreenFormat format)
		{
			// TODO Une tablette a des dimensions
			this.pos_x = pos_x;
			this.pos_y = pos_y;
			this.angle = angle;
			this.format = format;
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

		public int getWidth()
		{
			return Format.GetFormat(this.format)[0];
		}

		public int getHeight()
		{
			return Format.GetFormat(this.format)[1];
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

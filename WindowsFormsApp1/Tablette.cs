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

		/// <summary>
		/// Create a tablet simple representation 
		/// </summary>
		/// <param name="pos_x">The position in the room along the x axis</param>
		/// <param name="pos_y">The position in the room along the y axis</param>
		/// <param name="angle">The orientation on the tablet</param>
		/// <param name="format">Define the format use to represent this tablet in the room</param>
		public Tablet(int pos_x, int pos_y, decimal angle, ScreenFormat format)
		{
			this.pos_x = pos_x;
			this.pos_y = pos_y;
			this.angle = angle;
			this.format = format;
		}

		#region Ascesseurs
		/// <summary>
		/// Get position in the room along the x axis
		/// </summary>
		/// <returns></returns>
		public int getPosX()
		{
			return this.pos_x;
		}

		/// <summary>
		/// Get position in the room along the y axis
		/// </summary>
		/// <returns></returns>
		public int getPosY()
		{
			return this.pos_y;
		}

		/// <summary>
		/// Get the orientation in the room
		/// </summary>
		/// <returns></returns>
		public decimal getAngle()
		{
			return this.angle;
		}

		/// <summary>
		/// Get the width of the sreen depending on the <see cref="format"/>
		/// </summary>
		/// <returns></returns>
		public int getWidth()
		{
			return Format.GetFormat(this.format)[0];
		}

		/// <summary>
		/// Get the height of the sreen depending on the <see cref="format"/>
		/// </summary>
		/// <returns></returns>
		public int getHeight()
		{
			return Format.GetFormat(this.format)[1];
		}

		/// <summary>
		/// set position in the room along the x axis
		/// </summary>
		/// <param name="pos_x"></param>
		public void setPosX(int pos_x)
		{
			this.pos_x = pos_x;
		}

		/// <summary>
		/// set position in the room along the y axis
		/// </summary>
		/// <param name="pos_y"></param>
		public void setPosY(int pos_y)
		{
			this.pos_y = pos_y;
		}

		/// <summary>
		/// Set the orientation in the room
		/// </summary>
		/// <param name="angle">In degree</param>
		public void setAngle(decimal angle)
		{
			this.angle = angle;
		}

		#endregion
	}
}

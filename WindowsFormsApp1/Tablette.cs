using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BouncingBall
{
	public class Tablet
	{
		/// <summary>
		/// tablet position in the room along the x axis
		/// </summary>
		private int pos_x;
		/// <summary>
		/// tablet position in the room along the y axis
		/// </summary>
		private int pos_y;
		/// <summary>
		/// The orientation on the talette
		/// </summary>
		private float angle;
		/// <summary>
		/// 
		/// </summary>
		private ScreenFormat format;

		private Matrix matrix;

		public Ball ball;

		/// <summary>
		/// Create a tablet simple representation 
		/// </summary>
		/// <param name="pos_x">The position in the room along the x axis</param>
		/// <param name="pos_y">The position in the room along the y axis</param>
		/// <param name="angle">The orientation on the tablet</param>
		/// <param name="format">Define the format use to represent this tablet in the room</param>
		public Tablet(int pos_x, int pos_y, float angle, ScreenFormat format)
		{
			this.pos_x = pos_x;
			this.pos_y = pos_y;
			this.angle = angle;
			this.format = format;
			this.matrix = new Matrix();
		}

		internal void refreshBall(PointF position, Ball.ImageID imageID)
		{
			if (this.ball != null)
			{
				//lock (this.ball)
				//{
					this.ball.setPosition(position);
					this.ball.setID(imageID);
				//}
			}
			else
			{
				throw new NotImplementedException();
			}
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
		public float getAngle()
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

		public Matrix getTMatrix()
		{
			return this.matrix;
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
		public void setAngle(float angle)
		{
			this.angle = angle;
		}

		#endregion
	}
}

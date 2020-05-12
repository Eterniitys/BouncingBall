using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BouncingBall {
	public class Tablet {
		/// <summary>
		/// tablet position in the room along the x axis and the y axis
		/// </summary>
		private Point position;
		/// <summary>
		/// The orientation on the talette
		/// </summary>
		private float angle;
		/// <summary>
		/// 
		/// </summary>
		public ScreenFormat format { get; }

		public Ball ball;

		/// <summary>
		/// Create a tablet simple representation 
		/// </summary>
		/// <param name="angle">The orientation on the tablet</param>
		/// <param name="format">Define the format use to represent this tablet in the room</param>
		public Tablet(int pos_x, int pos_y, float angle, ScreenFormat format) {
			this.position = new Point(pos_x, pos_y);
			this.angle = angle;
			this.format = format;
		}

		internal void moveBy(int delta_x, int delta_y) {
			this.position.X -= delta_x;
			this.position.Y -= delta_y;
		}

		#region Ascesseurs
		public Point getPosition() {
			return this.position;
		}

		/// <summary>
		/// Get position in the room along the x axis
		/// </summary>
		/// <returns></returns>
		public int getPosX() {
			return this.position.X;
		}

		/// <summary>
		/// Get position in the room along the y axis
		/// </summary>
		/// <returns></returns>
		public int getPosY() {
			return this.position.Y;
		}

		/// <summary>
		/// Get the orientation in the room
		/// </summary>
		/// <returns></returns>
		public float getAngle() {
			return this.angle;
		}

		/// <summary>
		/// Get the width of the sreen depending on the <see cref="format"/>
		/// </summary>
		/// <returns></returns>
		public int getWidth() {
			return Format.GetFormat(this.format)[0];
		}

		/// <summary>
		/// Get the height of the sreen depending on the <see cref="format"/>
		/// </summary>
		/// <returns></returns>
		public int getHeight() {
			return Format.GetFormat(this.format)[1];
		}

		/// <summary>
		/// set position in the room along the x axis
		/// </summary>
		/// <param name="pos_x"></param>
		public void setPosX(int pos_x) {
			this.position.X = pos_x;
		}

		/// <summary>
		/// set position in the room along the y axis
		/// </summary>
		/// <param name="pos_y"></param>
		public void setPosY(int pos_y) {
			this.position.Y = pos_y;
		}

		/// <summary>
		/// Set the orientation in the room
		/// </summary>
		/// <param name="angle">In degree</param>
		public void setAngle(float angle) {
			this.angle = angle;
		}


		#endregion
	}
}

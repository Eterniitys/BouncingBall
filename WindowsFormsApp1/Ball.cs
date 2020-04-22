﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BouncingBall
{
	public class Ball
	{
		#region Properties of a ball
		/// <summary>
		/// All possible value of <see cref="state"/></see>
		/// </summary>
		public enum ImageID : int {
			BOUNCE,
			CATCH
		}
		/// <summary>
		/// Use in drawing function to draw the right picture depending on its value.
		/// </summary>
		private ImageID state;

		/// <summary>
		/// Use in drawing function to draw the right picture depending on <see cref="state"/></see> value.
		/// </summary>
		private readonly static Bitmap[] lst_img = {
				Properties.Resources.ballBOUNCE,
				Properties.Resources.ballCATCH
			};

		/// <summary>
		/// The center of the ball
		/// </summary>
		public PointF center; //TODO set private
		/// <summary>
		/// The room width
		/// </summary>
		private int room_width;
		/// <summary>
		/// The room lenght
		/// </summary>
		private int room_lenght;
		/// <summary>
		/// The size of the ball
		/// </summary>
		private SizeF size;
		/// <summary>
		/// The angle which the ball is moving with
		/// </summary>
		public int direction; //TODO set private
		/// <summary>
		/// The speed which the ball is moving with
		/// </summary>
		private int speed;
		#endregion

		#region Constructor
		/// <summary>
		/// Create a Ball in a defined room with a scale factor
		/// </summary>
		/// <param name="room_width">The width of the represented room in millimeters</param>
		/// <param name="room_lenght">The lenght of the represented room in millimeters</param>
		/// <param name="scale">Used to scale the ball size</param>
		public Ball(int room_width, int room_lenght , float scale = 1)
		{
			this.size = new SizeF(scale * 100F, scale * 100F); // a 10 centimeter diameter ball
			this.room_width = room_width;
			this.room_lenght = room_lenght;
			this.state = ImageID.CATCH;
			this.speed = 2; // TODO this value does need to not be hardcoded
			Random rnd = new Random();
			this.center = new PointF(
				(float)rnd.NextDouble() * (room_width - this.size.Width) + this.size.Width/2,
				(float)rnd.NextDouble() * (room_lenght - this.size.Height) + this.size.Height/2
				);
			this.direction = rnd.Next(-180, 180);
		}

		#endregion

		#region Drawing function
		/// <summary>
		/// Draw the referenced ball correctly scaled depending on the area and the room where the ball evolving.
		/// </summary>
		/// <param name="gfx">The Graphics of the component drawing the ball</param>
		/// <param name="window_width">The drawing component width</param>
		/// <param name="window_height">The drawing component height</param>
		public void draw(Graphics gfx, int window_width, int window_height)
		{
			SizeF scaled_size = new SizeF(
				this.size.Width * window_width / room_width,
				this.size.Height * window_height / room_lenght
				);
			PointF scaled_pos = new PointF(
				(this.center.X - this.size.Width / 2) * window_width / room_width,
				(this.center.Y - this.size.Height / 2) * window_height / room_lenght
				);
			RectangleF rect = new RectangleF(scaled_pos, scaled_size);
			gfx.DrawImage(lst_img[(int)this.state], rect);
			Pen bluePen = new Pen(Color.FromArgb(255, 0, 0, 255), 1);
			gfx.DrawArc(bluePen, rect, 0, 360);
			PointF tmp_p = new PointF(scaled_pos.X + scaled_size.Width, scaled_pos.Y + scaled_size.Height);
			gfx.DrawLine(bluePen, scaled_pos, tmp_p);
			//
		}
		#endregion

		#region Movement / Bouncing
		/// <summary>
		/// Move the ball in the field and manage its boucing on borders and bars.
		/// </summary>
		public void move()
		{
			double radDir = (this.direction+90) * (Math.PI / 180);
			center.X += (float)(Math.Sin(radDir) * speed);
			center.Y += (float)(Math.Cos(radDir) * speed);
			borderBounce();
		}

		/// <summary>
		/// Bounce the ball on the borders
		/// </summary>
		private void borderBounce()
		{
			float radius =  this.size.Width / 2;
			if (center.Y - radius < 0 || center.Y + radius > room_lenght)
			{
				direction = -direction;
			}

			if (center.X - radius < 0 || center.X + radius > room_width)
			{
				direction = 180-direction;
			}

			direction = direction < -180 ? direction + 360 : direction;
			direction = direction > 180 ? direction - 360 : direction;
		}

		#region fonctions not use yet
		/// <summary>
		/// Bounce the ball on the bars in the field.
		/// </summary>
		/*private void barBounce()
		{
			#region Delete "obsolete" bars
			for (int i = 0; i < bars.Count; i++)
			{
				if (bars[i].duration == 0)
				{
					bars.RemoveAt(i);
					i--;
				}
			}
			#endregion
			foreach (BounceBar bar in bars)
			{
				Point A = bar.A;
				PointF rC = bar.rotatePoint(center);    // Rotate ball center by A with theta angle

				if (((A.X <= rC.X && rC.X <= bar.rB.X) || (bar.rB.X <= rC.X && rC.X <= A.X))                    // If ball aligned between A and B
					&& (rC.Y + radius > A.Y - bar.duration / 12) && (rC.Y - radius < A.Y + bar.duration / barDivider))    // And distance to bar lower than radius (collision)
				{
					int tmpDir = (int)(direction + bar.theta * 180 / Math.PI) % 360;    // Rotated direction of ball
					if (tmpDir < 180)           // 0<->180 degree
					{
						rC.Y = rC.Y - 2 * (radius - Math.Abs(A.Y - rC.Y - bar.duration / barDivider));
					}
					else if (tmpDir > 180)      // 180<->360 degree
					{
						rC.Y = rC.Y + 2 * (radius - Math.Abs(rC.Y - A.Y - bar.duration / barDivider));
					}
					tmpDir = 360 - tmpDir;
					setDirection((int)(tmpDir - bar.theta * 180 / Math.PI));    // Set the new direction
					center = bar.rotatePointReverse(rC);                           // Rotate back the center
				}

				bar.duration--;
			}
		}*/
		#endregion

		#endregion
	}
}

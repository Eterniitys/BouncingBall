using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BouncingBall
{
	public class Ball
	{
		public enum ImageID : int {
			BOUNCE,
			CATCH
		}

		private readonly static Bitmap[] lst_img = {
				Properties.Resources.ballBOUNCE,
				Properties.Resources.ballCATCH
			};

		private Point center;
		private int room_width;
		private int room_long;
		private Size size;
		private decimal scale;
		private ImageID state;
		public int direction; //TODO set private
		private int speed;
		// - - - - - - - - - - - - - - 
		public Ball(int room_width, int room_long , decimal scale = 1)
		{
			this.size = new Size((int)(10 * scale), (int)(10 * scale));
			this.room_width = room_width;
			this.room_long = room_long;
			this.scale = scale;
			this.state = ImageID.CATCH;
			this.speed = 10;
			Random rnd = new Random();
			this.center = new Point(
				rnd.Next(room_width - this.size.Width) + this.size.Width/2,
				rnd.Next(room_long - this.size.Height) + this.size.Height/2
				);
			this.direction = rnd.Next(180);
		}

		public void draw(Graphics gfx, int window_width, int window_height)
		{

			Point scaled_pos = new Point(
				(this.center.X - this.size.Width / 2) * window_width / room_width,
				(this.center.Y - this.size.Height / 2) * window_height / room_long
				);
			Rectangle rect = new Rectangle(scaled_pos, this.size);
			// TODO remove circle
			Pen bluePen = new Pen(Color.FromArgb(255, 0, 0, 255), 1);
			gfx.DrawArc(bluePen, rect, 0, 360);
			//
			gfx.DrawImage(lst_img[(int)this.state], rect);
		}

		#region Movement / Bouncing
		/// <summary>
		/// Move the ball in the field and manage its boucing on borders and bars.
		/// </summary>
		public void move()
		{
			int radius = this.size.Width / 2;
			double radDir = (this.direction+90) * (Math.PI / 180);
			center.X += (int)(Math.Sin(radDir) * speed);
			center.Y += (int)(Math.Cos(radDir) * speed);
			borderBounce();
		}

		/// <summary>
		/// Bounce the ball on the borders
		/// </summary>
		private void borderBounce()
		{
			int radius = this.size.Width / 2;
			if (center.Y < 0 || center.Y > room_long)
			{
				direction = -direction;
			}

			if (center.X < 0 || center.X > room_width)
			{
				direction = 180-direction;
			}

			direction = direction < -180 ? direction + 360 : direction;
			direction = direction > 180 ? direction - 360 : direction;
		}

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
	}
}

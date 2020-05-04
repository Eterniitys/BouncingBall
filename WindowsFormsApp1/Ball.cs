using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BouncingBall {
	public class Ball : GameObject {

		public delegate void BallMovedHandler(PointF pos);
		public event BallMovedHandler onBallMoved;

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
		//TODO set private
		public PointF center;
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
		//TODO set private
		public int direction { get; set; }
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
		public Ball(int room_width, int room_lenght, float scale = 1) {
			this.size = new SizeF(scale * 25F, scale * 25F); // a 10 centimeter diameter ball
			this.room_width = room_width;
			this.room_lenght = room_lenght;
			this.state = ImageID.CATCH;
			this.speed = 5; // TODO this value does need to not be hardcoded
			Random rnd = new Random(); // 103 108
			this.center = new PointF(
				(float)rnd.NextDouble() * (room_width - this.size.Width) + this.size.Width / 2,
				(float)rnd.NextDouble() * (room_lenght - this.size.Height) + this.size.Height / 2
				);
			this.direction = rnd.Next(-180, 180);
		}

		#endregion

		#region Ascesseurs

		/// <summary>
		/// Get the center position of the ball
		/// </summary>
		/// <returns></returns>
		public PointF getPosition() {
			return this.center;
		}

		/// <summary>
		/// Get the ID to use to draw the ball
		/// </summary>
		/// <returns></returns>
		public ImageID getID() {
			return this.state;
		}

		public void setPosition(PointF pos) {
			this.center = pos;
		}

		public void setID(ImageID id) {
			this.state = id;
		}
		#endregion

		#region Drawing function
		/// <summary>
		/// Draw the referenced ball correctly scaled depending on the area and the room where the ball evolving.
		/// </summary>
		/// <param name="gfx">The Graphics of the component drawing the ball</param>
		/// <param name="window_width">The drawing component width</param>
		/// <param name="window_height">The drawing component height</param>
		public override void draw(Graphics gfx, PointF scale) {
			SizeF scaled_size = new SizeF(
				this.size.Width * scale.X,
				this.size.Height * scale.Y
				);
			PointF scaled_pos = new PointF(
				(this.center.X - this.size.Width / 2) * scale.X,
				(this.center.Y - this.size.Height / 2) * scale.Y
				);
			RectangleF rect = new RectangleF(scaled_pos, scaled_size);
			lock (lst_img) {
				gfx.DrawImage(lst_img[(int)this.state], rect);
			}
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
		public override void move(GameObject[] colliders) {
			double radDir = (this.direction + 90) * (Math.PI / 180);
			center.X += (float)(Math.Sin(radDir) * speed);
			center.Y += (float)(Math.Cos(radDir) * speed);
			borderBounce();
			wallBounce(colliders);
		}

		/// <summary>
		/// Bounce the ball on the borders
		/// </summary>
		private void borderBounce() {
			float radius = this.size.Width / 2;
			if (center.Y - radius < 0 || center.Y + radius > room_lenght) {
				direction = -direction;
			}

			if (center.X - radius < 0 || center.X + radius > room_width) {
				direction = 180 - direction;
			}
			
			direction += 180;
			direction %= 360;
			direction -= 180;
			
			onBallMoved?.Invoke(center);
		}

		/// <summary>
		/// Bounce the ball on the bars in the field.
		/// </summary>
		private void wallBounce(GameObject[] colliders) {
			bool hasCollided = false;

			foreach (GameObject gameObject in colliders) {
				bool isColliding = false;
				if (gameObject is Wall wall) {
					float minimalDist = (this.size.Width + wall.rectangle.Size.Height) / 2;
					isColliding = wallCollision(wall , minimalDist);
					if (isColliding) {
						int alpha = direction;
						int beta = (int)wall.angle;
						direction = -(alpha - beta) + beta;
					} else {
						isColliding = sideCollision(wall.getOrigine(), minimalDist);
						isColliding |= sideCollision(wall.getEnd(), minimalDist);
						if (isColliding) {
							direction += 180;
						}
					}
					hasCollided |= isColliding;
				}
			}
			this.setID(hasCollided ? ImageID.BOUNCE : ImageID.CATCH);
		}

		private bool axisCollision(Wall wall, float gap) {
			PointF A = wall.getOrigine();
			PointF B = wall.getEnd();

			PointF u = new PointF(A.X - B.X, A.Y - B.Y);
			PointF AC = new PointF(A.X - this.center.X, A.Y - this.center.Y);

			float numerator = u.X * AC.Y - u.Y * AC.X;
			numerator = numerator < 0 ? -numerator : numerator;
			float denominator = (float)Math.Sqrt(u.X * u.X + u.Y * u.Y);
			float CI = numerator / denominator;

			return CI < gap;
		}

		private bool wallCollision(Wall wall, float gap) {
			PointF A = wall.getOrigine();
			PointF B = wall.getEnd();

			if (axisCollision(wall, gap) == false)
				return false;  // si on ne touche pas la droite, on ne touchera jamais le segment

			PointF AB = new PointF(B.X - A.X, B.Y - A.Y);
			PointF AC = new PointF(this.center.X - A.X, this.center.Y - A.Y);
			PointF BC = new PointF(this.center.X - B.X, this.center.Y - B.Y);

			float pscal1 = AB.X * AC.X + AB.Y * AC.Y;  // produit scalaire
			float pscal2 = (-AB.X) * BC.X + (-AB.Y) * BC.Y;  // produit scalaire
			if (pscal1 >= 0 && pscal2 >= 0)
				return true;   // I entre A et B, ok.
							   // dernière possibilité, A ou B dans le cercle

			return false;
		}

		private bool sideCollision(PointF A, float gap) {
			float d = (A.X - center.X) * (A.X - center.X) + (A.Y - center.Y) * (A.Y - center.Y);
			return d <= gap * gap;
		}
		#endregion
	}
}

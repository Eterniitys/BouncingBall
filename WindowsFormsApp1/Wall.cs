using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BouncingBall {
	public class Wall : GameObject {

		#region Variables
		/// <summary>
		/// The Origin of a Wall
		/// </summary>
		private PointF origin;
		/// <summary>
		/// The End of a Wall
		/// </summary>
		private PointF end;
		/// <summary>
		/// The rectangle representing the wall
		/// </summary>
		public RectangleF rectangle;
		/// <summary>
		/// The angle of the wall, taked from <see cref="origin"/> to <see cref="end"/>
		/// </summary>
		public float angle { get; private set; }
		/// <summary>
		/// If false, the Wall is previsualize. Otherwise it is drawn normali
		/// </summary>
		private bool built = false;
		#endregion Variables

		#region Constructors
		/// <summary>
		/// Create a wall using 2 PointF
		/// </summary>
		/// <param name="origin">The Origin of a Wall</param>
		/// <param name="end">The End of a Wall</param>
		public Wall(PointF origin, PointF end) {
			this.origin = origin;
			this.end = end;
			this.rectangle = new RectangleF();
			processAngle();
		}
		/// <summary>
		/// Create a wall using 4 coordinate
		/// </summary>
		/// <param name="origin_x">The X value of the origin</param>
		/// <param name="origin_y">The Y value of the origin</param>
		/// <param name="end_x">The X value of the end</param>
		/// <param name="end_y">The Y value of the end</param>
		public Wall(float origin_x, float origin_y, float end_x, float end_y) {
			this.origin = new PointF(origin_x, origin_y);
			this.end = new PointF(end_x, end_y);
			this.rectangle = new RectangleF();
			processAngle();
		}
		/// <summary>
		/// Use a formatted string to create a Wall => the four coordinates origin and end separated by semicolon
		/// </summary>
		/// <param name="message">Formatted string</param>
		public Wall(string message) {
			string[] coord = message.Split(';');
			this.origin = new PointF(float.Parse(coord[0]), float.Parse(coord[1]));
			this.end = new PointF(float.Parse(coord[2]), float.Parse(coord[3]));
			this.rectangle = new RectangleF();
			processAngle();
		}
		#endregion Constructors

		#region Paint / draw
		/// <summary>
		/// Draw the wall
		/// </summary>
		/// <param name="gfx"></param>
		/// <param name="scale"></param>
		public override void draw(Graphics gfx, PointF scale) {
			if (!built) {
				Pen bluePen = new Pen(Color.FromArgb(255, 0, 0, 255), 5);
				bluePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
				gfx.DrawLine(bluePen, this.origin, this.end);
			} else {
				PointF scaledP1 = new PointF(this.origin.X * scale.X, this.origin.Y * scale.Y);
				PointF scaledP2 = new PointF(this.end.X * scale.X, this.end.Y * scale.Y);
				float tickness = 10 * scale.Y;
				int scaledDist = (int)Math.Sqrt(Math.Pow((double)(scaledP1.X - scaledP2.X), 2) + Math.Pow((double)(scaledP1.Y - scaledP2.Y), 2));
				if (scaledDist != 0) {
					this.rectangle.Location = new PointF(0, -tickness / 2);
					this.rectangle.Size = new Size(scaledDist, (int)(tickness));
					gfx.TranslateTransform(scaledP1.X, scaledP1.Y);
					gfx.RotateTransform(-angle);
					gfx.FillRectangle(Brushes.Maroon, this.rectangle);
					gfx.RotateTransform(angle);
					gfx.TranslateTransform(-scaledP1.X, -scaledP1.Y);
				}
				gfx.FillEllipse(Brushes.Maroon, scaledP1.X - tickness / 2, scaledP1.Y - tickness / 2, tickness, tickness);
				gfx.FillEllipse(Brushes.Maroon, scaledP2.X - tickness / 2, scaledP2.Y - tickness / 2, tickness, tickness);
			}

		}

		/// <summary>
		/// Process the angle made by <see cref="origin"/> and <see cref="end"/>
		/// </summary>
		public void processAngle() {
			int dist = (int)Math.Sqrt(Math.Pow((double)(origin.X - end.X), 2) + Math.Pow((double)(origin.Y - end.Y), 2));
			double sinus = (double)(origin.Y - end.Y) / dist;
			sinus = sinus > 1 ? 1 : sinus < -1 ? -1 : sinus;
			float angle = (float)(Math.Asin(sinus) * 180 / Math.PI);
			angle = origin.X > end.X ? angle < 0 ? -angle - 180 : 180 - angle : angle;
			this.angle = angle;
		}

		/// <summary>
		/// Apply the matrix to <see cref="origin"/> and <see cref="end"/>
		/// </summary>
		/// <param name="matrix"></param>
		internal void tranform(Matrix matrix) {
			PointF[] pts = { this.origin, this.end };
			matrix.TransformPoints(pts);
			this.origin = pts[0];
			this.end = pts[1];
		}

		/// <summary>
		/// Inherited from <seealso cref="GameObject"/>. Do nothing.
		/// </summary>
		/// <param name="colliders"></param>
		public override void move(GameObject[] colliders) {
			// A wall do not move
		}
		#endregion Paint / draw

		#region Accessors
		/// <summary>
		/// Return <see cref="origin"/>
		/// </summary>
		/// <returns></returns>
		public PointF getOrigine() {
			return this.origin;
		}
		/// <summary>
		/// Return <see cref="end"/>
		/// </summary>
		/// <returns></returns>
		public PointF getEnd() {
			return this.end;
		}
		/// <summary>
		/// set the value of <see cref="origin"/>
		/// </summary>
		/// <param name="p"></param>
		public void setOrigine(PointF p) {
			this.origin = p;
		}
		/// <summary>
		/// set the value of <see cref="origin"/>
		/// </summary>
		/// <param name="x">The X value of the origin</param>
		/// <param name="y">The Y value of the origin</param>
		public void setOrigine(float x, float y) {
			this.origin = new PointF(x, y);
		}
		/// <summary>
		/// set the value of <see cref="end"/>
		/// </summary>
		/// <param name="p"></param>
		public void setEnd(PointF p) {
			this.end = p;
		}
		/// <summary>
		/// set the value of <see cref="end"/>
		/// </summary>
		/// <param name="x">The X value of the end</param>
		/// <param name="y">The Y value of the end</param>
		public void setEnd(float x, float y) {
			this.end = new PointF(x, y);
		}
		/// <summary>
		/// Set the value of <see cref="built"/>
		/// </summary>
		public void setBuilt() {
			this.built = true;
		}
		#endregion Accessors
	}
}

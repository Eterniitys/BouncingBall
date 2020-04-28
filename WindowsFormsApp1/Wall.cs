using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BouncingBall {
	public class Wall : GameObject {
		private PointF p1;
		private PointF p2;
		public Rectangle rectangle;

		private bool built = false;

		public Wall(PointF p1, PointF p2) {
			this.p1 = p1;
			this.p2 = p2;
		}
		public Wall(float p1_x, float p1_y, float p2_x, float p2_y) {
			this.p1 = new PointF(p1_x, p1_y);
			this.p2 = new PointF(p2_x, p2_y);
			this.rectangle = new Rectangle(0, -5, 1, 1);
		}

		public Wall(string message) {
			string[] coord = message.Split(';');
			this.p1 = new PointF(float.Parse(coord[0]), float.Parse(coord[1]));
			this.p2 = new PointF(float.Parse(coord[2]), float.Parse(coord[3]));
			this.rectangle = new Rectangle(0, -5, 1, 1);
		}

		public override void draw(Graphics gfx, PointF scale) {
			if (!built) {
				Pen bluePen = new Pen(Color.FromArgb(255, 0, 0, 255), 5);
				bluePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
				gfx.DrawLine(bluePen, this.p1, this.p2);
			} else {
				PointF scaledP1 = new PointF(this.p1.X * scale.X, this.p1.Y * scale.Y);
				PointF scaledP2 = new PointF(this.p2.X * scale.X, this.p2.Y * scale.Y);
				int scaledDist = (int)Math.Sqrt(Math.Pow((double)(scaledP1.X - scaledP2.X), 2) + Math.Pow((double)(scaledP1.Y - scaledP2.Y), 2));
				if (scaledDist != 0) {
					this.rectangle.Size = new Size(scaledDist, (int)(10 * scale.X));
					double sinus = (double)(scaledP1.Y - scaledP2.Y) / scaledDist;
					sinus = sinus > 1 ? 1 : sinus < -1 ? -1 : sinus;
					float angle = -(float)(Math.Asin(sinus) * 180 / Math.PI);
					angle = scaledP1.X < scaledP2.X ? angle : -angle + 180;
					gfx.TranslateTransform(scaledP1.X, scaledP1.Y);
					gfx.RotateTransform(angle);
					gfx.FillRectangle(Brushes.Maroon, this.rectangle);
					gfx.RotateTransform(-angle);
					gfx.TranslateTransform(-scaledP1.X, -scaledP1.Y);
				}
				gfx.FillEllipse(Brushes.LightPink, scaledP1.X-5, scaledP1.Y-5, 10, 10);
				gfx.FillEllipse(Brushes.DeepPink, scaledP2.X-5, scaledP2.Y-5, 10, 10);
			}

		}
		internal void tranform(Matrix matrix) {
			PointF[] pts = { this.p1, this.p2 };
			matrix.TransformPoints(pts);
			this.p1 = pts[0];
			this.p2 = pts[1];
		}

		public override void move() {
			// A wall do not move
		}

		public PointF getOrigine() {
			return this.p1;
		}

		public PointF getEnd() {
			return this.p2;
		}

		public void setOrigine(PointF p) {
			this.p1 = p;
		}

		public void setOrigine(float x, float y) {
			this.p1 = new PointF(x, y);
		}

		public void setEnd(PointF p) {
			this.p2 = p;
		}

		public void setEnd(float x, float y) {
			this.p2 = new PointF(x, y);
		}

		public void setBuilt() {
			this.built = true;
		}
	}
}

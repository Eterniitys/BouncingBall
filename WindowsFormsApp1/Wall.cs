using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BouncingBall {
	public class Wall : GameObject {
		private PointF p1;
		private PointF p2;

		private bool built = false;

		public Wall(PointF p1, PointF p2) {
			this.p1 = p1;
			this.p2 = p2;
		}
		public Wall(float p1_x, float p1_y, float p2_x, float p2_y) {
			this.p1 = new PointF(p1_x, p1_y);
			this.p2 = new PointF(p2_x, p2_y);
		}

		public override void draw(Graphics gfx, int window_width, int window_height) {
			Pen bluePen = new Pen(Color.FromArgb(255, 0, 0, 255), 10);
			if (!built) {
				bluePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
			}
			gfx.DrawLine(bluePen, p1, p2);
		}

		public override void move() {
			// A wall do not move
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

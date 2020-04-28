using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BouncingBall {
	public abstract class GameObject {
		public abstract void move(GameObject[] colliders);
		public abstract void draw(System.Drawing.Graphics gfx, PointF scale);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BouncingBall {
	public abstract class GameObject {
		public abstract void move();
		public abstract void draw(System.Drawing.Graphics gfx, int window_width, int window_height);
	}
}

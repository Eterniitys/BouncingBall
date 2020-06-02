﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary {
	public abstract class GameObject {
		/// <summary>
		/// Move a <seealso cref="GameObject"/>
		/// </summary>
		/// <param name="colliders"></param>
		public abstract void move();

		public abstract bool collide(GameObject[] colliders);

		/// <summary>
		/// Draw a <seealso cref="GameObject"/>
		/// </summary>
		/// <param name="gfx"></param>
		/// <param name="scale"></param>
		public abstract void draw(System.Drawing.Graphics gfx, PointF scale);
	}
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary {
	public abstract class GameObject {
		/// <summary>
		/// Describe how this <seealso cref="GameObject"/> move
		/// </summary>
		/// <param name="colliders"></param>
		public abstract void move(int room_lenght, int roomWidth);

		/// <summary>
		/// Describe how this <seealso cref="GameObject"/> collide with others
		/// </summary>
		/// <param name="colliders"></param>
		/// <returns>true if collide at least once</returns>
		public abstract bool collide(GameObject[] colliders);

		/// <summary>
		/// Describe how this <seealso cref="GameObject"/> is drawn
		/// </summary>
		/// <param name="gfx"></param>
		/// <param name="scale"></param>
		public abstract void draw(System.Drawing.Graphics gfx, PointF scale);
	}
}

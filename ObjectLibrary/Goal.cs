using BouncingBall.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ObjectLibrary {
	class Goal : GameObject {

		public enum GlobalPos {
			NORTH,
			WEST,
			SOUTH,
			EAST
		}

		public delegate void GoalUpdateHandler();
		public event GoalUpdateHandler onGoalUpdate;

		GlobalPos anchor { get; set; }
		Point center { get; }
		float radius { get; }

		public Goal(GlobalPos gp, int radius) {
			anchor = gp;
			switch (gp) {
				case GlobalPos.NORTH:
					this.center = new Point(Settings.Default.iRoomWidth / 2, 0);
					break;
				case GlobalPos.WEST:
					this.center = new Point(0, Settings.Default.iRoomHeight / 2);
					break;
				case GlobalPos.SOUTH:
					this.center = new Point(Settings.Default.iRoomWidth / 2, Settings.Default.iRoomHeight);
					break;
				case GlobalPos.EAST:
					this.center = new Point(Settings.Default.iRoomWidth, Settings.Default.iRoomHeight / 2);
					break;
			}
			this.radius = radius;
		}

		public Goal(string text) {
			string[] buffer = text.Split(';');
			this.center = new Point(int.Parse(buffer[0]), int.Parse(buffer[1]));
			this.radius = float.Parse(buffer[2]);
		}

		public override void draw(Graphics gfx, PointF scale) {
			PointF scaled_pos = new PointF(
				this.center.X * scale.X,
				this.center.Y * scale.Y
				);
			PointF scaledRadius = new PointF {
				X = radius * scale.X,
				Y = radius * scale.Y
			};
			gfx.DrawEllipse(Pens.Red, scaled_pos.X - scaledRadius.X, scaled_pos.Y - scaledRadius.Y, scaledRadius.X * 2, scaledRadius.Y * 2);
		}

		public override void move() {
			Random rd = new Random();
		}
		public override bool collide(GameObject[] colliders) {
		}
		}
			return false;
		}

		public override string ToString() {
			return string.Format("{0};{1};{2};{3}", center.X, center.Y, radius, anchor);
		}
	}
}

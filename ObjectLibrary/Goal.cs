using BouncingBall.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ObjectLibrary {
	class Goal : GameObject {

		public delegate void OnBallReachHandler(string id);
		public event OnBallReachHandler onBallReach;

		public enum GlobalPos {
			NORTH,
			WEST,
			SOUTH,
			EAST
		}

		public GlobalPos anchor { get; set; }
		public Point center { get; set; }
		public float radius { get; set; }

		public Goal(int room_lenght, int room_width) {
			anchor = GlobalPos.NORTH;
			this.move(room_lenght, room_width);
		}

		public Goal(string text) {
			string[] buffer = text.Split(';');
			this.center = new Point(int.Parse(buffer[0]), int.Parse(buffer[1]));
			this.radius = float.Parse(buffer[2]);
			this.anchor = (GlobalPos)int.Parse(buffer[3]);
		}

		private Point getPointFromAnchor(GlobalPos gp) {
			Point p = new Point();
			switch (gp) {
				case GlobalPos.NORTH:
					p = new Point(Settings.Default.iRoomWidth / 2, 0);
					break;
				case GlobalPos.WEST:
					p = new Point(0, Settings.Default.iRoomHeight / 2);
					break;
				case GlobalPos.SOUTH:
					p = new Point(Settings.Default.iRoomWidth / 2, Settings.Default.iRoomHeight);
					break;
				case GlobalPos.EAST:
					p = new Point(Settings.Default.iRoomWidth, Settings.Default.iRoomHeight / 2);
					break;
			}
			return p;
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
			gfx.FillEllipse(Brushes.ForestGreen, scaled_pos.X - scaledRadius.X, scaled_pos.Y - scaledRadius.Y, scaledRadius.X * 2, scaledRadius.Y * 2);
			gfx.DrawEllipse(Pens.Green, scaled_pos.X - scaledRadius.X, scaled_pos.Y - scaledRadius.Y, scaledRadius.X * 2, scaledRadius.Y * 2);
		}

		public override void move(int room_lenght, int roomWidth) {
			Random rd = new Random();
			GlobalPos oldAnchor = this.anchor;
			do {
				this.anchor = (GlobalPos)rd.Next(3);
			} while (oldAnchor == this.anchor);
			this.center = getPointFromAnchor(anchor);
			this.radius = rd.Next((int)(
				Math.Min(room_lenght, roomWidth) * Settings.Default.fMinGoalSize),
				(int)(Math.Min(room_lenght, roomWidth) * Settings.Default.fMaxGoalSize)
				);
		}

		public override bool collide(GameObject[] colliders) {
			if (colliders[0] is Ball ball) {
				var dist = (int)(Math.Pow(this.center.X - ball.center.X, 2) + Math.Pow(this.center.Y - ball.center.Y, 2) - Math.Pow(this.radius + ball.radius, 2));
				if (dist < 0) {
					onBallReach?.Invoke(ball.lastToHit);
					ball.lastToHit = null;
					return true;
				}
			}
			return false;
		}

		public override string ToString() {
			return string.Format("{0};{1};{2};{3}", center.X, center.Y, radius, (int)anchor);
		}

	}
}

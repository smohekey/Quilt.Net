namespace Quilt.VG {
	using System.Collections.Generic;
	using System.Numerics;
	using Quilt.Collections;

	internal class Triangle {
		public static float Area(Vector2 p0, Vector2 p1, Vector2 p2) {
			float area = 0;

			area += p0.X * (p2.Y - p1.Y);
			area += p1.X * (p0.Y - p2.Y);
			area += p2.X * (p1.Y - p0.Y);

			return area * 0.5f;
		}

		public static bool IsConvex(Vector2 p0, Vector2 p1, Vector2 p2) {
			if (Area(p0, p1, p2) < 0) {
				return true;
			} else {
				return false;
			}
		}

		internal static bool ContainsPoint(IEnumerable<PathPoint> points, Vector2 p0, Vector2 p1, Vector2 p2) {
			float area1, area2, area3;

			foreach (var point in points) {
				Vector2 p3 = point.Position;

				if ((point.Curvature == Curvature.Concave) &&
					(((p3.X != p0.X) && (p3.Y != p0.Y)) ||
			 		((p3.X != p1.X) && (p3.Y != p1.Y)) ||
			 		((p3.X != p2.X) && (p3.Y != p2.Y)))) {

					area1 = Area(p0, p1, point.Position);
					area2 = Area(p1, p2, point.Position);
					area3 = Area(p2, p0, point.Position);

					if (area1 > 0) {
						if ((area2 > 0) && (area3 > 0)) {
							return true;
						}
					}

					if (area1 < 0) {
						if ((area2 < 0) && (area3 < 0)) {
							return true;
						}
					}
				}
			}

			return false;
		}
	}
}

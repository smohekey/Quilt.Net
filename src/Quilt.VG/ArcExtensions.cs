namespace Quilt.VG {
	using System;
	using System.Numerics;

	public static class ArcExtensions {
		public static IFinishingPathBuilder ArcTo(this IFinishingPathBuilder @this, float x, float y, float radius, bool clockwise) {
			return @this.ArcTo(new Vector2(x, y), radius, clockwise);
		}

		public static IFinishingPathBuilder ArcTo(this IFinishingPathBuilder @this, Vector2 p1, float radius, bool clockwise) {
			var p0 = @this.Position;

			return @this.Arc(p0, p1, radius, clockwise);
		}

		private static IFinishingPathBuilder Arc(this IFinishingPathBuilder @this, Vector2 p0, Vector2 p1, float radius, bool clockwise) {
			var pC = GetArcCenter(p0, p1, radius, clockwise);

			var a0 = MathF.Atan2(p0.Y - pC.Y, p0.X - p0.X);
			var a1 = MathF.Atan2(p1.Y - pC.Y, p1.X - p1.X);

			float deltaAngle = a1 - a0;

			if (clockwise) {
				if (MathF.Abs(deltaAngle) >= MathF.PI * 2) {
					deltaAngle = MathF.PI * 2;
				} else {
					while (deltaAngle < 0.0f) {
						deltaAngle += MathF.PI * 2;
					}
				}
			} else {
				if (MathF.Abs(deltaAngle) >= MathF.PI * 2) {
					deltaAngle = -MathF.PI * 2;
				} else {
					while (deltaAngle > 0.0f) {
						deltaAngle -= MathF.PI * 2;
					}
				}
			}

			var segmentCount = Math.Abs(2 * MathF.PI * radius * (deltaAngle / 360) * Constants.RAD_2_DEG) / 4;

			for (int i = 0; i <= segmentCount; i++) {
				var angle = a0 + deltaAngle * (i / (float)segmentCount);
				var deltaX = MathF.Cos(angle);
				var deltaY = MathF.Sin(angle);

				var x = pC.X + deltaX * radius;
				var y = pC.Y + deltaY * radius;

				@this.Position = new Vector2(x, y);
			}

			return @this;
		}

		private static Vector2 GetArcCenter(Vector2 p0, Vector2 p1, float radius, bool clockwise) {
			var xA = (p1.X - p0.X) / 2;
			var yA = -(p1.Y - p0.Y) / 2;

			var xM = p1.X + xA;
			var yM = p1.Y + yA;

			var a = MathF.Sqrt((xA * xA) + (yA * yA));
			var b = MathF.Sqrt((radius * radius) - (a * a));

			if (clockwise) {
				return new Vector2(
					xM + (b * yA) / a,
					yM - (b * xA) / a
				);
			} else {
				return new Vector2(
					xM - (b * yA) / a,
					yM + (b * xA) / a
				);
			}
		}
	}
}

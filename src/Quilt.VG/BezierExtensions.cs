using System.Runtime.CompilerServices;
namespace Quilt.VG {
	using System;
	using System.Numerics;

	public static class BezierExtensions {
		public static IFinishingPathBuilder BezierTo(this IFinishingPathBuilder @this, float x1, float y1, float x2, float y2, float x3, float y3) {
			return @this.BezierTo(new Vector2(x1, y1), new Vector2(x2, y2), new Vector2(x3, y3));
		}

		public static IFinishingPathBuilder BezierTo(this IFinishingPathBuilder @this, Vector2 p1, Vector2 p2, Vector2 p3) {
			var p0 = @this.Position;

			return @this.Bezier(p0, p1, p2, p3);
		}

		internal static IFinishingPathBuilder Bezier(this IFinishingPathBuilder @this, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3) {
			const int COUNT = 100;

			float dt = 1.0f / COUNT;
			float t = 0.0f;

			for (int i = 0; i <= COUNT; i++) {
				var x = MathF.Pow((1 - t), 3) * p0.X + 3 * t * MathF.Pow((1 - t), 2) * p1.X + 3 * (1 - t) * MathF.Pow(t, 2) * p2.X + MathF.Pow(t, 3) * p3.X;
				var y = MathF.Pow((1 - t), 3) * p0.Y + 3 * t * MathF.Pow((1 - t), 2) * p1.Y + 3 * (1 - t) * MathF.Pow(t, 2) * p2.Y + MathF.Pow(t, 3) * p3.Y;

				@this.AddPoint(new Vector2(x, y));

				t += dt;
			}

			return @this;
		}
	}
}

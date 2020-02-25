using System.IO;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;
using System.Drawing;
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

		/*private static IFinishingPathBuilder Arc(this IFinishingPathBuilder @this, Vector2 p0, Vector2 p3, float r, bool clockwise) {
			var pC = GetArcCenter(p0, p3, r, clockwise);

			var a0 = MathF.Atan2(p0.Y - pC.Y, p0.X - pC.X);
			var a1 = MathF.Atan2(p3.Y - pC.Y, p3.X - pC.X);

			float da = a1 - a0;

			if (clockwise) {
				if (MathF.Abs(da) >= MathF.PI * 2) {
					da = MathF.PI * 2;
				} else {
					while (da < 0.0f) {
						da += MathF.PI * 2;
					}
				}
			} else {
				if (MathF.Abs(da) >= MathF.PI * 2) {
					da = -MathF.PI * 2;
				} else {
					while (da > 0.0f) {
						da -= MathF.PI * 2;
					}
				}
			}

			var ndivs = Math.Max(1, Math.Min((int)(MathF.Abs(da) / (MathF.PI * 0.5f) + 0.5f), 5));
			var hda = da / ndivs / 2.0f;
			var kappa = MathF.Abs(4.0f / 3.0f * (1.0f - MathF.Cos(hda)) / MathF.Sin(hda));

			if (clockwise) {
				kappa = -kappa;
			}

			var px = 0f;
			var py = 0f;
			var ptanx = 0f;
			var ptany = 0f;

			for (var i = 0; i <= ndivs; i++) {
				var a = a0 + da * (i / (float)ndivs);
				var dx = MathF.Cos(a);
				var dy = MathF.Sin(a);
				var x = pC.X + dx * r;
				var y = pC.Y + dy * r;
				var tanx = -dy * r * kappa;
				var tany = dx * r * kappa;

				@this.BezierTo(px + ptanx, py + ptany, x - tanx, y - tany, x, y);

				px = x;
				py = y;
				ptanx = tanx;
				ptany = tany;
			}

			return @this;
		}*/

		private static IFinishingPathBuilder Arc(this IFinishingPathBuilder @this, Vector2 p0, Vector2 p1, float r, bool clockwise) {

			if (!TryGetArcCenter(p0, p1, r, clockwise, out var pC)) {
				return @this;
			}

			var a0 = MathF.Atan2(p0.Y - pC.Y, p0.X - pC.X);
			var a1 = MathF.Atan2(p1.Y - pC.Y, p1.X - pC.X);

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

			// the arc length in pixels / 4
			var segmentCount = (int)(Math.Abs(2 * MathF.PI * r * (deltaAngle / 360) * Constants.RAD_2_DEG) / 4);

			// we start from 1 here because the previous MoveTo will have set the first position
			for (int i = 1; i <= segmentCount; i++) {
				var angle = a0 + deltaAngle * (i / (float)segmentCount);
				var deltaX = MathF.Cos(angle);
				var deltaY = MathF.Sin(angle);

				var x = pC.X + deltaX * r;
				var y = pC.Y + deltaY * r;

				@this.AddPoint(new Vector2(x, y));
			}

			return @this;
		}

		private static bool TryGetArcCenter(Vector2 p0, Vector2 p1, float r, bool clockwise, out Vector2 result) {
			var lim = 4 * r * r;
			var d = MathF.Pow(p1.X - p0.X, 2) + MathF.Pow(p1.Y - p0.Y, 2);

			var x2 = (p0.X + p1.X) / 2;
			var y2 = (p0.Y + p1.Y) / 2;

			if (lim < d) {
				// no solution
				result = default;

				return false;
			} else if (lim == d) {
				result = new Vector2(x2, y2);

				return true;
			}

			var q = MathF.Sqrt(d);

			var xB = MathF.Sqrt(MathF.Pow(r, 2) - MathF.Pow(q / 2, 2)) * (p0.Y - p1.Y) / q;
			var yB = MathF.Sqrt(MathF.Pow(r, 2) - MathF.Pow(q / 2, 2)) * (p1.X - p0.X) / q;

			if (clockwise) {
				result = new Vector2(x2 + xB, y2 + yB);

				return true;
			} else {
				result = new Vector2(x2 - xB, y2 - yB);

				return true;
			}

			/*if (clockwise) {
				return new Vector2(
					x2 - MathF.Sqrt(r * r - MathF.Pow(d / 2, 2)) * (p0.Y - p1.Y) / d,
					y2 - MathF.Sqrt(r * r - MathF.Pow(d / 2, 2)) * (p0.X - p1.X) / d
				);
			} else {
				return new Vector2(
					x2 + MathF.Sqrt(r * r - MathF.Pow(d / 2, 2)) * (p0.Y - p1.Y) / d,
					y2 + MathF.Sqrt(r * r - MathF.Pow(d / 2, 2)) * (p0.X - p1.X) / d
				);
			}*/
		}
	}
}

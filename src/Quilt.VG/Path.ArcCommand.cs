namespace Quilt.VG {
	using System;
	using System.Numerics;
	using System.Text;
	using Quilt.Utilities;

	public partial class Path {
		public class ArcCommand : Command {
			public Vector2 P1 { get; }
			public float Radius { get; }

			public ArcCommand(CommandFlags flags, Vector2 p1, float radius) : base(flags) {
				P1 = p1;
				Radius = radius;
			}

			internal override void Execute(Builder builder, ref Vector2 start, ref Vector2 position, ref Vector2? prevPosition) {
				var isClockwise = Flags.IsSet(CommandFlags.EllipseOrArcIsClockwise);

				var p0 = position;

				if (!TryGetArcCenter(p0, P1, Radius, isClockwise, out var pC)) {
					builder.AddPoint(P1);

					return;
				}

				var a0 = MathF.Atan2(p0.Y - pC.Y, p0.X - pC.X);
				var a1 = MathF.Atan2(P1.Y - pC.Y, P1.X - pC.X);

				float deltaAngle = a1 - a0;

				if (isClockwise) {
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

				// TODO: recursively subdivide

				// the arc length in pixels / 4
				var segmentCount = (int)(Math.Abs(2 * MathF.PI * Radius * (deltaAngle / 360) * Constants.RAD_2_DEG) / 4);

				// we start from 1 here because the previous MoveTo will have set the first position
				for (int i = 1; i <= segmentCount; i++) {
					var angle = a0 + deltaAngle * (i / (float)segmentCount);
					var deltaX = MathF.Cos(angle);
					var deltaY = MathF.Sin(angle);

					var x = pC.X + deltaX * Radius;
					var y = pC.Y + deltaY * Radius;

					builder.AddPoint(x, y);
				}
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
			}

			internal override void ToSVGString(StringBuilder builder) {
				throw new NotImplementedException();
			}
		}
	}
}

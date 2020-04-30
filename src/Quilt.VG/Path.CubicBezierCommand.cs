namespace Quilt.VG {
	using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Numerics;
  using System.Text;
  using Quilt.Utilities;

	public partial class Path {
		public class CubicBezierCommand : Command {
			public Vector2 P1 { get; }
			public Vector2 P2 { get; }
			public Vector2 P3 { get; }

			public CubicBezierCommand(CommandFlags flags, Vector2 p1, Vector2 p2, Vector2 p3) : base(flags) {
				P1 = p1;
				P2 = p2;
				P3 = p3;
			}

			internal override void Execute(Builder builder, ref Vector2 start, ref Vector2 position, ref Vector2? prevPosition) {
				var isDelta = Flags.IsSet(CommandFlags.IsDelta);
				var isSmooth = Flags.IsSet(CommandFlags.BezierIsSmooth);

				var x0 = position.X;
				var y0 = position.Y;

				var x1 = 0f;
				var y1 = 0f;

				if (isSmooth) {
					x1 = prevPosition.HasValue ? prevPosition.Value.X : x0;
					y1 = prevPosition.HasValue ? prevPosition.Value.Y : y0;
				} else {
					x1 = isDelta ? position.X + P1.X : P1.X;
					y1 = isDelta ? position.Y + P1.Y : P1.Y;
				}

				var x2 = isDelta ? position.X + P2.X : P2.X;
				var y2 = isDelta ? position.Y + P2.Y : P2.Y;

				var x3 = isDelta ? position.X + P3.X : P3.X;
				var y3 = isDelta ? position.Y + P3.Y : P3.Y;

				var p0 = new Vector2(x0, y0);
				var p1 = new Vector2(x1, y1);
				var p2 = new Vector2(x2, y2);
				var p3 = new Vector2(x3, y3);

				builder.AddPoint(p0);

				GenerateCurve(builder, p0, p1, p2, p3, p0, p3, 0, 1);

				builder.AddPoint(p3);
			}


			// recursively subdivide the curve
			// begin with the start point p0, the end point p2, calculate the mid point p1
			// if the angle p0p1p2 is between 2 and 178
			// Recurse with p0 == to p0 and p2 == p1
			// Add p2 
			// Recurse with p0 == tp p1 and p2 == p2
			private static void GenerateCurve(Builder builder, Vector2 c0, Vector2 c1, Vector2 c2, Vector2 c3, Vector2 pStart, Vector2 pEnd, float tStart, float tEnd) {
				var tMid = tStart + (tEnd - tStart) / 2;
				var pMid = GeneratePoint(ref c0, ref c1, ref c2, ref c3, tMid);

				var ab = new Vector2(pMid.X - pStart.X, pMid.Y - pStart.Y);
				var cb = new Vector2(pMid.X - pEnd.X, pMid.Y - pEnd.Y);

				var dot = (ab.X * cb.X + ab.Y * cb.Y);
				var cross = (ab.X * cb.Y - ab.Y * cb.X);

				float alpha = MathF.Atan2(cross, dot);

				var angle = MathF.Abs(alpha * 180 / MathF.PI + 0.5f);

				if (angle > 1 && angle < 179) {
					GenerateCurve(builder, c0, c1, c2, c3, pStart, pMid, tStart, tMid);

					builder.AddPoint(pMid);

					GenerateCurve(builder, c0, c1, c2, c3, pMid, pEnd, tMid, tEnd);
				}
			}

			private static Vector2 GeneratePoint(ref Vector2 c0, ref Vector2 c1, ref Vector2 c2, ref Vector2 c3, float t) {
				var x = MathF.Pow((1 - t), 3) * c0.X + 3 * t * MathF.Pow((1 - t), 2) * c1.X + 3 * (1 - t) * MathF.Pow(t, 2) * c2.X + MathF.Pow(t, 3) * c3.X;
				var y = MathF.Pow((1 - t), 3) * c0.Y + 3 * t * MathF.Pow((1 - t), 2) * c1.Y + 3 * (1 - t) * MathF.Pow(t, 2) * c2.Y + MathF.Pow(t, 3) * c3.Y;

				return new Vector2(x, y);
			}

			internal override void ToSVGString(StringBuilder builder) {
				var isSmooth = Flags.IsSet(CommandFlags.BezierIsSmooth);
				var isDelta = Flags.IsSet(CommandFlags.IsDelta);

				if(isSmooth) {
					if(isDelta) {
						builder.Append('s');
					} else {
						builder.Append('S');
					}

					builder.Append(' ');
				} else {
					if(isDelta) {
						builder.Append('c');
					} else {
						builder.Append('C');
					}

					builder.Append(' ');
					builder.Append(P1.X);
					builder.Append(',');
					builder.Append(P1.Y);
					builder.Append(' ');
				}

				builder.Append(P2.X);
				builder.Append(',');
				builder.Append(P2.Y);
				builder.Append(' ');
				builder.Append(P3.X);
				builder.Append(',');
				builder.Append(P3.Y);
				builder.Append(' ');
			}
		}
	}
}

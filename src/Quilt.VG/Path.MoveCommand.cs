namespace Quilt.VG {
	using System.Numerics;
	using System.Text;
	using Quilt.Utilities;

	public partial class Path {
		public class MoveCommand : Command {
			public Vector2 V { get; }

			public MoveCommand(CommandFlags flags, Vector2 v) : base(flags) {
				V = v;
			}

			internal override void Execute(Builder builder, ref Vector2 start, ref Vector2 position, ref Vector2? prevPosition) {
				if (builder._contourLength != 0) {
					if (builder._contourLength == 1 && builder._points[builder._contourOffset].Position == start) {
						builder._contourLength = 0;
						builder._points.RemoveAt(builder._points.Count - 1);
					} else {
						builder.EndContour(false);
					}
				}

				var isDelta = Flags.IsSet(CommandFlags.IsDelta);

				var x = isDelta ? position.X + V.X : V.X;
				var y = isDelta ? position.Y + V.Y : V.Y;

				builder.AddPoint(x, y);

				start = position;
			}

			internal override void ToSVGString(StringBuilder builder) {
				if(Flags.IsSet(CommandFlags.IsDelta)) {
					builder.Append('m');
				} else {
					builder.Append('M');
				}

				builder.Append(' ');
				builder.Append(V.X);
				builder.Append(',');
				builder.Append(V.Y);
				builder.Append(' ');
			}
		}
	}
}

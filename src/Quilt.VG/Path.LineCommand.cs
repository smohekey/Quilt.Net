namespace Quilt.VG {
	using System.Numerics;
	using System.Text;
	using Quilt.Utilities;

	public partial class Path {
		public class LineCommand : Command {
			public Vector2 V { get; }

			public LineCommand(CommandFlags flags, Vector2 v) : base(flags) {
				V = v;
			}

			internal override void Execute(Builder builder, ref Vector2 start, ref Vector2 position, ref Vector2? prevPosition) {
				var isDelta = Flags.IsSet(CommandFlags.IsDelta);

				var x = isDelta ? position.X + V.X : V.X;
				var y = isDelta ? position.Y + V.Y : V.Y;

				builder.AddPoint(x, y);
			}

			internal override void ToSVGString(StringBuilder builder) {
				if(Flags.IsSet(CommandFlags.IsDelta)) {
					builder.Append('l');
				} else {
					builder.Append('L');
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

namespace Quilt.VG {
	using System.Numerics;
	using System.Text;
	using Quilt.Utilities;

	public partial class Path {
		public class VerticalLineCommand : Command {
			public float Y { get; }

			public VerticalLineCommand(CommandFlags flags, float y) : base(flags) {
				Y = y;
			}

			internal override void Execute(Builder builder, ref Vector2 start, ref Vector2 position, ref Vector2? prevPosition) {
				var isDelta = Flags.IsSet(CommandFlags.IsDelta);

				var x = position.X;
				var y = isDelta ? position.Y + Y : Y;

				builder.AddPoint(x, y);
			}

			internal override void ToSVGString(StringBuilder builder) {
				if(Flags.IsSet(CommandFlags.IsDelta)) {
					builder.Append('v');
				} else {
					builder.Append('V');
				}

				builder.Append(' ');
				builder.Append(Y);
				builder.Append(' ');
			}
		}
	}
}

namespace Quilt.VG {
	using System.Numerics;
	using System.Text;
	using Quilt.Utilities;

	public partial class Path {
		public class HorizontalLineCommand : Command {
			public float X { get; }

			public HorizontalLineCommand(CommandFlags flags, float x) : base(flags) {
				X = x;
			}

			internal override void Execute(Builder builder, ref Vector2 start, ref Vector2 position, ref Vector2? prevPosition) {
				var isDelta = Flags.IsSet(CommandFlags.IsDelta);

				var x = isDelta ? position.X + X : X;
				var y = position.Y;

				builder.AddPoint(x, y);
			}

			internal override void ToSVGString(StringBuilder builder) {
				if(Flags.IsSet(CommandFlags.IsDelta)) {
					builder.Append('h');
				} else {
					builder.Append('H');
				}

				builder.Append(' ');
				builder.Append(X);
				builder.Append(' ');
			}
		}
	}
}

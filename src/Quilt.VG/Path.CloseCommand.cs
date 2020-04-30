namespace Quilt.VG {
	using System.Numerics;
	using System.Text;

	public partial class Path {
		public class CloseCommand : Command {
			public CloseCommand() : base(CommandFlags.None) {

			}

			internal override void Execute(Builder builder, ref Vector2 start, ref Vector2 position, ref Vector2? prevPosition) {
				if (start != position) {
					builder.AddPoint(start);
				}

				builder.EndContour(true);
			}

			internal override void ToSVGString(StringBuilder builder) {
				builder.Append('Z');
				builder.Append(' ');
			}
		}
	}
}

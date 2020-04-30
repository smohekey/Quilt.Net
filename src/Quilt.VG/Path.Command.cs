namespace Quilt.VG {
	using System;
	using System.Numerics;
	using System.Text;
	using Quilt.Collections;

	public partial class Path {

		[Flags]
		public enum CommandFlags : byte {
			None = 0x00,
			IsDelta = 0x01,
			BezierIsSmooth = 0x02,
			EllipseIsLarge = 0x04,
			EllipseOrArcIsClockwise = 0x08
		}

		public abstract class Command {
			public CommandFlags Flags { get; }

			protected Command(CommandFlags flags) {
				Flags = flags;
			}

			internal abstract void Execute(Builder builder, ref Vector2 start, ref Vector2 position, ref Vector2? prevPosition);

			internal abstract void ToSVGString(StringBuilder builder);
		}

		public class EllipseCommand : Command {
			public Vector2 P1 { get; }
			public Vector2 Radius { get; }
			public float Angle { get; }

			public EllipseCommand(CommandFlags flags, Vector2 p1, Vector2 radius, float angle) : base(flags) {
				P1 = p1;
				Radius = radius;
				Angle = angle;
			}

			internal override void Execute(Builder builder, ref Vector2 start, ref Vector2 position, ref Vector2? prevPosition) {
				throw new NotImplementedException();
			}

			internal override void ToSVGString(StringBuilder builder) {
				throw new NotImplementedException();
			}
		}
	}
}

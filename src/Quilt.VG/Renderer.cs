namespace Quilt.VG {
	using System;
	using System.Numerics;
	using Quilt.GL;

	abstract class Renderer {
		protected readonly GLContext _gl;

		protected Renderer(GLContext gl) {
			_gl = gl;
		}

		public abstract void Render(FrameBuilder frame, Matrix4x4 projection, Vector2 viewport, Path path);

		protected Vector2 ExtrapolatePoint(Vector2 p0, Vector2 p1, float d) {
			float length = MathF.Sqrt(MathF.Pow(p1.X - p0.X, 2) + MathF.Pow(p1.Y - p0.Y, 2));

			float slopeX = (p1.X - p0.X) / length;
			float slopeY = (p1.Y - p0.Y) / length;

			return new Vector2(
				p1.X + slopeX * d,
				p1.Y + slopeY * d
			);
		}
	}
}

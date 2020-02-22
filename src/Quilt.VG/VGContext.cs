namespace Quilt.VG {
	using System.Numerics;
	using Quilt.GL;

	public class VGContext {
		internal readonly StrokeRenderer _strokeRenderer;
		internal readonly FillRenderer _fillRenderer;

		public VGContext(GLContext gl) {
			_strokeRenderer = new StrokeRenderer(gl);
			_fillRenderer = new FillRenderer(gl);
		}

		public IFrameBuilder BeginFrame(int width, int height) {
			return new FrameBuilder(
				this,
				Matrix4x4.CreateOrthographicOffCenter(0, width, height, 0, -1, 1),
				new Vector2(width, height)
			);
		}
	}
}

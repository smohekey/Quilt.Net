namespace Quilt.VG {
	using System.Numerics;

	public class FrameBuilder : IFrameBuilder {
		private readonly VGContext _vg;
		private readonly Matrix4x4 _projection;
		private readonly Vector2 _viewport;

		public FrameBuilder(VGContext vg, Matrix4x4 projection, Vector2 viewport) {
			_vg = vg;
			_projection = projection;
			_viewport = viewport;
		}

		public Color StrokeColor { get; set; }
		public float StrokeWidth { get; set; }
		public float StrokeMiter { get; set; }
		public StrokeFlags StrokeFlags { get; set; }
		public Color FillColor { get; set; }

		public IFrameBuilder SetStrokeColor(Color strokeColor) {
			StrokeColor = strokeColor;

			return this;
		}

		public IFrameBuilder SetStrokeWidth(float strokeWidth) {
			StrokeWidth = strokeWidth;

			return this;
		}

		public IFrameBuilder SetStrokeMiter(float strokeMiter) {
			StrokeMiter = strokeMiter;

			return this;
		}

		public IFrameBuilder SetStrokeFlags(StrokeFlags strokeFlags) {
			StrokeFlags = strokeFlags;

			return this;
		}

		public IFrameBuilder SetFillColor(Color fillColor) {
			FillColor = fillColor;

			return this;
		}

		public IPathBuilder CreatePath() {
			return new Path.Builder(this);
		}

		public IFrameBuilder FillPath(IPath path) {
			_vg._fillRenderer.Render(this, _projection, _viewport, path);

			return this;
		}

		public IFrameBuilder StrokePath(IPath path) {
			_vg._strokeRenderer.Render(this, _projection, _viewport, path);

			return this;
		}
	}
}

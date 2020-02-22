namespace Quilt.VG {
	using System.Numerics;

	public interface IFrameBuilder {
		Vector4 StrokeColor { get; set; }
		float StrokeWidth { get; set; }
		float StrokeMiter { get; set; }
		StrokeFlags StrokeFlags { get; set; }
		Vector4 FillColor { get; set; }

		IFrameBuilder SetStrokeColor(Vector4 strokeColor);

		IFrameBuilder SetStrokeWidth(float strokeWidth);

		IFrameBuilder SetStrokeMiter(float strokeMiter);

		IFrameBuilder SetStrokeFlags(StrokeFlags strokeFlags);

		IFrameBuilder SetFillColor(Vector4 fillColor);

		IPathBuilder CreatePath(Vector2 start);
		IFrameBuilder StrokePath(IPathBuilder path);
		IFrameBuilder FillPath(IPathBuilder path);

		public IPathBuilder CreatePath(float x, float y) {
			return CreatePath(new Vector2(x, y));
		}
	}
}

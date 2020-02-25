namespace Quilt.VG {
	public interface IFrameBuilder {
		Color StrokeColor { get; set; }
		float StrokeWidth { get; set; }
		float StrokeMiter { get; set; }
		StrokeFlags StrokeFlags { get; set; }
		Color FillColor { get; set; }

		IFrameBuilder SetStrokeColor(Color strokeColor);

		IFrameBuilder SetStrokeWidth(float strokeWidth);

		IFrameBuilder SetStrokeMiter(float strokeMiter);

		IFrameBuilder SetStrokeFlags(StrokeFlags strokeFlags);

		IFrameBuilder SetFillColor(Color fillColor);

		IPathBuilder CreatePath();
		IFrameBuilder StrokePath(IPath path);
		IFrameBuilder FillPath(IPath path);
	}
}

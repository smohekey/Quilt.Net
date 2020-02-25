namespace Quilt.VG {
	using System.Numerics;

	public interface IBasePathBuilder<T> where T : IBasePathBuilder<T> {
		Vector2 Position { get; }

		Color FillColor { get; set; }
		StrokeFlags StrokeFlags { get; set; }
		Color StrokeColor { get; set; }
		float StrokeWidth { get; set; }

		T SetFillColor(Color fillColor);
		T AddPoint(Vector2 position);
		T SetStrokeFlags(StrokeFlags strokeFlags);
		T SetStrokeColor(Color strokeColor);
		T SetStrokeWidth(float strokeWidth);
	}
}

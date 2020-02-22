namespace Quilt.VG {
	using System.Numerics;

	public interface IBasePathBuilder<T> where T : IBasePathBuilder<T> {
		Path Path { get; }

		Vector4 FillColor { get; set; }
		Vector2 Position { get; set; }
		StrokeFlags StrokeFlags { get; set; }
		Vector4 StrokeColor { get; set; }
		float StrokeWidth { get; set; }

		T SetFillColor(Vector4 fillColor);
		T SetPosition(Vector2 position);
		T SetStrokeFlags(StrokeFlags strokeFlags);
		T SetStrokeColor(Vector4 strokeColor);
		T SetStrokeWidth(float strokeWidth);
	}
}

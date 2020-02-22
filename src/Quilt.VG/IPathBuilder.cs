using System.Numerics;

namespace Quilt.VG {
	public interface IPathBuilder {
		CommandList Commands { get; }

		Vector4 FillColor { get; set; }
		Vector2 Position { get; set; }
		StrokeFlags StrokeFlags { get; set; }
		Vector4 StrokeColor { get; set; }
		float StrokeMiter { get; set; }
		float StrokeWidth { get; set; }

		IPathBuilder SetFillColor(Vector4 fillColor);
		IPathBuilder SetPosition(Vector2 position);
		IPathBuilder SetStrokeFlags(StrokeFlags strokeFlags);
		IPathBuilder SetStrokeColor(Vector4 strokeColor);
		IPathBuilder SetStrokeMiter(float strokeMiter);
		IPathBuilder SetStrokeWidth(float strokeWidth);

		IPathBuilder Fill();
		IPathBuilder Stroke();
	}
}

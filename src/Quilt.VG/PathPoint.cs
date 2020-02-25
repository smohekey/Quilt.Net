namespace Quilt.VG {
	using System.Numerics;

	public struct PathPoint {
		public readonly Vector2 Position;

		public Curvature Curvature;
		public readonly Color StrokeColor;
		public readonly float StrokeWidth;
		public readonly StrokeFlags StrokeFlags;
		public readonly Color FillColor;

		public PathPoint(Vector2 position, Color strokeColor, float strokeWidth, StrokeFlags strokeFlags, Color fillColor) {
			Position = position;
			Curvature = Curvature.Convex;
			StrokeColor = strokeColor;
			StrokeWidth = strokeWidth;
			StrokeFlags = strokeFlags;
			FillColor = fillColor;
		}
	}
}

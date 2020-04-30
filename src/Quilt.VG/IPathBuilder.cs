namespace Quilt.VG {
	using System.Numerics;

	public interface IPathBuilder {
		IPathBuilder ArcBy(Vector2 p1, float radius, bool isClockwise);
		IPathBuilder ArcTo(Vector2 p1, float radius, bool isClockwise);
		IPathBuilder BezierBy(Vector2 d1, Vector2 d2);
		IPathBuilder BezierBy(Vector2 d1, Vector2 d2, Vector2 d3);
		IPathBuilder BezierTo(Vector2 p1, Vector2 p2);
		IPathBuilder BezierTo(Vector2 p1, Vector2 p2, Vector2 p3);		
		IPathBuilder EllipseBy(Vector2 d1, Vector2 radius, float angle, bool isLarge, bool isClockwise);
		IPathBuilder EllipseTo(Vector2 p1, Vector2 radius, float angle, bool isLarge, bool isClockwise);
		IPathBuilder HorizontalLineBy(float deltaX);
		IPathBuilder HorizontalLineTo(float x);
		IPathBuilder LineBy(Vector2 delta);
		IPathBuilder LineTo(Vector2 position);
		IPathBuilder MoveBy(Vector2 delta);
		IPathBuilder MoveTo(Vector2 position);
		IPathBuilder SmoothBezierBy(Vector2 d2);
		IPathBuilder SmoothBezierBy(Vector2 d2, Vector2 d3);
		IPathBuilder SmoothBezierTo(Vector2 p2);
		IPathBuilder SmoothBezierTo(Vector2 p2, Vector2 p3);
		IPathBuilder VerticalLineBy(float deltaY);
		IPathBuilder VerticalLineTo(float y);
		IPathBuilder Close();

		Path Build();

		IPathBuilder MoveTo(float x, float y) {
			return MoveTo(new Vector2(x, y));
		}

		IPathBuilder MoveBy(float dx, float dy) {
			return MoveBy(new Vector2(dx, dy));
		}

		IPathBuilder LineTo(float x, float y) {
			return LineTo(new Vector2(x, y));
		}

		IPathBuilder LineBy(float dx, float dy) {
			return LineBy(new Vector2(dx, dy));
		}

		IPathBuilder ArcTo(float x1, float y1, float radius, bool isClockwise) {
			return ArcTo(new Vector2(x1, y1), radius, isClockwise);
		}

		IPathBuilder ArcBy(float dx, float dy, float radius, bool isClockwise) {
			return ArcBy(new Vector2(dx, dy), radius, isClockwise);
		}

		IPathBuilder EllipseTo(float x1, float y1, float radiusX, float radiusY, float angle, bool isLarge, bool isClockwise) {
			return EllipseTo(new Vector2(x1, y1), new Vector2(radiusX, radiusY), angle, isLarge, isClockwise);
		}

		IPathBuilder EllipseBy(float dx, float dy, float radiusX, float radiusY, float angle, bool isLarge, bool isClockwise) {
			return EllipseBy(new Vector2(dx, dy), new Vector2(radiusX, radiusY), angle, isLarge, isClockwise);
		}

		IPathBuilder BezierTo(float x1, float y1, float x2, float y2, float x3, float y3) {
			return BezierTo(new Vector2(x1, y1), new Vector2(x2, y2), new Vector2(x3, y3));
		}

		IPathBuilder BezierBy(float dx1, float dy1, float dx2, float dy2, float dx3, float dy3) {
			return BezierBy(new Vector2(dx1, dy1), new Vector2(dx2, dy2), new Vector2(dx3, dy3));
		}

		IPathBuilder SmoothBezierTo(float x2, float y2, float x3, float y3) {
			return SmoothBezierTo(new Vector2(x2, y2), new Vector2(x3, y3));
		}

		IPathBuilder SmoothBezierBy(float dx2, float dy2, float dx3, float dy3) {
			return SmoothBezierTo(new Vector2(dx2, dy2), new Vector2(dx3, dy3));
		}

		IPathBuilder BezierTo(float x1, float y1, float x2, float y2) {
			return BezierTo(new Vector2(x1, y1), new Vector2(x2, y2));
		}

		IPathBuilder BezierBy(float dx1, float dy1, float dx2, float dy2) {
			return BezierBy(new Vector2(dx1, dy1), new Vector2(dx2, dy2));
		}

		IPathBuilder SmoothBezierTo(float x2, float y2) {
			return SmoothBezierTo(new Vector2(x2, y2));
		}

		IPathBuilder SmoothBezierBy(float dx2, float dy2) {
			return SmoothBezierBy(new Vector2(dx2, dy2));
		}
	}
}

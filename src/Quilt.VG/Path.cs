namespace Quilt.VG {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Numerics;
	using Quilt.Collections;

	public class Path : IPath {
		private readonly FrameBuilder _frameBuilder;
		private readonly PooledArray<PathPoint> _points = new PooledArray<PathPoint>();
		private readonly Lazy<WindingOrder> _windingOrder;

		public WindingOrder WindingOrder => _windingOrder.Value;

		private Path(FrameBuilder frameBuilder, PooledArray<PathPoint> points) {
			_frameBuilder = frameBuilder;
			_points = points;
			_windingOrder = new Lazy<WindingOrder>(DetermineWindingOrder, true);

			ClassifyPoints();
		}

		private void ClassifyPoints() {
			var concaveCount = 0;
			var pointCount = _points.Length;

			if (WindingOrder == WindingOrder.CounterClockwise) {
				for (int i = 0, j = pointCount - 1; i != j; i++, j--) {
					var temp = _points[j];
					_points[j] = _points[i];
					_points[j] = temp;
				}
			}

			for (int i = 0; i < pointCount - 1; i++) {
				if (i == 0) {
					if (Triangle.IsConvex(_points[pointCount - 2].Position, _points[i].Position, _points[i + 1].Position)) {
						_points[i].Curvature = Curvature.Convex;
					} else {
						_points[i].Curvature = Curvature.Concave;

						concaveCount++;
					}
				} else {
					if (Triangle.IsConvex(_points[i - 1].Position, _points[i].Position, _points[i + 1].Position)) {
						_points[i].Curvature = Curvature.Convex;
					} else {
						_points[i].Curvature = Curvature.Concave;

						concaveCount++;
					}
				}
			}
		}

		public IPath Fill() {
			_frameBuilder.FillPath(this);

			return this;
		}

		public IPath Stroke() {
			_frameBuilder.StrokePath(this);

			return this;
		}

		public IFrameBuilder Finish() {
			return _frameBuilder;
		}

		public IEnumerator<PathPoint> GetEnumerator() {
			return _points.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public IEnumerator<PathPoint> GetReverseEnumerator() {
			return _points.GetReverseEnumerator();
		}

		private WindingOrder DetermineWindingOrder() {
			int pointCount = _points.Length;

			// If vertices duplicates first as last to represent closed polygon,
			// skip last.
			PathPoint first = _points[0];
			PathPoint last = _points[^1];

			if (last.Position.Equals(first.Position)) {
				pointCount -= 1;
			}

			var minVertex = FindCornerVertex(_points);

			// Orientation matrix:
			//     [ 1  xa  ya ]
			// O = | 1  xb  yb |
			//     [ 1  xc  yc ]
			Vector2 a = _points[WrapAt(minVertex - 1, pointCount)].Position;
			Vector2 b = _points[minVertex].Position;
			Vector2 c = _points[WrapAt(minVertex + 1, pointCount)].Position;

			// determinant(O) = (xb*yc + xa*yb + ya*xc) - (ya*xb + yb*xc + xa*yc)
			var detOrient = b.X * c.Y + a.X * b.Y + a.Y * c.X - (a.Y * b.X + b.Y * c.X + a.X * c.Y);

			// TBD: check for "==0", in which case is not defined?
			// Can that happen?  Do we need to check other vertices / eliminate duplicate vertices?
			WindingOrder result = detOrient > 0
							? WindingOrder.Clockwise
							: WindingOrder.CounterClockwise;
			return result;
		}

		// Find vertex along one edge of bounding box.
		// In this case, we find smallest y; in case of tie also smallest x.
		private static int FindCornerVertex(PooledArray<PathPoint> points) {
			var minVertex = -1;
			var minY = float.MaxValue;
			var minXAtMinY = float.MaxValue;

			for (var i = 0; i < points.Length; i++) {
				Vector2 vert = points[i].Position;

				var y = vert.Y;

				if (y > minY) {
					continue;
				}

				if (y == minY) {
					if (vert.X >= minXAtMinY) {
						continue;
					}
				}

				// Minimum so far.
				minVertex = i;
				minY = y;
				minXAtMinY = vert.X;
			}

			return minVertex;
		}

		// Return value in (0..n-1).
		// Works for i in (-n..+infinity).
		// If need to allow more negative values, need more complex formula.
		private static int WrapAt(int i, int n) {
			// "+n": Moves (-n..) up to (0..).
			return (i + n) % n;
		}

		public class Builder : IPathBuilder, IFinishingPathBuilder {
			private readonly FrameBuilder _frameBuilder;
			private readonly PooledArray<PathPoint> _points = new PooledArray<PathPoint>();

			public Vector2 Position { get; private set; }
			public Color StrokeColor { get; set; }
			public float StrokeWidth { get; set; }
			public StrokeFlags StrokeFlags { get; set; }
			public Color FillColor { get; set; }

			public Builder(FrameBuilder frameBuilder) {
				_frameBuilder = frameBuilder;
			}

			public IPathBuilder AddPoint(Vector2 position) {
				Position = position;

				_points[_points.Length++] = new PathPoint(
					Position = position,
					StrokeColor = StrokeColor,
					StrokeWidth = StrokeWidth,
					StrokeFlags = StrokeFlags,
					FillColor = FillColor
				);

				return this;
			}

			IFinishingPathBuilder IBasePathBuilder<IFinishingPathBuilder>.AddPoint(Vector2 position) {
				AddPoint(position);

				return this;
			}

			public IPathBuilder SetStrokeColor(Color strokeColor) {
				StrokeColor = strokeColor;

				return this;
			}

			IFinishingPathBuilder IBasePathBuilder<IFinishingPathBuilder>.SetStrokeColor(Color strokeColor) {
				SetStrokeColor(strokeColor);

				return this;
			}

			public IPathBuilder SetStrokeWidth(float strokeWidth) {
				StrokeWidth = strokeWidth;

				return this;
			}

			IFinishingPathBuilder IBasePathBuilder<IFinishingPathBuilder>.SetStrokeWidth(float strokeWidth) {
				SetStrokeWidth(strokeWidth);

				return this;
			}

			public IPathBuilder SetStrokeFlags(StrokeFlags strokeFlags) {
				StrokeFlags = strokeFlags;

				return this;
			}

			IFinishingPathBuilder IBasePathBuilder<IFinishingPathBuilder>.SetStrokeFlags(StrokeFlags strokeFlags) {
				SetStrokeFlags(strokeFlags);

				return this;
			}

			public IPathBuilder SetFillColor(Color fillColor) {
				FillColor = fillColor;

				return this;
			}

			IFinishingPathBuilder IBasePathBuilder<IFinishingPathBuilder>.SetFillColor(Color fillColor) {
				SetFillColor(fillColor);

				return this;
			}

			public IFinishingPathBuilder MoveTo(Vector2 position) {
				AddPoint(position);

				return this;
			}

			public IPath Build() {
				return new Path(_frameBuilder, _points);
			}
		}
	}
}

using System.IO;
using System.Numerics;
using System.Diagnostics;
using System.Runtime.Intrinsics;
using System.Buffers;
namespace Quilt.VG {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Numerics;
	using System.Runtime.InteropServices;
	using Quilt.GL;

	public class VGContext {
		private const float DEG_2_RAD = MathF.PI / 180;
		private const float RAD_2_DEG = 180 / MathF.PI;
		private const int BUFFER_SIZE = 1024;

		private readonly ArrayPool<Point> _pointListPool = ArrayPool<Point>.Create();
		private readonly List<PathSegment> _pathSegments = new List<PathSegment>();

		private readonly Stack<State> _stateStack = new Stack<State>();

		private readonly GLContext _gl;
		private readonly GLVertexArray _arcVA;
		private readonly GLBuffer _arcVB;
		private readonly GLProgram _strokeProgram;

		private readonly int _projectionUniform;
		private readonly int _viewportUniform;
		private readonly int _strokeWidthUniform;
		private readonly int _mitreLimitUniform;
		private readonly int _alphaUniform;

		private Matrix4x4 _projection;
		private Vector2 _viewport;

		private State _state;
		private PathSegment _currentPathSegment;
		private int _pathSegmentCount = 0;

		public VGContext(GLContext gl) {
			_state = new State();
			_gl = gl;

			_arcVA = _gl.CreateVertexArray();
			_arcVB = _gl.CreateBuffer();

			gl.BindVertexArray(_arcVA);
			gl.BindBuffer(BufferTarget.Array, _arcVB);
			gl.BufferData<Point>(BufferTarget.Array, BUFFER_SIZE, BufferUsage.StreamDraw);

			gl.EnableVertexAttribArray(0);
			gl.EnableVertexAttribArray(1);
			gl.EnableVertexAttribArray(2);

			gl.VertexAttribPointer(0, 2, DataType.Float, false, Marshal.SizeOf<float>() * 7, 0);
			gl.VertexAttribPointer(1, 4, DataType.Float, false, Marshal.SizeOf<float>() * 7, Marshal.SizeOf<float>() * 2);
			gl.VertexAttribPointer(2, 1, DataType.Float, false, Marshal.SizeOf<float>() * 7, Marshal.SizeOf<float>() * 6);

			var assembly = typeof(VGContext).Assembly;

			var strokeVertexSource = assembly.GetManifestResourceStream($"Quilt.VG.Shaders.Stroke.vert")!;
			var strokeVertexShader = _gl.CreateVertexShader(strokeVertexSource);

			var strokeGeometrySource = assembly.GetManifestResourceStream($"Quilt.VG.Shaders.Stroke.geom")!;
			var strokeGeometryShader = _gl.CreateGeometryShader(strokeGeometrySource);

			var strokeFragmentSource = assembly.GetManifestResourceStream("Quilt.VG.Shaders.Stroke.frag")!;
			var strokeFragmentShader = _gl.CreateFragmentShader(strokeFragmentSource);

			_strokeProgram = _gl.CreateProgram(strokeVertexShader, strokeGeometryShader, strokeFragmentShader);

			_projectionUniform = _gl.GetUniformLocation(_strokeProgram, "_projection");
			_viewportUniform = _gl.GetUniformLocation(_strokeProgram, "_viewport");
			_strokeWidthUniform = _gl.GetUniformLocation(_strokeProgram, "_strokeWidth");
			_mitreLimitUniform = _gl.GetUniformLocation(_strokeProgram, "_mitreLimit");
			_alphaUniform = _gl.GetUniformLocation(_strokeProgram, "_alpha");
		}

		public float StrokeWidth {
			get {
				return _state.StrokeWidth;
			}
			set {
				_state.StrokeWidth = value;
			}
		}

		public Vector4 StrokeColor {
			get {
				return _state.StrokeColor;
			}
			set {
				_state.StrokeColor = value;
			}
		}

		public float Alpha {
			get {
				return _state.Alpha;
			}
			set {
				_state.Alpha = value;
			}
		}

		public void PushState() {
			_stateStack.Push(_state);
			_state = new State(_state);
		}

		public void PopState() {
			_state = _stateStack.Pop();
		}
		public void BeginFrame(int width, int height) {
			_projection = Matrix4x4.CreateOrthographicOffCenter(0, width, height, 0, -1, 1);
			_viewport = new Vector2(width, height);

			ResetPath();
		}

		private void ResetPath() {
			foreach (var pathSegment in _pathSegments) {
				_pointListPool.Return(pathSegment.PointList);
			}

			if (_pathSegmentCount > 0) {
				_pointListPool.Return(_currentPathSegment.PointList);
			}

			_currentPathSegment.PointList = Array.Empty<Point>();
			_currentPathSegment.PointCount = 0;

			_pathSegmentCount = 0;
			_pathSegments.Clear();
		}

		public void BeginPath(float x, float y) {
			ResetPath();

			EmitPathPoint(x, y);
		}

		public void LineTo(float x, float y) {
			EmitPathPoint(x, y);
		}

		public void ArcTo(float x, float y, float radius, bool clockwise) {
			var (x0, y0, _, _) = LastPathPoint;

			Arc(x0, y0, x, y, radius, clockwise);
		}

		public void Arc(float x0, float y0, float x1, float y1, float r, bool clockwise) {
			var (xC, yC) = ArcCenter(x0, y0, x1, y1, r, clockwise);

			var a0 = MathF.Atan2(y0 - yC, x0 - xC);
			var a1 = MathF.Atan2(y1 - yC, x1 - xC);

			EmitArc(xC, yC, r, a0, a1, clockwise);
		}

		private (float X, float Y) ArcCenter(float x0, float y0, float x1, float y1, float r, bool clockwise) {
			var xA = (x1 - x0) / 2;
			var yA = -(y1 - y0) / 2;

			var xM = x1 + xA;
			var yM = y1 + yA;

			var a = MathF.Sqrt((xA * xA) + (yA * yA));
			var b = MathF.Sqrt((r * r) - (a * a));

			if (clockwise) {
				return (
					xM + (b * yA) / a,
					yM - (b * xA) / a
				);
			} else {
				return (
					xM - (b * yA) / a,
					yM + (b * xA) / a
				);
			}
		}

		private void EmitArc(float xC, float yC, float r, float a0, float a1, bool clockwise) {
			float deltaAngle = a1 - a0;

			if (clockwise) {
				if (MathF.Abs(deltaAngle) >= MathF.PI * 2) {
					deltaAngle = MathF.PI * 2;
				} else {
					while (deltaAngle < 0.0f) {
						deltaAngle += MathF.PI * 2;
					}
				}
			} else {
				if (MathF.Abs(deltaAngle) >= MathF.PI * 2) {
					deltaAngle = -MathF.PI * 2;
				} else {
					while (deltaAngle > 0.0f) {
						deltaAngle -= MathF.PI * 2;
					}
				}
			}

			var segmentCount = Math.Abs(2 * MathF.PI * r * (deltaAngle / 360) * RAD_2_DEG) / 2;

			for (int i = 0; i <= segmentCount; i++) {
				var angle = a0 + deltaAngle * (i / (float)segmentCount);
				var deltaX = MathF.Cos(angle);
				var deltaY = MathF.Sin(angle);

				var x = xC + deltaX * r;
				var y = yC + deltaY * r;

				EmitPathPoint(x, y);
			}
		}

		public void BezierTo(float x1, float y1, float x2, float y2, float x3, float y3) {
			var (x0, y0, _, _) = LastPathPoint;

			EmitBezier(x0, y0, x1, y1, x2, y2, x3, y3);
		}

		private void EmitBezier(float x0, float y0, float x1, float y1, float x2, float y2, float x3, float y3) {
			const int COUNT = 100;

			float dt = 1.0f / COUNT;
			float t = 0.0f;
			for (int i = 0; i <= COUNT; i++) {
				var x = MathF.Pow((1 - t), 3) * x0 + 3 * t * MathF.Pow((1 - t), 2) * x1 + 3 * (1 - t) * MathF.Pow(t, 2) * x2 + MathF.Pow(t, 3) * x3;
				var y = MathF.Pow((1 - t), 3) * y0 + 3 * t * MathF.Pow((1 - t), 2) * y1 + 3 * (1 - t) * MathF.Pow(t, 2) * y2 + MathF.Pow(t, 3) * y3;

				EmitPathPoint(x, y);

				t += dt;
			}
		}

		public void Stroke() {
			_gl.UseProgram(_strokeProgram);

			_gl.UniformMatrix(_projectionUniform, 1, false, _projection);
			_gl.Uniform(_viewportUniform, _viewport.X, _viewport.Y);
			_gl.Uniform(_strokeWidthUniform, _state.StrokeWidth);
			_gl.Uniform(_mitreLimitUniform, _state.MitreLimit);
			_gl.Uniform(_alphaUniform, _state.Alpha);

			_gl.BindVertexArray(_arcVA);
			_gl.BindBuffer(BufferTarget.Array, _arcVB);

			foreach (var segment in _pathSegments.Append(_currentPathSegment)) {
				var pointCount = segment.PointCount;

				if (!segment.IsFinished) {
					// duplicate the last point
					//segment.PointList[segment.PointCount] = segment.PointList[segment.PointCount - 1];

					var (x0, y0, _, _) = segment.PointList[segment.PointCount - 2];
					var (x1, y1, color, width) = segment.PointList[segment.PointCount - 1];

					var (x2, y2) = ExtrapolatePoint(x0, y0, x0, y1, 1);

					segment.PointList[segment.PointCount] = new Point(x2, y2, color, width);

					pointCount++;
				}

				Console.WriteLine($"Drawing path segment with {pointCount} points...");
#if DEBUG
				for (int i = 0; i < pointCount; i++) {
					var point = segment.PointList[i];
					var adjacency = (i == 0 || i == pointCount - 1) ? "*" : "";

					Console.WriteLine($"{point.X}, {point.Y} {adjacency}");
				}
#endif

				_gl.BufferSubData(BufferTarget.Array, 0, segment.PointList, Marshal.SizeOf<Point>() * pointCount);
				_gl.DrawArrays(DrawMode.LineStripWithAdjacency, 0, pointCount);
			}
		}

		private Point LastPathPoint {
			get {
				return _currentPathSegment.LastPoint;
			}
		}

		private void EmitPathPoint(float x, float y) {
			if (_pathSegmentCount == 0) {
				// Add a new, empty PathSegment
				_currentPathSegment = new PathSegment(_pointListPool.Rent(BUFFER_SIZE));
				_pathSegmentCount++;

				// Add a copy of the first point at the start for adjacency.
				// When the geometry shader sees an adjacency point the same as it's
				// real neighbour, it treats it as a start/end.
				_currentPathSegment.AddPoint(x, y, _state.StrokeColor, _state.StrokeWidth);
			} else if (_currentPathSegment.PointCount == 2) {
				// fic up the position of the first adjacency point

				var (x1, y1, _, _) = _currentPathSegment.PointList[1];

				var (x2, y2) = ExtrapolatePoint(x, y, x1, y, 1);

				_currentPathSegment.PointList[0].X = x2;
				_currentPathSegment.PointList[0].Y = y2;
			} else if (_currentPathSegment.PointCount == BUFFER_SIZE - 1) {
				// Add a new PathSegment with the last point of the previous segment as the start adjacemcy point
				var newPathSegment = new PathSegment(_pointListPool.Rent(BUFFER_SIZE));

				var (lastX, lastY, color, width) = LastPathPoint;
				newPathSegment.AddPoint(lastX, lastY, color, width);

				// we also add the new point as the end adjacency point in the previous segment
				//_currentPathSegment.AddPoint(x, y, _state.StrokeColor, _state.StrokeWidth);

				var (x2, y2) = ExtrapolatePoint(lastX, lastY, x, y, 1);
				_currentPathSegment.AddPoint(x2, y2, color, width);

				// finish the segment
				_currentPathSegment.IsFinished = true;

				_pathSegments.Add(_currentPathSegment);
				_currentPathSegment = newPathSegment;
				_pathSegmentCount++;
			}

			_currentPathSegment.AddPoint(x, y, _state.StrokeColor, _state.StrokeWidth);
		}

		private (float X, float Y) ExtrapolatePoint(float x0, float y0, float x1, float y1, float d) {
			float length = MathF.Sqrt(MathF.Pow(x1 - x0, 2) + MathF.Pow(y1 - y0, 2));

			float slopeX = (x1 - x0) / length;
			float slopeY = (y1 - y0) / length;

			return (
				x1 + slopeX * d,
				y1 + slopeY * d
			);
		}

		private class State {
			public float StrokeWidth;
			public Vector4 StrokeColor;
			public float MitreLimit;
			public float Alpha;

			public State() {
				StrokeWidth = 1f;
				StrokeColor = new Vector4(1f, 1f, 1f, 1f);
				MitreLimit = 10f;
				Alpha = 1f;
			}

			public State(State other) {
				StrokeWidth = other.StrokeWidth;
				StrokeColor = other.StrokeColor;
				MitreLimit = other.MitreLimit;
				Alpha = other.Alpha;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct Point {
			public float X;
			public float Y;
			public Vector4 Color;
			public float Width;

			public Point(float x, float y, Vector4 color, float width) {
				X = x;
				Y = y;
				Color = color;
				Width = width;
			}

			public void Deconstruct(out float x, out float y, out Vector4 color, out float width) {
				x = X;
				y = Y;
				color = Color;
				width = Width;
			}
		}

		private struct PathSegment {
			public Point[] PointList;
			public int PointCount;
			public bool IsFinished;

			public PathSegment(Point[] pointList) {
				PointList = pointList;
				PointCount = 0;
				IsFinished = false;
			}

			public void AddPoint(float x, float y, Vector4 color, float width) {
				PointList[PointCount++] = new Point(x, y, color, width);
			}

			public Point LastPoint {
				get {
					return PointList[PointCount - 1];
				}
			}
		}
	}
}

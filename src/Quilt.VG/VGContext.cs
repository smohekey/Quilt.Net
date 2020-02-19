using System.Buffers;
namespace Quilt.VG {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Numerics;
	using System.Runtime.InteropServices;
	using Quilt.GL;

	public class VGContext {
		private const int BUFFER_SIZE = 1024;

		private readonly ArrayPool<Vector2> _pointListPool = ArrayPool<Vector2>.Create();
		private readonly List<PathSegment> _pathSegments = new List<PathSegment>();

		private readonly GLContext _gl;
		private readonly GLVertexArray _arcVA;
		private readonly GLBuffer _arcVB;
		private readonly GLProgram _pathProgram;

		private PathSegment _currentSegment;
		private int _pathSegmentCount = 0;

		public VGContext(GLContext gl) {
			_gl = gl;

			_arcVA = _gl.CreateVertexArray();
			_arcVB = _gl.CreateBuffer();

			// rent a buffer temporarily for the initial call to BufferData
			var pointList = _pointListPool.Rent(BUFFER_SIZE);

			gl.BindVertexArray(_arcVA);
			gl.BindBuffer(BufferTarget.Array, _arcVB);
			gl.BufferData(BufferTarget.Array, pointList, BufferUsage.StreamDraw);

			_pointListPool.Return(pointList);

			gl.VertexAttribPointer(0, 2, DataType.Float, false, Marshal.SizeOf<float>() * 2, 0);
			gl.EnableVertexAttribArray(0);

			var assembly = typeof(VGContext).Assembly;

			var pathVertexSource = assembly.GetManifestResourceStream($"Quilt.VG.Shaders.Path.vert")!;
			var pathVertexShader = _gl.CreateVertexShader(pathVertexSource);

			var pathFragmentSource = assembly.GetManifestResourceStream("Quilt.VG.Shaders.Path.frag")!;
			var pathFragmentShader = _gl.CreateFragmentShader(pathFragmentSource);

			_pathProgram = _gl.CreateProgram(pathVertexShader, pathFragmentShader);
		}

		public void BeginFrame(int width, int height) {
			int projectionUniform = _gl.GetUniformLocation(_pathProgram, "u_projection");

			var projection = Matrix4x4.CreateOrthographicOffCenter(0, width, height, 0, -1, 1);

			_gl.UseProgram(_pathProgram);
			_gl.UniformMatrix(projectionUniform, 1, false, projection);
		}

		public void BeginPath(Vector2 start) {
			foreach (PathSegment segment in _pathSegments) {
				_pointListPool.Return(segment.PointList);
			}

			_pathSegments.Clear();
			_pathSegmentCount = 0;

			AddPathPoint(start);
		}

		public void LineTo(Vector2 target) {
			AddPathPoint(target);
		}

		public void ArcTo(Vector2 end, Vector2 center) {
			bool clockwise = true;

			var start = LastPathPoint;

			Console.WriteLine($"start: {start}, center: {center}, end: {end}");

			var totalAngle = 2 * MathF.PI * MathF.Atan2(end.Y - center.Y, end.X - center.X) - MathF.Atan2(start.Y - center.Y, start.X - center.X);

			Console.WriteLine($"totalAngle: {totalAngle}");

			var arcPointCount = Math.Max(1, Math.Min((int)(MathF.Abs(totalAngle) / (MathF.PI * 0.5f) + 0.5f), 5));

			Console.WriteLine($"arcPointCount: {arcPointCount}");

			var a = totalAngle / arcPointCount;
			var r = MathF.Sqrt(MathF.Pow(start.X - center.X, 2) + MathF.Pow(start.Y - center.Y, 2));

			Console.WriteLine($"r: {r}");

			var angle = MathF.Atan2(start.Y - center.Y, start.X - center.X);

			AddPathPoint(start);

			for (int i = 0; i < arcPointCount; i++) {
				if (clockwise) {
					angle -= a / r;
				} else {
					angle += a / r;
				}

				var x = center.X + r * MathF.Cos(angle);
				var y = center.Y + r * MathF.Sin(angle);

				Console.WriteLine($"point: {x}, {y}");

				AddPathPoint(new Vector2(x, y));
			}

			AddPathPoint(end);
		}

		public void BezierTo(Vector2 p1, Vector2 p2, Vector2 p3) {
			const int COUNT = 100;

			var start = LastPathPoint;

			float dt = 1.0f / COUNT;
			float t = 0.0f;
			for (int i = 0; i <= COUNT; i++) {
				var x = MathF.Pow((1 - t), 3) * start.X + 3 * t * MathF.Pow((1 - t), 2) * p1.X + 3 * (1 - t) * MathF.Pow(t, 2) * p2.X + MathF.Pow(t, 3) * p3.X;
				var y = MathF.Pow((1 - t), 3) * start.Y + 3 * t * MathF.Pow((1 - t), 2) * p1.Y + 3 * (1 - t) * MathF.Pow(t, 2) * p2.Y + MathF.Pow(t, 3) * p3.Y;

				AddPathPoint(x, y);

				t += dt;
			}


		}

		public void Stroke() {
			_gl.UseProgram(_pathProgram);
			_gl.BindVertexArray(_arcVA);
			_gl.BindBuffer(BufferTarget.Array, _arcVB);

			foreach (var segment in _pathSegments.Append(_currentSegment)) {
				var pointCount = segment.PointCount;

				_gl.BufferSubData(BufferTarget.Array, 0, segment.PointList, Marshal.SizeOf<Vector2>() * pointCount);

				_gl.DrawArrays(DrawMode.LineStrip, 0, pointCount);
			}
		}

		private Vector2 LastPathPoint {
			get {
				return _currentSegment.PointList[_currentSegment.PointCount - 1];
			}
		}

		private void AddPathPoint(float x, float y) {
			AddPathPoint(new Vector2(x, y));
		}

		private void AddPathPoint(Vector2 vertex) {
			if (_pathSegmentCount == 0) {
				// add a new, empty PathSegment
				_currentSegment = new PathSegment(_pointListPool.Rent(BUFFER_SIZE));
				_pathSegmentCount++;
			} else if (_currentSegment.PointCount == BUFFER_SIZE) {
				// add a new PathSegment with the last point of the previous segment as the first point
				var newSegment = new PathSegment(_pointListPool.Rent(BUFFER_SIZE));

				newSegment.PointList[0] = LastPathPoint;
				newSegment.PointCount = 1;

				_pathSegments.Add(_currentSegment);
				_currentSegment = newSegment;
				_pathSegmentCount++;
			}

			_currentSegment.PointList[_currentSegment.PointCount++] = vertex;
		}

		private struct PathSegment {
			public Vector2[] PointList;
			public int PointCount;

			public PathSegment(Vector2[] vertexList) {
				PointList = vertexList;
				PointCount = 0;
			}
		}
	}
}

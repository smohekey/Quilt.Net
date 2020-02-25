using System.Reflection.Metadata;
namespace Quilt.VG {
	using System;
	using System.Numerics;
	using System.Runtime.InteropServices;
	using Quilt.Collections;
	using Quilt.GL;

	internal class FillRenderer : Renderer {
		private static readonly int __pointSize = Marshal.SizeOf<Point>();
		private static readonly int __vector2Size = Marshal.SizeOf<Vector2>();
		private static readonly int __vector4Size = Marshal.SizeOf<Vector4>();

		private readonly Point[] _pointArray = new Point[Constants.BUFFER_SIZE];

		private readonly Pool<PooledList<Point>> _fragmentPool = new Pool<PooledList<Point>>();
		private readonly PooledList<PooledList<Point>> _fragments = new PooledList<PooledList<Point>>();

		private readonly GLVertexArray _pointsVA;
		private readonly GLBuffer _pointsVB;
		private readonly GLProgram _program;
		private readonly int _projectionUniform;

		public FillRenderer(GLContext gl) : base(gl) {
			_pointsVA = gl.CreateVertexArray();
			_pointsVB = gl.CreateBuffer();

			gl.BindVertexArray(_pointsVA);
			gl.BindBuffer(BufferTarget.Array, _pointsVB);
			gl.BufferData(BufferTarget.Array, _pointArray, BufferUsage.StreamDraw);

			gl.EnableVertexAttribArray(0);
			gl.EnableVertexAttribArray(1);
			gl.EnableVertexAttribArray(2);
			gl.EnableVertexAttribArray(3);

			var index = 0u;
			var offset = 0;

			gl.VertexAttribPointer(index++, 2, DataType.Float, false, __pointSize, offset); offset += __vector2Size; // Position
			gl.VertexAttribPointer(index++, 4, DataType.Float, false, __pointSize, offset); offset += __vector4Size; // Color

			var assembly = typeof(FillRenderer).Assembly;

			using var vertexShaderSource = assembly.GetManifestResourceStream($"Quilt.VG.Shaders.Fill.vert")!;
			using var vertexShader = _gl.CreateVertexShader(vertexShaderSource);

			using var geometryShaderSource = assembly.GetManifestResourceStream($"Quilt.VG.Shaders.Fill.geom")!;
			using var geometryShader = _gl.CreateGeometryShader(geometryShaderSource);

			using var fragmentShaderSource = assembly.GetManifestResourceStream("Quilt.VG.Shaders.Fill.frag")!;
			using var fragmentShader = _gl.CreateFragmentShader(fragmentShaderSource);

			_program = _gl.CreateProgram(vertexShader/*, geometryShader*/, fragmentShader);

			_projectionUniform = _gl.GetUniformLocation(_program, "_projection");
		}

		public override void Render(IFrameBuilder frame, Matrix4x4 projection, Vector2 viewport, IPath path) {
			_gl.UseProgram(_program);

			_gl.UniformMatrix(_projectionUniform, 1, false, projection);

			_gl.BindVertexArray(_pointsVA);
			_gl.BindBuffer(BufferTarget.Array, _pointsVB);

			_gl.Disable(Capability.CullFace);
			_gl.Clear(BufferBit.Stencil);
			_gl.ClearStencil(0);

			_gl.Enable(Capability.StencilTest);

			//glStencilOp(GL_INVERT, GL_INVERT, GL_INVERT);
			//glStencilFunc(GL_ALWAYS, 1, 1);
			_gl.ColorMask(false, false, false, false);
			_gl.StencilMask(1);
			_gl.StencilOp(StencilOperation.Keep, StencilOperation.Keep, StencilOperation.Invert);
			_gl.StencilFunc(StencilFunc.Always, 0, ~0u);

			DrawPoints(path);

			//_gl.StencilMask(0);
			_gl.StencilOp(StencilOperation.Zero, StencilOperation.Zero, StencilOperation.Zero);
			_gl.StencilFunc(StencilFunc.EqualTo, 1, 1);
			_gl.ColorMask(true, true, true, true);

			DrawPoints(path);

			_gl.Disable(Capability.StencilTest);

			/*			BuildFragments(path);

						foreach (var fragment in _fragments) {
							var pointCount = 0;
							var previousFront = default(Point?);
							var previousBack = default(Point?);

							for (int i = 0, j = fragment.Count - 1; i <= j; i++, j--) {
								if (pointCount == 0 && previousFront.HasValue && previousBack.HasValue) {
									_pointArray[pointCount++] = previousFront.Value;
									_pointArray[pointCount++] = previousBack.Value;
								}

								previousFront = _pointArray[pointCount++] = fragment[i];
								previousBack = _pointArray[pointCount++] = fragment[j];

								if (pointCount >= _pointArray.Length) {
									FlushBuffer();

									pointCount = 0;
								}
							}

							if (pointCount > 0) {
								FlushBuffer();
							}

							void FlushBuffer() {
			#if DEBUG
								Console.WriteLine("Flushing fill buffer...");

								for (int i = 0; i < pointCount; i++) {
									var p = _pointArray[i];

									Console.WriteLine($"{p.Position.X}, {p.Position.Y}");
								}
			#endif

								_gl.BufferSubData(BufferTarget.Array, 0, _pointArray, __pointSize * pointCount);
								_gl.DrawArrays(DrawMode.TriangleStrip, 0, pointCount);
							}
						}

						foreach (var fragment in _fragments) {
							_fragmentPool.Return(fragment);
						}

						_fragments.Clear();*/
		}

		private void DrawPoints(IPath path) {
			var pointCount = 0;

			var first = default(Point?);

			foreach (var point in path) {
				if (!first.HasValue) {
					first = new Point(point.Position, point.FillColor);

					_pointArray[pointCount++] = first.Value;

					continue;
				}

				if (pointCount == 0) {
					_pointArray[pointCount++] = first.Value;
				}

				_pointArray[pointCount++] = new Point(point.Position, point.FillColor);

				if (pointCount >= _pointArray.Length) {
					FlushBuffer();

					pointCount = 0;
				}
			}

			if (pointCount > 0) {
				FlushBuffer();
			}

			void FlushBuffer() {
#if DEBUG
				Console.WriteLine("Flushing fill buffer...");

				for (int i = 0; i < pointCount; i++) {
					var p = _pointArray[i];

					Console.WriteLine($"{p.Position.X}, {p.Position.Y}");
				}
#endif

				_gl.BufferSubData(BufferTarget.Array, 0, _pointArray, __pointSize * pointCount);
				_gl.DrawArrays(DrawMode.TriangleFan, 0, pointCount);
			}
		}

		// private void BuildFragments(IPath path) {
		// 	var fragment = _fragmentPool.Rent();
		// 	var firstFragment = default(PooledArray<Point>?);

		// 	_fragments.Add(fragment);

		// 	var count = 0;
		// 	var p0 = default(Point);
		// 	var p1 = default(Point);

		// 	var right = path.WindingOrder == WindingOrder.Clockwise;

		// 	var enumerator = right ? path.GetEnumerator() : path.GetReverseEnumerator();

		// 	while (enumerator.MoveNext()) {
		// 		var pathPoint = enumerator.Current;

		// 		var p2 = new Point(pathPoint.Position, pathPoint.FillColor);

		// 		if (count > 1) {
		// 			if (Triangle.IsConvex(p0.Position, p1.Position, p2.Position) && !Triangle.ContainsPoint(path, p0.Position, p1.Position, p2.Position)) {

		// 			}
		// 		}

		// 		fragment.Add(p2);

		// 		p0 = p1;
		// 		p1 = p2;

		// 		count++;
		// 	}
		// }


		// private static float GetDirection(ref Vector2 a, ref Vector2 b, ref Vector2 c) {
		// 	return (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
		// }

		private struct Point {
			public Vector2 Position;
			public Color Color;

			public Point(Vector2 position, Color color) {
				Position = position;
				Color = color;
			}
		}
	}
}

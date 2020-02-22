namespace Quilt.VG {
	using System;
	using System.Numerics;
	using System.Runtime.InteropServices;
	using Quilt.GL;

	internal class StrokeRenderer : Renderer {
		private static readonly int __uintSize = Marshal.SizeOf<uint>();
		private static readonly int __vector2Size = Marshal.SizeOf<Vector2>();
		private static readonly int __vector4Size = Marshal.SizeOf<Vector4>();
		private static readonly int __floatSize = Marshal.SizeOf<float>();
		private static readonly int __pointSize = Marshal.SizeOf<Point>();

		private readonly Point[] _points = new Point[Constants.BUFFER_SIZE];

		private readonly GLVertexArray _pointsVA;
		private readonly GLBuffer _pointsVB;
		private readonly GLProgram _program;
		private readonly int _projectionUniform;
		private readonly int _viewportUniform;
		private readonly int _miterLimitUniform;

		public StrokeRenderer(GLContext gl) : base(gl) {
			_pointsVA = _gl.CreateVertexArray();
			_pointsVB = _gl.CreateBuffer();

			gl.BindVertexArray(_pointsVA);
			gl.BindBuffer(BufferTarget.Array, _pointsVB);
			gl.BufferData(BufferTarget.Array, _points, BufferUsage.StreamDraw);

			gl.EnableVertexAttribArray(0);
			gl.EnableVertexAttribArray(1);
			gl.EnableVertexAttribArray(2);
			gl.EnableVertexAttribArray(3);

			var index = 0u;
			var offset = 0;

			gl.VertexAttribPointer(index++, 2, DataType.Float, false, __pointSize, offset); offset += __vector2Size; // Position
			gl.VertexAttribPointer(index++, 1, DataType.UnsignedInt, false, __pointSize, offset); offset += __uintSize; // Flags
			gl.VertexAttribPointer(index++, 4, DataType.Float, false, __pointSize, offset); offset += __vector4Size; // Color
			gl.VertexAttribPointer(index++, 1, DataType.Float, false, __pointSize, offset); offset += __floatSize; // Width

			var assembly = typeof(VGContext).Assembly;

			var strokeVertexSource = assembly.GetManifestResourceStream($"Quilt.VG.Shaders.Stroke.vert")!;
			var strokeVertexShader = _gl.CreateVertexShader(strokeVertexSource);

			var strokeGeometrySource = assembly.GetManifestResourceStream($"Quilt.VG.Shaders.Stroke.geom")!;
			var strokeGeometryShader = _gl.CreateGeometryShader(strokeGeometrySource);

			var strokeFragmentSource = assembly.GetManifestResourceStream("Quilt.VG.Shaders.Stroke.frag")!;
			var strokeFragmentShader = _gl.CreateFragmentShader(strokeFragmentSource);

			_program = _gl.CreateProgram(strokeVertexShader, strokeGeometryShader, strokeFragmentShader);

			_projectionUniform = _gl.GetUniformLocation(_program, "_projection");
			_viewportUniform = _gl.GetUniformLocation(_program, "_viewport");
			_miterLimitUniform = _gl.GetUniformLocation(_program, "_miterLimit");
		}

		public override void Render(FrameBuilder frame, Matrix4x4 projection, Vector2 viewport, Path path) {
			_gl.UseProgram(_program);

			_gl.UniformMatrix(_projectionUniform, 1, false, projection);
			_gl.Uniform(_viewportUniform, viewport.X, viewport.Y);
			_gl.Uniform(_miterLimitUniform, 1f);

			_gl.BindVertexArray(_pointsVA);
			_gl.BindBuffer(BufferTarget.Array, _pointsVB);

			var color = frame.StrokeColor;
			var width = frame.StrokeWidth;
			var flags = frame.StrokeFlags;

			var pendingExtrapolate = false;
			var previous = default(Point?);
			var pointCount = 0;
			var tail1 = default(Point);
			var tail2 = default(Point);

			var mainEnumerator = path.GetEnumerator();

			while (mainEnumerator.MoveNext()) {
				var command = mainEnumerator.Current;

				switch (command.Type) {
					case CommandType.SetStrokeColor: {
						color = command.StrokeColor;

						break;
					}

					case CommandType.SetStrokeWidth: {
						width = command.StrokeWidth;

						break;
					}

					case CommandType.SetStrokeFlags: {
						flags = command.StrokeFlags;

						break;
					}

					case CommandType.SetPosition: {
						if (pendingExtrapolate) {
							// now that we have two positions,
							// we have to extrapolate the first point in a path

							var p1 = _points[pointCount - 1];
							var p2 = ExtrapolatePoint(command.Position, p1.Position, 1);

							_points[0] = new Point(p2, p1.Flags, p1.Color, p1.Width);

							pendingExtrapolate = false;
						}

						if (pointCount == 0) {
							// First point in batch
							if (!previous.HasValue) {
								// If we don't have a previous point, set it to empty for now
								// and queue a pending extrapolation
								pendingExtrapolate = true;

								previous = default(Point);
							}

							// frist point in a batch is an adjacency point, either the previous one,
							// or a yet to be extrapolated one
							_points[pointCount++] = previous.Value;
						}

						previous = _points[pointCount++] = new Point(command.Position, flags, color, width);

						if (pointCount == _points.Length - 1) {
							// Here we have to determine whether the final point in this batch
							// is the final point overall, if it is we have to extrapolate an adjacency
							// point from this point and the previous one.
							// We take a copy of mainEnumerator here to enumerate forward for
							// the next point without affecting the mainEnumerator
							// (it is a struct to achieve that).

							var enumerator = mainEnumerator;
							var next = default(Point?);

							while (enumerator.MoveNext()) {
								var c = enumerator.Current;

								if (c.Type == CommandType.SetPosition) {
									var current = _points[pointCount];

									next = new Point(c.Position, current.Flags, current.Color, current.Width);
								}
							}

							if (!next.HasValue) {
								// A next point wasn't found, so we extrapolate one from the current point
								// and the previous one

								var p0 = _points[pointCount - 1];
								var p2 = ExtrapolatePoint(p0.Position, command.Position, 1);

								next = new Point(p2, flags, color, width);
							}

							_points[pointCount++] = next.Value;
						}

						tail1 = tail2;
						tail2 = previous.Value;

						break;
					}
				}

				if (pointCount == _points.Length) {
					FlushBuffer();

					pointCount = 0;
				}
			}

			if (pointCount > 0) {
				var p2 = ExtrapolatePoint(tail1.Position, tail2.Position, 1);

				_points[pointCount++] = new Point(p2, tail2.Flags, tail2.Color, tail2.Width);

				FlushBuffer();
			}

			void FlushBuffer() {
#if DEBUG
				Console.WriteLine("Flushing buffer...");

				for (int i = 0; i < pointCount; i++) {
					var p = _points[i];
					var adjacency = (i == 0 || i == pointCount - 1) ? "*" : "";

					Console.WriteLine($"{p.Position.X}, {p.Position.Y} {adjacency}");
				}
#endif

				_gl.BufferSubData(BufferTarget.Array, 0, _points, __pointSize * pointCount);
				_gl.DrawArrays(DrawMode.LineStripWithAdjacency, 0, pointCount);
			}
		}

		private struct Point {
			public Vector2 Position;
			public StrokeFlags Flags;
			public Vector4 Color;
			public float Width;

			public Point(Vector2 position, StrokeFlags flags, Vector4 color, float width) {
				Position = position;
				Flags = flags;
				Color = color;
				Width = width;
			}
		}
	}
}

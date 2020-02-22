namespace Quilt.VG {
	using System;
	using System.Numerics;
	using System.Runtime.InteropServices;
	using Quilt.GL;

	internal class FillRenderer : Renderer {
		private readonly Point[] _points = new Point[Constants.BUFFER_SIZE];
		private readonly int __pointSize = Marshal.SizeOf<Point>();
		private readonly int __vector2Size = Marshal.SizeOf<Vector2>();
		private readonly int __vector4Size = Marshal.SizeOf<Vector4>();

		private readonly GLVertexArray _pointsVA;
		private readonly GLBuffer _pointsVB;
		private readonly GLProgram _program;
		private readonly int _projectionUniform;
		private readonly int _viewportUniform;
		private readonly int _centerPositionUniform;
		private readonly int _centerColorUniform;

		public FillRenderer(GLContext gl) : base(gl) {
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
			gl.VertexAttribPointer(index++, 4, DataType.Float, false, __pointSize, offset); offset += __vector4Size; // Color

			var assembly = typeof(FillRenderer).Assembly;

			using var vertexShaderSource = assembly.GetManifestResourceStream($"Quilt.VG.Shaders.Fill.vert")!;
			using var vertexShader = _gl.CreateVertexShader(vertexShaderSource);

			using var geometryShaderSource = assembly.GetManifestResourceStream($"Quilt.VG.Shaders.Fill.geom")!;
			using var geometryShader = _gl.CreateGeometryShader(geometryShaderSource);

			using var fragmentShaderSource = assembly.GetManifestResourceStream("Quilt.VG.Shaders.Fill.frag")!;
			using var fragmentShader = _gl.CreateFragmentShader(fragmentShaderSource);

			_program = _gl.CreateProgram(vertexShader, geometryShader, fragmentShader);

			_projectionUniform = _gl.GetUniformLocation(_program, "_projection");
			_viewportUniform = _gl.GetUniformLocation(_program, "_viewport");
			_centerPositionUniform = _gl.GetUniformLocation(_program, "_centerPosition");
			_centerColorUniform = _gl.GetUniformLocation(_program, "_centerColor");
		}

		public override void Render(FrameBuilder frame, Matrix4x4 projection, Vector2 viewport, Path path) {
			_gl.UseProgram(_program);

			_gl.UniformMatrix(_projectionUniform, 1, false, projection);
			_gl.Uniform(_viewportUniform, viewport.X, viewport.Y);

			_gl.BindVertexArray(_pointsVA);
			_gl.BindBuffer(BufferTarget.Array, _pointsVB);

			var color = frame.StrokeColor;

			var centered = false;
			var pointCount = 0;

			foreach (var command in path) {
				switch (command.Type) {
					case CommandType.SetFillColor: {
						color = command.FillColor;

						break;
					}

					case CommandType.SetPosition: {
						if (!centered) {
							var center = Vector2.Transform(command.Position, projection);

							_gl.Uniform(_centerPositionUniform, center.X, center.Y);
							_gl.Uniform(_centerColorUniform, color.X, color.Y, color.Z, color.W);

							centered = true;

							continue;
						}

						_points[pointCount++] = new Point(command.Position, color);

						break;
					}

					default: {
						break;
					}
				}

				if (pointCount == _points.Length) {
					FlushBuffer();

					centered = false;
					pointCount = 0;
				}
			}

			if (pointCount > 0) {
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
				_gl.DrawArrays(DrawMode.LineStrip, 0, pointCount);
			}
		}

		private struct Point {
			public Vector2 Position;
			public Vector4 Color;

			public Point(Vector2 position, Vector4 color) {
				Position = position;
				Color = color;
			}
		}
	}
}

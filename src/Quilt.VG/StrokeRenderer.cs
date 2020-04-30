namespace Quilt.VG {
	using System.Numerics;
	using Quilt.GL;

	internal class StrokeRenderer : Renderer {
		private readonly GLProgram _program;
		private readonly int _projectionUniform;
		private readonly int _viewportUniform;
		private readonly int _colorUniform;
		private readonly int _flagsUniform;
		private readonly int _widthUniform;
		private readonly int _miterLimitUniform;

		public StrokeRenderer(GLContext gl) : base(gl) {
			var assembly = typeof(Surface).Assembly;

			var strokeVertexSource = assembly.GetManifestResourceStream($"Quilt.VG.Shaders.Stroke.vert")!;
			var strokeVertexShader = _gl.CreateVertexShader(strokeVertexSource);

			var strokeGeometrySource = assembly.GetManifestResourceStream($"Quilt.VG.Shaders.Stroke.geom")!;
			var strokeGeometryShader = _gl.CreateGeometryShader(strokeGeometrySource);

			var strokeFragmentSource = assembly.GetManifestResourceStream("Quilt.VG.Shaders.Stroke.frag")!;
			var strokeFragmentShader = _gl.CreateFragmentShader(strokeFragmentSource);

			_program = _gl.CreateProgram(strokeVertexShader, strokeGeometryShader, strokeFragmentShader);

			_projectionUniform = _gl.GetUniformLocation(_program, "_projection");
			_viewportUniform = _gl.GetUniformLocation(_program, "_viewport");
			_colorUniform = _gl.GetUniformLocation(_program, "_color");
			_flagsUniform = _gl.GetUniformLocation(_program, "_flags");
			_widthUniform = _gl.GetUniformLocation(_program, "_width");
			_miterLimitUniform = _gl.GetUniformLocation(_program, "_miterLimit");
		}

		public override void Render(ref Context context, Matrix4x4 projection, Vector2 viewport, Path path) {
			_gl.UseProgram(_program);

			_gl.UniformMatrix(_projectionUniform, 1, false, projection);
			_gl.Uniform(_viewportUniform, viewport.X, viewport.Y);
			_gl.Uniform(_colorUniform, context._state._strokeColor);
			_gl.Uniform(_flagsUniform, (int)context._state._strokeJoinStyle << 8 | (int)context._state._strokeCapStyle);
			_gl.Uniform(_widthUniform, context._state._strokeWidth);
			_gl.Uniform(_miterLimitUniform, context._state._strokeMiterLimit);

			_gl.BindVertexArray(path._vertexArray);
			_gl.BindBuffer(BufferTarget.Array, path._vertexBuffer);

			foreach (var contour in path) {
				_gl.DrawArrays(DrawMode.LineStripWithAdjacency, contour._offset, contour._length);
			}
		}
	}
}

namespace Quilt.VG {
	using System.Numerics;
	using Quilt.GL;

	internal class FillRenderer : Renderer {
		private readonly GLProgram _program;
		private readonly int _mvpUniform;
		private readonly int _colorUniform;

		public FillRenderer(GLContext gl) : base(gl) {
			var assembly = typeof(FillRenderer).Assembly;

			using var vertexShaderSource = assembly.GetManifestResourceStream($"Quilt.VG.Shaders.Fill.vert")!;
			using var vertexShader = _gl.CreateVertexShader(vertexShaderSource);

			using var fragmentShaderSource = assembly.GetManifestResourceStream("Quilt.VG.Shaders.Fill.frag")!;
			using var fragmentShader = _gl.CreateFragmentShader(fragmentShaderSource);

			_program = _gl.CreateProgram(vertexShader, fragmentShader);

			_mvpUniform = _gl.GetUniformLocation(_program, "_mvp");
			_colorUniform = _gl.GetUniformLocation(_program, "_color");
		}

		public override void Render(ref Context context, Matrix4x4 mvp, Vector2 viewport, Path path) {
			_gl.UseProgram(_program);

			_gl.UniformMatrix(_mvpUniform, 1, false, mvp);
			_gl.Uniform(_colorUniform, context._state._fillColor);

			_gl.BindVertexArray(path._vertexArray);
			_gl.BindBuffer(BufferTarget.Array, path._vertexBuffer);

			_gl.Disable(Capability.CullFace);
			_gl.Clear(BufferBit.Stencil);
			_gl.ClearStencil(0);

			_gl.Enable(Capability.StencilTest);

			_gl.ColorMask(false, false, false, false);
			_gl.StencilMask(1);
			_gl.StencilOp(StencilOperation.Keep, StencilOperation.Keep, StencilOperation.Invert);
			_gl.StencilFunc(StencilFunc.Always, 0, ~0u);

			foreach (var contour in path) {
				_gl.DrawArrays(DrawMode.TriangleFan, contour._offset + 1, contour._length - 1);
			}

			_gl.StencilOp(StencilOperation.Zero, StencilOperation.Zero, StencilOperation.Zero);
			_gl.StencilFunc(StencilFunc.EqualTo, 1, 1);
			_gl.ColorMask(true, true, true, true);

			foreach (var contour in path) {
				_gl.DrawArrays(DrawMode.TriangleFan, contour._offset + 1, contour._length - 1);
			}

			_gl.Disable(Capability.StencilTest);
		}
	}
}

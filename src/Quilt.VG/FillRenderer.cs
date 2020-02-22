namespace Quilt.VG {
	using System.Numerics;
	using Quilt.GL;

	internal class FillRenderer : Renderer {
		private readonly GLProgram _program;
		private readonly int _projectionUniform;
		private readonly int _viewportUniform;
		private readonly int _centerUniform;

		public FillRenderer(GLContext gl) : base(gl) {
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
			_centerUniform = _gl.GetUniformLocation(_program, "_center");
		}

		public override void Render(FrameBuilder frame, Matrix4x4 projection, Vector2 viewport, CommandList commands) {
			throw new System.NotImplementedException();
		}
	}
}

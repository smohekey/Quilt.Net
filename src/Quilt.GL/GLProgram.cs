namespace Quilt.GL {
  using Quilt.GL.Unmanaged;

	public class GLProgram : GLObject {
		private readonly Unmanaged.Program _program;

		internal GLProgram(IGL gl, Unmanaged.Program program) : base(gl) {
			_program = program;
		}

		public void AttachShader(GLShader shader) {
			_gl.AttachShader(_program, shader);

			CheckError();
		}
	}
}

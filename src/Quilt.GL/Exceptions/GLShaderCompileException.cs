namespace Quilt.GL.Exceptions {
	using System;

	public class GLShaderCompileException : Exception {
		private GLShader _shader;

		public GLShaderCompileException(GLShader shader, string message) : base(message) {
			_shader = shader;
		}
	}
}

namespace Quilt.GL.Exceptions {
	using System;

	public class GLProgramLinkException : Exception {
		private GLProgram _program;

		public GLProgramLinkException(GLProgram program, string message) : base(message) {
			_program = program;

		}
	}
}

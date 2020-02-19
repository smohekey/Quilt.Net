namespace Quilt.GL {
	using System;
	using System.Runtime.InteropServices;
	using System.Text;
	using Quilt.GL.Exceptions;
	using Quilt.GL.Unmanaged;
	using Quilt.Unmanaged;

	[UnmanagedObject(CallingConvention = CallingConvention.Cdecl, Prefix = "gl")]
	public abstract class GLProgram : GLObject {
		protected GLProgram(UnmanagedLibrary library, GLContext context, uint handle, GLShader?[] shaders) : base(library, context, handle) {
			foreach (var shader in shaders) {
				if (shader != null) {
					AttachShader(shader);
				}
			}

			Link();
		}

		protected abstract void GetProgramiv(uint prgram, ProgramProperty property, out int value);

		private bool IsLinked {
			get {
				GetProgramiv(_handle, ProgramProperty.LinkStatus, out var value);

				CheckError();

				return value != 0;
			}
		}

		protected abstract void GetProgramInfoLog(uint program, GLsizei maxLength, out GLsizei length, StringBuilder infoLog);

		public string InfoLog {
			get {
				var infoLog = new StringBuilder(256);
				GLsizei length;

				do {
					GetProgramInfoLog(_handle, infoLog.Capacity, out length, infoLog);

					CheckError();
				} while (length > infoLog.Capacity);

				return infoLog.ToString();
			}
		}

		protected abstract void AttachShader(uint program, uint shader);

		private void AttachShader(GLShader shader) {
			AttachShader(_handle, shader._handle);

			CheckError();
		}

		protected abstract void DetachShader(uint program, uint shader);

		private void DetachShader(GLShader shader) {
			DetachShader(_handle, shader._handle);

			CheckError();
		}

		protected abstract void LinkProgram(uint program);

		private void Link() {
			LinkProgram(_handle);

			CheckError();

			if (!IsLinked) {
				throw new GLProgramLinkException(this, InfoLog);
			}
		}

		protected abstract void DeleteProgram(uint program);

		protected override void DisposeUnmanaged() {
			DeleteProgram(_handle);
		}
	}
}

namespace Quilt.GL {
	using System.Runtime.InteropServices;
	using System.Text;
	using Quilt.GL.Exceptions;
	using Quilt.GL.Unmanaged;
	using Quilt.Unmanaged;

	[UnmanagedObject(CallingConvention = CallingConvention.Cdecl, Prefix = "gl")]
	public abstract class GLProgram : GLObject {
		protected GLProgram(UnmanagedLibrary library, int handle, GLShader[] shaders) : base(library, handle) {
			foreach (var shader in shaders) {
				AttachShader(shader);
			}

			Link();
		}

		protected abstract void GetProgramiv(int prgram, ProgramProperty property, out int value);

		private bool IsLinked {
			get {
				GetProgramiv(_handle, ProgramProperty.LinkStatus, out var value);

				CheckError();

				return value != 0;
			}
		}

		protected abstract void GetProgramInfoLog(int program, int maxLength, out int length, StringBuilder infoLog);

		public string InfoLog {
			get {
				var infoLog = new StringBuilder(256);
				int length;

				do {
					GetProgramInfoLog(_handle, infoLog.Capacity, out length, infoLog);

					CheckError();
				} while (length > infoLog.Capacity);

				return infoLog.ToString();
			}
		}

		protected abstract void AttachShader(int program, int shader);

		private void AttachShader(GLShader shader) {
			AttachShader(_handle, shader._handle);

			CheckError();
		}

		protected abstract void DetachShader(int program, int shader);

		private void DetachShader(GLShader shader) {
			DetachShader(_handle, shader._handle);

			CheckError();
		}

		protected abstract void LinkProgram(int program);

		private void Link() {
			LinkProgram(_handle);

			CheckError();

			if (!IsLinked) {
				throw new GLProgramLinkException(this, InfoLog);
			}
		}

		protected abstract void UseProgram(int program);

		public void Use() {
			UseProgram(_handle);

			CheckError();
		}

		public abstract void Uniform1f(int location, float v0);
		public abstract void Uniform2f(int location, float v0, float v1);
		public abstract void Uniform3f(int location, float v0, float v1, float v2);
		public abstract void Uniform4f(int location, float v0, float v1, float v2, float v3);

		public abstract void Uniform1i(int location, int v0);
		public abstract void Uniform2i(int location, int v0, int v1);
		public abstract void Uniform3i(int location, int v0, int v1, int v2);
		public abstract void Uniform4i(int location, int v0, int v1, int v2, int v3);

		public abstract void Uniform1ui(int location, uint v0);
		public abstract void Uniform2ui(int location, uint v0, uint v1);
		public abstract void Uniform3ui(int location, uint v0, uint v1, uint v2);
		public abstract void Uniform4ui(int location, uint v0, uint v1, uint v2, uint v3);

		public abstract void Uniform1fv(int location, out float v0);
		public abstract void Uniform2fv(int location, out float v0, out float v1);
		public abstract void Uniform3fv(int location, out float v0, out float v1, out float v2);
		public abstract void Uniform4fv(int location, out float v0, out float v1, out float v2, out float v3);

		public abstract void Uniform1iv(int location, out int v0);
		public abstract void Uniform2iv(int location, out int v0, out int v1);
		public abstract void Uniform3iv(int location, out int v0, out int v1, out int v2);
		public abstract void Uniform4iv(int location, out int v0, out int v1, out int v2, out int v3);

		public abstract void Uniform1uiv(int location, out uint v0);
		public abstract void Uniform2uiv(int location, out uint v0, out uint v1);
		public abstract void Uniform3uiv(int location, out uint v0, out uint v1, out uint v2);
		public abstract void Uniform4uiv(int location, out uint v0, out uint v1, out uint v2, out uint v3);

		protected abstract void DeleteProgram(int program);

		protected override void DisposeUnmanaged() {
			DeleteProgram(_handle);
		}
	}
}

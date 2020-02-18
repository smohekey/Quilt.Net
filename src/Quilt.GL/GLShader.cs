namespace Quilt.GL {
	using System;
	using System.Text;
	using Quilt.GL.Exceptions;
	using Quilt.GL.Unmanaged;
	using Quilt.Unmanaged;

	public abstract class GLShader : GLObject, IDisposable {
		protected GLShader(UnmanagedLibrary library, int handle, string source) : base(library, handle) {
			SetShaderSource(source);

			Compile();
		}

		protected abstract void GetShaderiv(int handle, ShaderProperty property, out int value);

		public bool IsDeleted {
			get {
				GetShaderiv(_handle, ShaderProperty.DeleteStatus, out var value);

				CheckError();

				return value != 0;
			}
		}

		private bool IsCompiled {
			get {
				GetShaderiv(_handle, ShaderProperty.CompileStatus, out var value);

				CheckError();

				return value != 0;
			}
		}

		protected abstract void ShaderSource(int shader, int count, string[] sources, int[] lengths);

		private void SetShaderSource(params string[] sources) {
			var lengths = new int[sources.Length];

			for (int i = 0; i < lengths.Length; i++) {
				lengths[i] = -1;
			}

			ShaderSource(_handle, sources.Length, sources, lengths);
		}

		protected abstract void GetShaderSource(int shader, int maxLength, out int length, StringBuilder source);

		public string Source {
			get {
				var source = new StringBuilder(256);
				int length;

				do {
					GetShaderSource(_handle, source.Capacity, out length, source);
				} while (length > source.Capacity);

				CheckError();

				return source.ToString();
			}
		}

		protected abstract void GetShaderInfoLog(int shader, int maxLength, out int length, StringBuilder infoLog);

		public string InfoLog {
			get {
				var infoLog = new StringBuilder(256);
				int length;

				do {
					GetShaderInfoLog(_handle, infoLog.Capacity, out length, infoLog);
				} while (length > infoLog.Capacity);

				CheckError();

				return infoLog.ToString();
			}
		}

		protected abstract void CompileShader(int shader);

		private void Compile() {
			CompileShader(_handle);

			CheckError();

			if (!IsCompiled) {
				throw new GLShaderCompileException(this, InfoLog);
			}
		}

		protected abstract void DeleteShader(int shader);

		protected override void DisposeUnmanaged() {
			DeleteShader(_handle);
		}
	}
}

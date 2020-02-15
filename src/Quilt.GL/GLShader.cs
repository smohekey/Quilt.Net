namespace Quilt.GL {
	using System;
  using System.Threading;
  using Quilt.GL.Unmanaged;

	public abstract class GLShader : IDisposable {
		private readonly IGL _gl;
		private readonly Shader _shader;

		protected GLShader(IGL gl, Shader shader) {
			_gl = gl;
			_shader = shader;
		}

		public bool IsDeleted { 
			get {
				_gl.GetShaderiv(_shader, ShaderProperty.DeleteStatus, out var value);

				return value != 0;
			}
		}

		public bool IsCompiled {
			get {
				_gl.GetShaderiv(_shader, ShaderProperty.CompileStatus, out var value);

				return value != 0;
			}
		}

		public string Source {
			get {
				return _gl.GetShaderSource(_shader);
			}
		}

		public string InfoLog {
			get {
				return _gl.GetShaderInfoLog(_shader);
			}
		}

		public bool TryCompile(out string infoLog) {
			_gl.CompileShader(_shader);

			infoLog = InfoLog;

			return IsCompiled;
		}

		#region IDisposable Support
		private int _disposed = 0; // To detect redundant calls

		protected virtual void Dispose(bool disposing) {
			if (Interlocked.Increment(ref _disposed) == 1) {
				if (disposing) {
					// TODO: dispose managed state (managed objects).
				}

				_gl.DeleteShader(_shader);
			}
		}

		~GLShader() {
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(false);
		}

		// This code added to correctly implement the disposable pattern.
		public void Dispose() {
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}

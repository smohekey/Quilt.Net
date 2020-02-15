namespace Quilt.GL {
  using Quilt.GL.Exceptions;
  using Quilt.GL.Unmanaged;

	public abstract class GLObject {
		protected readonly IGL _gl;

		protected GLObject(IGL gl) {
			_gl = gl;
		}

		protected void CheckError() {
			switch (_gl.GetError()) {
				case Error.None: {
					return;
				}

				case Error.InvalidEnum: {
					throw new GLInvalidEnumException();
				}

				case Error.InvalidValue: {
					throw new GLInvalidValueException();
				}

				case Error.InvalidOperatiorn: {
					throw new GLInvalidOperationException();
				}

				case Error.StackOverflow: {
					throw new GLStackOverflowException();
				}

				case Error.StackUnderflow: {
					throw new GLStackUnderflowException();
				}

				case Error.OutOfMemory: {
					throw new GLOutOfMemoryException();
				}

				case Error.InvalidFramebufferOperation: {
					throw new GLInvalidFramebufferOperationException();
				}
			}
		}
	}
}

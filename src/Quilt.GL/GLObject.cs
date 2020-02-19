namespace Quilt.GL {
	using System.Runtime.CompilerServices;
	using Quilt.GL.Exceptions;
	using Quilt.GL.Unmanaged;
	using Quilt.Unmanaged;

	public abstract class GLObject : UnmanagedObject {
		internal readonly GLContext _context;
		internal readonly uint _handle;

		protected GLObject(UnmanagedLibrary library, GLContext context, uint handle) : base(library) {
			_context = context;
			_handle = handle;
		}

		protected abstract Error GetError();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected void CheckError() {
			switch (GetError()) {
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

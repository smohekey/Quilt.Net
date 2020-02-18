namespace Quilt.GL {
  using System;
  using System.Runtime.InteropServices;
  using Quilt.GL.Unmanaged;
  using Quilt.Unmanaged;

	[UnmanagedObject(CallingConvention = CallingConvention.Cdecl, Prefix = "gl")]
	public abstract class GLBuffer : GLObject<uint> {
		protected GLBuffer(UnmanagedLibrary library, uint handle) : base(library, handle) {

		}

		protected abstract void BindBuffer(BufferType type, uint buffer);

		public Binding Bind(BufferType type) {
			BindBuffer(type, _handle);

			CheckError();

			return new Binding(this, type);
		}

		protected abstract unsafe void BufferData(BufferType type, GLsizei size, void* data, BufferUsage usage);

		public ref struct Binding {
			private readonly GLBuffer _buffer;
			private readonly BufferType _type;

			internal Binding(GLBuffer buffer, BufferType type) {
				_buffer = buffer;
				_type = type;
			}

			public unsafe void BufferData<T>(T[] data, BufferUsage usage) where T : unmanaged {
				fixed (void* ptr = data) {
					_buffer.BufferData(_type, data.Length * Marshal.SizeOf<T>(), ptr, usage);
				}

				_buffer.CheckError();
			}

			// no need to implement IDisposable for this to work, as of C#8
			public void Dispose() {
				_buffer.BindBuffer(_type, 0);
			}
		}
	}
}

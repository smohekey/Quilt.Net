namespace Quilt.GL {
	using System;
	using System.Runtime.InteropServices;
  using Quilt.GL.Unmanaged;
  using Quilt.Unmanaged;

	[UnmanagedObject(CallingConvention = CallingConvention.Cdecl, Prefix = "gl")]
	public abstract class GLVertexArray : GLObject<uint> {
		protected GLVertexArray(UnmanagedLibrary library, uint handle) : base(library, handle) {

		}

		protected abstract void BindVertexArray(uint vertexArray);

		protected abstract void VertexAttribPointer(uint index, int size, DataType type, bool normalized, GLsizei stride, GLsizei offset);
		protected abstract void VertexAttribIPointer(uint index, int size, DataType type, GLsizei stride, GLsizei offset);
		protected abstract void VertexAttribLPointer(uint index, int size, DataType type, GLsizei stride, GLsizei offset);
		protected abstract void EnableVertexAttribArray(uint index);
		protected abstract void DisableVertexAttribArray(uint index);

		public Binding Bind() {
			BindVertexArray(_handle);

			CheckError();

			return new Binding(this);
		}

		protected abstract unsafe void DeleteVertexArrays(GLsizei count, void* vertexArrays);

		private unsafe void DeleteVertexArrays(params uint[] vertexArrays) {
			fixed(void* ptr = vertexArrays) {
				DeleteVertexArrays(vertexArrays.Length, ptr);
			}
		}
		protected override void DisposeUnmanaged() {
			DeleteVertexArrays(_handle);
		}

		public ref struct Binding {
			private GLVertexArray _vertexArray;

			internal Binding(GLVertexArray vertexArray) {
				_vertexArray = vertexArray;
			}

			public void VertexAttributePointer(uint index, int size, DataType type, bool normalized, int stride, int offset) {
				_vertexArray.VertexAttribPointer(index, size, type, normalized, stride, offset);
			}

			public void VertexAttributeIPointer(uint index, int size, DataType type, int stride, int offset) {
				_vertexArray.VertexAttribIPointer(index, size, type, stride, offset);
				_vertexArray.CheckError();
			}

			public void VertexAttributeLPointer(uint index, int size, DataType type, int stride, int offset) {
				_vertexArray.VertexAttribLPointer(index, size, type, stride, offset);
				_vertexArray.CheckError();
			}

			public void EnableVertexAttribute(uint index) {
				_vertexArray.EnableVertexAttribArray(index);
			}

			public void DisableVertexAttribute(uint index) {
				_vertexArray.DisableVertexAttribArray(index);
			}

			// no need to implement IDisposable for this to work, as of C#8
			public void Dispose() {
				_vertexArray.BindVertexArray(0);
			}
		}
	}
}

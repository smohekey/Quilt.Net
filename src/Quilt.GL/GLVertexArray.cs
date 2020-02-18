namespace Quilt.GL {
	using System;
	using System.Runtime.InteropServices;
	using Quilt.Unmanaged;

	[UnmanagedObject(CallingConvention = CallingConvention.Cdecl, Prefix = "gl")]
	public abstract class GLVertexArray : GLObject {
		protected GLVertexArray(UnmanagedLibrary library, int handle) : base(library, handle) {

		}

		protected abstract void BindVertexArray(int vertexArray);

		protected abstract void VertexAttribPointer(uint index, int size, DataType type, bool normalized, int stride, int offset);
		protected abstract void VertexAttribIPointer(uint index, int size, DataType type, int stride, int offset);
		protected abstract void VertexAttribLPointer(uint index, int size, DataType type, int stride, int offset);
		protected abstract void EnableVertexAttrib(uint index);
		protected abstract void DisableVertexAttrib(uint index);

		public Binding Bind() {
			BindVertexArray(_handle);

			CheckError();

			return new Binding(this);
		}

		protected abstract void DestroyVertexArray(int vertexArray);
		protected override void DisposeUnmanaged() {
			DestroyVertexArray(_handle);
		}

		public ref struct Binding {
			private GLVertexArray _vertexArray;

			internal Binding(GLVertexArray vertexArray) {
				_vertexArray = vertexArray;
			}

			public void VertexAttributePointer(uint index, int size, DataType type, bool normalized, int stride, int offset) {
				_vertexArray.VertexAttribPointer(index, size, type, normalized, stride, offset);
				_vertexArray.CheckError();
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
				_vertexArray.EnableVertexAttrib(index);
			}

			public void DisableVertexAttribute(uint index) {
				_vertexArray.DisableVertexAttrib(index);
			}

			// no need to implement IDisposable for this to work, as of C#8
			public void Dispose() {
				_vertexArray.BindVertexArray(0);
			}
		}
	}
}

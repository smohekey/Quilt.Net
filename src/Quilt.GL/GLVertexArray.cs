namespace Quilt.GL {
	using System.Runtime.InteropServices;
	using Quilt.GL.Unmanaged;
	using Quilt.Unmanaged;

	[UnmanagedObject(CallingConvention = CallingConvention.Cdecl, Prefix = "gl")]
	public abstract class GLVertexArray : GLObject {
		protected GLVertexArray(UnmanagedLibrary library, GLContext context, uint handle) : base(library, context, handle) {

		}

		protected override void DisposeUnmanaged() {
			//_context.DeleteVertexArrays(_handle);
		}
	}
}

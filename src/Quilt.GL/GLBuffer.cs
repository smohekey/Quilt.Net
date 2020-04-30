namespace Quilt.GL {
	using System.Runtime.InteropServices;
	using Quilt.Unmanaged;

	[UnmanagedObject(CallingConvention = CallingConvention.Cdecl, Prefix = "gl")]
	public abstract class GLBuffer : GLObject {
		protected GLBuffer(UnmanagedLibrary library, GLContext context, uint handle) : base(library, context, handle) {

		}

		protected override void DisposeUnmanaged() {
			_context.DeleteBuffer(this);
		}
	}
}

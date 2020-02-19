namespace Quilt.GL {
	using System.Runtime.InteropServices;
	using Quilt.Unmanaged;

	[UnmanagedObject(CallingConvention = CallingConvention.Cdecl, Prefix = "gl")]
	public abstract class GLVertexShader : GLShader {
		protected GLVertexShader(UnmanagedLibrary library, GLContext context, uint handle, string source) : base(library, context, handle, source) {

		}
	}
}

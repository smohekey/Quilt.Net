namespace Quilt.GL {
	using System.Runtime.InteropServices;
	using Quilt.Unmanaged;

	[UnmanagedObject(CallingConvention = CallingConvention.Cdecl, Prefix = "gl")]
	public abstract class GLVertexShader : GLShader {
		protected GLVertexShader(UnmanagedLibrary library, uint handle, string source) : base(library, handle, source) {

		}
	}
}

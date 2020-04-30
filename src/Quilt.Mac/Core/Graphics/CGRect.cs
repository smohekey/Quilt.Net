namespace Quilt.Mac.Core.Graphics {
	using System.Runtime.InteropServices;
  using Quilt.Mac.CodeGen;

  [StructLayout(LayoutKind.Sequential)]
	[MarshalWith(typeof(StructMarshaler))]
	public struct CGRect {
		public CGPoint Origin;
		public CGSize Size;
	}
}

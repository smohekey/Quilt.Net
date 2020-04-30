namespace Quilt.Mac.Core.Graphics {
	using System.Runtime.InteropServices;
  using Quilt.Mac.CodeGen;

  [StructLayout(LayoutKind.Sequential)]
	[MarshalWith(typeof(StructMarshaler))]
	public struct CGSize {
		public CGFloat Width;
		public CGFloat Height;
	}
}

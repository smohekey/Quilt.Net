namespace Quilt.VG {
	using System.Numerics;
	using System.Runtime.InteropServices;

	[StructLayout(LayoutKind.Explicit)]
	public struct PathOperation {
		[FieldOffset(0)]
		public PathOperationType Type;

		[FieldOffset(4)]
		public Vector2 Position;

		[FieldOffset(4)]
		public Color StrokeColor;

		[FieldOffset(4)]
		public float StrokeWidth;

		[FieldOffset(4)]
		public float StrokeMiter;

		[FieldOffset(1)]
		public StrokeFlags StrokeFlags;

		[FieldOffset(4)]
		public Color FillColor;
	}
}

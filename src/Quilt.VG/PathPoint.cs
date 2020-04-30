namespace Quilt.VG {
	using System.Numerics;
  using System.Runtime.InteropServices;

	[StructLayout(LayoutKind.Sequential)]
	public struct PathPoint {
		internal static readonly int SIZE = Marshal.SizeOf<PathPoint>();

		public readonly Vector2 Position;

		public PathPoint(Vector2 position) {
			Position = position;
		}
	}
}

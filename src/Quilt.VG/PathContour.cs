namespace Quilt.VG {
	using System;

	public class PathContour {
		internal readonly PathPoint[] _points;
		internal readonly int _offset;
		internal readonly int _length;

		public bool IsClosed { get; }

		internal PathContour(PathPoint[] points, int offset, int length, bool isClosed) {
			_points = points;
			_offset = offset;
			_length = length;

			IsClosed = isClosed;
		}

		public ReadOnlySpan<PathPoint> Points => new ReadOnlySpan<PathPoint>(_points, _offset + 1, _length - 2);
	}
}

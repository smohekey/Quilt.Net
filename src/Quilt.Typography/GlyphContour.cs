namespace Quilt.Typography {
  using System;
	using System.Collections;
	using System.Collections.Generic;
  using System.Linq;

  public class GlyphContour : IReadOnlyList<GlyphPoint> {
		private readonly GlyphPoint[] _points;
		private readonly uint _offset;
		private readonly uint _length;

		public GlyphContour(GlyphPoint[] points, uint offset, uint length) {
			_points = points;

			_offset = offset;
			_length = length;
		}

		public int Count {
			get {
				return (int)_length;
			}
		}

		public GlyphPoint this[int index] {
			get {
				if(index >= _length) {
					throw new ArgumentOutOfRangeException(nameof(index));
				}

				return _points[_offset + index];
			}
		}

		public IEnumerator<GlyphPoint> GetEnumerator() {
			return _points.Skip((int)_offset).Take((int)_length).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
}

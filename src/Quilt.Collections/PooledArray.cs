namespace Quilt.Collections {
	using System;
	using System.Buffers;
	using System.Collections;
	using System.Collections.Generic;

	public class PooledArray<T> : IEnumerable<T>, IReverseEnumerable<T> {
		private const int INITIAL_SEGMENTS = 512;
		private const int SEGMENT_SHIFT = 10;
		private const int SEGMENT_LENGTH = 1 << SEGMENT_SHIFT;
		private const int SEGMENT_MASK = SEGMENT_LENGTH - 1;

		private static readonly ArrayPool<Segment> __segmentPool = ArrayPool<Segment>.Create();
		private static readonly ArrayPool<T> __arrayPool = ArrayPool<T>.Create();

		private Segment[] _segments;
		private int _segmentCount;

		public int Length {
			get {
				return (_segmentCount - 1) * SEGMENT_LENGTH + _segments[0]._count;
			}
			set {
				var newSegmentCount = (value >> SEGMENT_SHIFT) + 1;

				if (_segmentCount < newSegmentCount) {
					if (_segments.Length < newSegmentCount) {
						var newSegments = __segmentPool.Rent(_segments.Length * 2);

						Array.Copy(_segments, newSegments, _segments.Length);

						__segmentPool.Return(_segments);

						_segments = newSegments;
					}

					while (_segmentCount < newSegmentCount) {
						AddSegment();
					}
				} else {
					while (_segmentCount > newSegmentCount) {
						__arrayPool.Return(_segments[--_segmentCount]._items);
					}
				}

				var newItemCount = value & SEGMENT_MASK;

				_segments[_segmentCount - 1]._count = newItemCount;
			}
		}

		private void AddSegment() {
			_segments[_segmentCount++] = new Segment {
				_items = __arrayPool.Rent(SEGMENT_LENGTH),
				_count = 0
			};
		}

		public PooledArray() {
			_segments = __segmentPool.Rent(INITIAL_SEGMENTS);

			AddSegment();
		}

		public ref T this[int index] {
			get {
				if (index < 0 || index > Length) {
					throw new ArgumentOutOfRangeException(nameof(index));
				}

				return ref _segments[index >> SEGMENT_SHIFT]._items[index & SEGMENT_MASK];
			}
		}

		public IEnumerator<T> GetEnumerator() {
			return new ForwardEnumerator(_segments, _segmentCount);
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public IEnumerator<T> GetReverseEnumerator() {
			return new ReverseEnumerator(_segments, _segmentCount);
		}

		private struct Segment {
			public T[] _items;
			public int _count;
		}

		private struct ForwardEnumerator : IEnumerator<T> {
			private readonly Segment[] _segments;
			private readonly int _segmentCount;
			private int _segmentIndex;
			private int _index;

			public ForwardEnumerator(Segment[] segments, int segmentCount) {
				_segments = segments;
				_segmentCount = segmentCount;
				_segmentIndex = 0;
				_index = -1;
			}

			public T Current => _segments[_segmentIndex]._items[_index];

			object IEnumerator.Current => Current!;

			public bool MoveNext() {
				while (true) {
					if (_segmentCount == _segmentIndex) {
						return false;
					}

					var segment = _segments[_segmentIndex];

					_index++;

					if (segment._count <= _index) {
						_segmentIndex++;
						_index = -1;

						continue;
					}

					return true;
				}
			}

			public void Reset() {
				_segmentIndex = -1;
				_index = -1;
			}

			public void Dispose() {

			}
		}

		private struct ReverseEnumerator : IEnumerator<T> {
			private readonly Segment[] _segments;
			private readonly int _segmentCount;
			private int _segmentIndex;
			private int _index;

			public ReverseEnumerator(Segment[] segments, int segmentCount) {
				_segments = segments;
				_segmentCount = segmentCount;
				_segmentIndex = segmentCount - 1;
				_index = _segments[_segmentIndex]._count;
			}

			public T Current => _segments[_segmentIndex]._items[_index];

			object IEnumerator.Current => Current!;

			public bool MoveNext() {
				while (true) {
					if (_segmentIndex == -1) {
						return false;
					}

					_index--;

					if (_index == -1) {
						_segmentIndex--;

						if (_segmentIndex == -1) {
							return false;
						}

						_segmentIndex = _segments[_segmentIndex]._count;

						continue;
					}

					return true;
				}
			}

			public void Reset() {
				throw new NotImplementedException();
			}

			public void Dispose() {
				throw new NotImplementedException();
			}
		}
	}
}

namespace Quilt.Collections {
	using System;
	using System.Buffers;
	using System.Collections;
	using System.Collections.Generic;

	public class PooledArray<T> : IEnumerable<T>, IReverseEnumerable<T> {
		private const int SEGMENT_SHIFT = 10;
		private const int SEGMENT_LENGTH = 1 << SEGMENT_SHIFT;
		private const int SEGMENT_MASK = SEGMENT_LENGTH - 1;

		private static readonly ArrayPool<T> __arrayPool = ArrayPool<T>.Create();

		private readonly List<Segment> _segments = new List<Segment>();

		public int Length {
			get {
				return (_segments.Count - 1) * SEGMENT_LENGTH + _segments[_segments.Count - 1]._count;
			}
			set {
				var newSegmentCount = (value >> SEGMENT_SHIFT) + 1;

				while (_segments.Count < newSegmentCount) {
					AddSegment();
				}

				while (_segments.Count > newSegmentCount) {
					_segments.RemoveAt(_segments.Count - 1);
				}

				var newItemCount = value & SEGMENT_MASK;

				var segment = _segments[_segments.Count - 1];

				_segments[_segments.Count - 1] = new Segment {
					_items = segment._items,
					_count = newItemCount
				};
			}
		}

		public PooledArray() {
			AddSegment();
		}

		private void AddSegment() {
			_segments.Add(new Segment {
				_items = __arrayPool.Rent(SEGMENT_LENGTH),
				_count = 0
			});
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
			return new ForwardEnumerator(_segments);
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public IEnumerator<T> GetReverseEnumerator() {
			return new ReverseEnumerator(_segments);
		}

		private struct Segment {
			public T[] _items;
			public int _count;
		}

		private struct ForwardEnumerator : IEnumerator<T> {
			private readonly List<Segment> _segments;
			private int _segmentIndex;
			private int _index;

			public ForwardEnumerator(List<Segment> segments) {
				_segments = segments;
				_segmentIndex = 0;
				_index = -1;
			}

			public T Current => _segments[_segmentIndex]._items[_index];

			object IEnumerator.Current => Current!;

			public bool MoveNext() {
				while (true) {
					if (_segments.Count == _segmentIndex) {
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
			private readonly List<Segment> _segments;
			private int _segmentIndex;
			private int _index;

			public ReverseEnumerator(List<Segment> segments) {
				_segments = segments;
				_segmentIndex = _segments.Count - 1;
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

namespace Quilt.VG {
  using System;
  using System.Buffers;
	using System.Collections;
	using System.Collections.Generic;

  public class CommandList : IEnumerable<Command> {
		private const int SEGMENT_SIZE = 1024;

		private static readonly ArrayPool<Command> __arrayPool = ArrayPool<Command>.Create();

		private readonly List<Segment> _segments = new List<Segment>();

		private Segment LastSegment {
			get {
				return _segments[_segments.Count - 1];
			}
		}

		public void Append(Command command) {
			if(_segments.Count == 0 || LastSegment.IsFull) {
				_segments.Add(new Segment(__arrayPool.Rent(SEGMENT_SIZE)));
			}

			var segment = LastSegment;

			segment.Append(command);
		}

		public void Clear() {
			foreach(var segment in _segments) {
				__arrayPool.Return(segment._commands);
			}

			_segments.Clear();
		}

		public IEnumerator<Command> GetEnumerator() {
			return new Enumerator(_segments);
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		private class Segment {
			internal readonly Command[] _commands;
			internal int _commandCount;

			public Segment(Command[] commands) {
				_commands = commands;
				_commandCount = 0;
			}

			public bool IsFull {
				get {
					return _commandCount == _commands.Length;
				}
			}
			public void Append(Command command) {
				_commands[_commandCount++] = command;
			}
		}

		// We use a struct enumerator so that callers can take a copy and 'fork' enumeration
		private struct Enumerator : IEnumerator<Command> {
			private readonly List<Segment> _segments;
			private int _segmentIndex;
			private int _index;

			public Enumerator(List<Segment> segments) {
				_segments = segments;
				_segmentIndex = -1;
				_index = -1;
			}

			public Command Current {
				get {
					return _segments[_segmentIndex]?._commands[_index] ?? throw new InvalidOperationException();
				}
			}

			object? IEnumerator.Current {
				get {
					return Current;
				}
			}

			public bool MoveNext() {
				if(_segmentIndex == -1) {
					_segmentIndex++;
				}

				while (true) {
					if (_segmentIndex >= _segments.Count) {
						return false;
					}

					_index++;

					if (_index >= _segments[_segmentIndex]._commandCount) {
						_segmentIndex++;

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
	}
}

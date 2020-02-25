namespace Quilt.Collections {
	public class PooledStack<T> {
		private readonly PooledArray<T> _array = new PooledArray<T>();

		public bool IsEmpty => _array.Length == 0;

		public void Push(T item) {
			_array[_array.Length++] = item;
		}

		public ref T Peek() {
			return ref _array[_array.Length];
		}

		public T Pop() {
			return _array[--_array.Length];
		}
	}
}

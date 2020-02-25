namespace Quilt.Collections {
	using System;
	using System.Collections;
	using System.Collections.Generic;

	public class PooledList<T> : IList<T> {
		private static readonly EqualityComparer<T> __comparer = EqualityComparer<T>.Default;

		private readonly PooledArray<T> _items = new PooledArray<T>();

		public T this[int index] {
			get => _items[index];
			set => _items[index] = value;
		}

		public int Count => _items.Length;

		public bool IsReadOnly => false;

		public void Add(T item) {
			_items[_items.Length++] = item;
		}

		public void Clear() {
			_items.Length = 0;
		}

		public bool Contains(T item) {
			return IndexOf(item) != -1;
		}

		public void CopyTo(T[] array, int arrayIndex) {
			throw new NotImplementedException();
		}

		public IEnumerator<T> GetEnumerator() {
			return _items.GetEnumerator();
		}

		public int IndexOf(T item) {
			for (var i = 0; i < _items.Length; i++) {
				if (__comparer.Equals(item, _items[i])) {
					return i;
				}
			}

			return -1;
		}

		public void Insert(int index, T item) {
			throw new NotImplementedException();
		}

		public bool Remove(T item) {
			throw new NotImplementedException();
		}

		public void RemoveAt(int index) {
			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
}

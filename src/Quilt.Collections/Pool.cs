namespace Quilt.Collections {
	public class Pool<T> where T : new() {
		private readonly PooledStack<T> _freeStack = new PooledStack<T>();

		public T Rent() {
			if (_freeStack.IsEmpty) {
				return new T();
			}

			return _freeStack.Pop();
		}

		public void Return(T item) {
			_freeStack.Push(item);
		}
	}
}

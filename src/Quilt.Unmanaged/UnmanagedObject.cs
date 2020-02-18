namespace Quilt.Unmanaged {
	using System;
	using System.Threading;

	public abstract class UnmanagedObject : IDisposable {
		internal const string LOAD_SYMBOL_NAME = nameof(LoadSymbol);
		internal const string LOAD_DELEGATES_NAME = nameof(LoadDelegates);

		protected readonly UnmanagedLibrary _library;

		protected UnmanagedObject(UnmanagedLibrary library) {
			_library = library;

			LoadDelegates();
		}

		protected IntPtr LoadSymbol(string name) {
			return _library.LoadSymbol(name);
		}

		protected abstract void LoadDelegates();

		#region IDisposable Support
		private int _disposed = 0; // To detect redundant calls

		protected abstract void DisposeUnmanaged();

		protected virtual void Dispose(bool disposing) {
			if (Interlocked.Increment(ref _disposed) == 1) {
				if (disposing) {
					// TODO: dispose managed state (managed objects).
				}

				DisposeUnmanaged();
			}
		}

		~UnmanagedObject() {
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(false);
		}

		// This code added to correctly implement the disposable pattern.
		public void Dispose() {
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);

			GC.SuppressFinalize(this);
		}
		#endregion
	}
}

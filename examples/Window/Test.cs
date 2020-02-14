namespace Window {
  using System;
  using System.Runtime.InteropServices;
  using Quilt.Unmanaged;

	public class Test {
		[return: MarshalAs(UnmanagedType.LPUTF8Str)]
		private delegate string GetVersionDelegate();

		private readonly UnmanagedLibrary _library;
		private readonly Func<string> _getVersionDelegate;

		public Test(UnmanagedLibrary library) {
			_library = library;
			_getVersionDelegate = Marshal.GetDelegateForFunctionPointer<Func<string>>(_library.GetSymbol("foo"));
		}

		public string GetVersion() {
			return _getVersionDelegate.Invoke();
		}
	}
}

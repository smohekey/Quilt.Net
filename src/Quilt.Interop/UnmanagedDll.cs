namespace Quilt.Interop {
	using System;

	public class UnmanagedDll {
		/// <summary>
		/// The name of the unmanaged dll as used in DllImport attributes.
		/// </summary>
		/// <value></value>
		public string Name { get; }

		/// <summary>
		/// The path the unmanaged dll was ultimately loaded from.
		/// </summary>
		/// <value></value>
		public string Path { get; }

		/// <summary>
		/// The handle to the unmanaged dll.
		/// </summary>
		/// <value></value>
		public IntPtr Handle { get; }

		public UnmanagedDll(string name, string path, IntPtr handle) {
			Name = name;
			Path = path;
			Handle = handle;
		}
	}
}

namespace Quilt.Unmanaged {
	using System;
	using System.Runtime.InteropServices;

	[AttributeUsage(AttributeTargets.Interface)]
	public class UnmanagedInterfaceAttribute : Attribute {
		public CallingConvention CallingConvention { get; set; } = CallingConvention.StdCall;

		public CharSet CharSet { get; set; } = CharSet.Ansi;

		public bool SetLastError { get; set; } = false;

		public string Prefix { get; set; } = "";
	}
}

namespace Quilt.Unmanaged {
	using System;
	using System.Runtime.InteropServices;

	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
	public class UnmanagedMethodAttribute : Attribute {
		public string? Prefix { get; set; }
		public string? Name { get; set; }
		public CallingConvention? CallingConvention { get; set; }

		public CharSet? CharSet { get; set; } = null;

		public bool? SetLastError { get; set; } = null;
	}
}

namespace Quilt.Interop {
	using System;
  using System.Runtime.InteropServices;

  [AttributeUsage(AttributeTargets.Method)]
  public class UnmanagedImportAttribute : Attribute {
		public CallingConvention? CallingConvention { get; set; } = null;

		public CharSet? CharSet { get; set; } = null;
		public string? Prefix { get; set; } = null;

		public string? EntryPoint { get; set; } = null;
	}
}

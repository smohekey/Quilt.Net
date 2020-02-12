namespace Quilt.Unmanaged {
	using System;
	using System.Runtime.InteropServices;

	[AttributeUsage(AttributeTargets.Class)]
	public class UnmanagedLibraryAttribute : Attribute {
		public string Name { get; }

		public string[] Aliases { get; }

		public CallingConvention CallingConvention { get; set; } = CallingConvention.StdCall;

		public string Prefix { get; set; } = "";

		public UnmanagedLibraryAttribute(string name, params string[] aliases) {
			Name = name;
			Aliases = aliases;
		}
	}
}

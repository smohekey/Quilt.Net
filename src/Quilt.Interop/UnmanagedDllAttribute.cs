namespace Quilt.Interop {
	using System;
  using System.Runtime.InteropServices;

  [AttributeUsage(AttributeTargets.Interface)]
	public class UnmanagedInterface : Attribute {
		public string Name { get; }

		public string[] Aliases { get; }

		public CallingConvention CallingConvention { get; set; } = CallingConvention.StdCall;

		public CharSet CharSet { get; set; } = CharSet.Ansi;

		public string Prefix { get; set; } = "";

		public UnmanagedInterface(string name, params string[] aliases) {
			Name = name;
			Aliases = aliases;
		}
	}
}

namespace Quilt.Interop {
	using System;
  using System.Runtime.InteropServices;

  [AttributeUsage(AttributeTargets.Class)]
	public class UnmanagedDllAttribute : Attribute {
		public string Name { get; }

		public string[] Aliases { get; }

		public CallingConvention CallingConvention { get; set; } = CallingConvention.StdCall;

		public CharSet CharSet { get; set; } = CharSet.Ansi;

		public string Prefix { get; set; } = "";

		public UnmanagedDllAttribute(string name, params string[] aliases) {
			Name = name;
			Aliases = aliases;
		}
	}
}

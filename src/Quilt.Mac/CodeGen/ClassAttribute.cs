namespace Quilt.Mac.CodeGen {
	using System;

	[AttributeUsage(AttributeTargets.Class)]
	public sealed class ClassAttribute : Attribute {
		public string? Name { get; set; }
	}
}

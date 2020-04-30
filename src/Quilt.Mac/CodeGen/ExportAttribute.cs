namespace Quilt.Mac.CodeGen {
	using System;

	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
	public sealed class ExportAttribute : Attribute {
		public string? Name { get; set; }
	}
}

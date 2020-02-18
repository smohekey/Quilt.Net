namespace Quilt {
	using System;

	public class QuiltAttributeAttribute : Attribute {
		public string? LocalName { get; set; }

		public QuiltAttributeAttribute(string? localName = null) {
			LocalName = localName;
		}
	}
}

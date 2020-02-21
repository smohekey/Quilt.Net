namespace Quilt.Xml {
	using System;

	public class QuiltElementAttribute : Attribute {
		public string NamespaceURI { get; set; }
		public string? LocalName { get; set; }

		public QuiltElementAttribute(string namespaceUri, string? localName = null) {
			NamespaceURI = namespaceUri;
			LocalName = localName;
		}
	}
}

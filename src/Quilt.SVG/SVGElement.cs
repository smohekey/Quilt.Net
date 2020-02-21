namespace Quilt.SVG {
	using System.Xml;
	using System;

	public abstract class SVGElement : XmlElement {
		public SVGSVGElement OwnerElement {
			get {
				throw new NotImplementedException();
			}
		}

		public SVGElement ViewportElement {
			get {
				throw new NotImplementedException();
			}
		}

		protected SVGElement(string prefix, string localName, string namespaceURI, QuiltDocument document) : base(prefix, localName, namespaceURI, document) {

		}
	}
}

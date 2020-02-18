using System.Xml.Serialization;
namespace Quilt {
	using System;
	using System.Collections.Generic;
	using System.Xml;
	using System.Xml.Serialization;

	internal class ElementLoader<T> where T : Element {
		private static readonly Dictionary<string, Namespace> __namespaces;

		static ElementLoader() {
			__namespaces = Namespace.GetNamespaces();
		}

		public T Load(XmlReader reader) {
			var elements = new Stack<Element>();

			do {
				switch (reader.NodeType) {
					case XmlNodeType.Element: {
						if (!__namespaces.TryGetValue(reader.NamespaceURI, out var @namespace)) {
							throw new Exception();
						}

						if (!@namespace.TryCreateElement(reader.LocalName, Application.Instance, out var element)) {
							throw new Exception();
						}

						elements.Push(element);

						if (reader.MoveToFirstAttribute()) {
							do {
								//reader.Value;
							} while (reader.MoveToNextAttribute());

							reader.MoveToElement();
						}

						break;
					}

					case XmlNodeType.EndElement: {
						elements.Pop();

						break;
					}

					case XmlNodeType.Document: {

						break;
					}
				}
			} while (reader.Read());

			throw new Exception();
		}
	}
}

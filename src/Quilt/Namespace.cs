using System.Xml.Serialization;
namespace Quilt {
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Xml;
	using System.Xml.Schema;

	class Namespace {
		public const string URI = "http://schema.insightful.co.nz/quilt";

		private delegate Element CreateElementDelegate(Application application);

		private static readonly Type __applicationType = typeof(Application);
		private static readonly Type __namespaceType = typeof(Namespace);
		private static readonly Type __elementAttributeType = typeof(QuiltElementAttribute);
		private static readonly Type __attributeAttributeType = typeof(QuiltAttributeAttribute);
		private static readonly Type __xmlSerializerType = typeof(XmlSerializer);
		private static readonly ElementGenerator __generator = new ElementGenerator();

		public static Dictionary<string, Namespace> GetNamespaces() {
			var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes());
			var namespaces = new Dictionary<string, Namespace>();

			// scan over the types once to find all Namespace types
			foreach (var type in types) {
				if (__namespaceType.IsAssignableFrom(type)) {
					var @namespace = (Namespace)Activator.CreateInstance(type, true)!;

					namespaces.Add(@namespace.Uri, @namespace);
				}
			}

			// scan over the types again to distribute Element types to Namespace instances
			foreach (var type in types) {
				if (type.GetCustomAttributes(__elementAttributeType, false).FirstOrDefault() is QuiltElementAttribute elementAttribute) {
					if (!namespaces.TryGetValue(elementAttribute.NamespaceURI, out var @namespace)) {
						// TODO: log error

						continue;
					}

					var concreteType = type;

					if (type.IsAbstract) {
						concreteType = GenerateConcreteType(type);
					}

					var elementInfo = new ElementInfo(type, CreateElementLambda(concreteType));

					@namespace._elementTypes.Add(elementAttribute.LocalName ?? type.Name, elementInfo);

					foreach (PropertyInfo property in type.GetProperties()) {
						if (property.GetCustomAttributes(__attributeAttributeType, false).FirstOrDefault() is QuiltAttributeAttribute attributeAttribute) {
							elementInfo.Overrides.Add(type, attributeAttribute.LocalName ?? property.Name, new XmlAttributes() {
								//XmlElements = { new XmlElementAttribute(__xmlSerializerType.MakeGenericType(property.PropertyType)) }
							});

							//elementInfo.AttributeInfos.Add(attributeAttribute.LocalName ?? property.Name, new AttributeInfo(CreateAttributeLambda(type, property)));
						}
					}
				}
			}

			return namespaces;
		}

		private static CreateElementDelegate CreateElementLambda(Type type) {
			ConstructorInfo? constructor = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { __applicationType }, null);

			var document = Expression.Parameter(__applicationType, "application");

			var parameters = new ParameterExpression[] {
				document
			};

			return Expression.Lambda<CreateElementDelegate>(Expression.New(constructor, parameters), parameters).Compile();
		}

		/*public bool TryCreateAttribute(QuiltElement element, string prefix, string localName, string namespaceURI, QuiltDocument document, [NotNullWhen(true)] out QuiltAttribute? attribute) {
			if (!_elementTypes.TryGetValue(element.LocalName, out var elementInfo)) {
				attribute = null;

				return false;
			}

			if (!elementInfo.AttributeInfos.TryGetValue(localName, out var attributeInfo)) {
				attribute = null;

				return false;
			}

			attribute = attributeInfo.Create(prefix, localName, namespaceURI, document);

			return true;
		}*/

		/*private static Func<string, string, string, QuiltDocument, QuiltAttribute> CreateAttributeLambda(Type elementType, PropertyInfo property) {
			ConstructorInfo? constructor = __quiltAttributeType.MakeGenericType(elementType, property.PropertyType).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { __stringType, __stringType, __stringType, __quiltDocumentType }, null);

			var prefix = Expression.Parameter(__stringType, "prefix");
			var localName = Expression.Parameter(__stringType, "localName");
			var namespaceURI = Expression.Parameter(__stringType, "namespaceURI");
			var document = Expression.Parameter(__quiltDocumentType, "document");

			var parameters = new ParameterExpression[] {
				prefix,
				localName,
				namespaceURI,
				document
			};

			return Expression.Lambda<Func<string, string, string, QuiltDocument, QuiltAttribute>>(Expression.New(constructor, parameters), parameters).Compile();
		}*/

		private static Type GenerateConcreteType(Type type) {
			return __generator.Generate(type);
		}

		private readonly Dictionary<string, ElementInfo> _elementTypes = new Dictionary<string, ElementInfo>();

		public string Uri { get; }

		protected Namespace(string uri) {
			Uri = uri;
		}

		private Namespace() : this(URI) {

		}

		internal bool TryCreateElement(string name, Application application, [NotNullWhen(true)] out Element? element) {
			if (!_elementTypes.TryGetValue(name, out var elementInfo)) {
				element = null;

				return false;
			}

			element = elementInfo.Create(application);

			return true;
		}

		private class ElementInfo {
			public Type Type { get; }
			public CreateElementDelegate Create { get; }

			public XmlAttributeOverrides Overrides { get; } = new XmlAttributeOverrides();

			public ElementInfo(Type type, CreateElementDelegate create) {
				Type = type;
				Create = create;
			}
		}
	}

	/*public class DerivedSerializer<T> : IXmlSerializable {
		#region Constructors
		public DerivedSerializer() { }

		public DerivedSerializer(T derived) {
			_derived = derived;
		}
		#endregion Constructors

		#region Properties
		public T Derived {
			get { return _derived; }
		}
		private T _derived;
		#endregion Properties

		#region IXmlSerializable Implementation
		public XmlSchema GetSchema() {
			return null!;
		}

		public void ReadXml(XmlReader reader) {
			Type type = Type.GetType(reader.GetAttribute("type"));
			reader.ReadStartElement();
			this._derived = (T)new
										XmlSerializer(type).Deserialize(reader);
			reader.ReadEndElement();
		}

		public void WriteXml(XmlWriter writer) {
			writer.WriteAttributeString("type", parameters.GetType().ToString());
			new XmlSerializer(_derived.GetType()).Serialize(writer, _derived, Program.NoNamespace);
		}
		#endregion IXmlSerializable Implementation

		public static implicit operator DerivedSerializer<T>(T p) {
			return p == null ? null! : new DerivedSerializer<T>(p);
		}

		public static implicit operator T(DerivedSerializer<T> p) {
			if (p == null) {
				return default!;
			} else {
				return p.Derived;
			}
		}
	}*/
}

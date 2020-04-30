namespace Quilt.Mac.ObjectiveC {
  using System;
  using System.Runtime.InteropServices;
  using System.Text;
  using Quilt.Mac.Foundation;

  public class Property : NSObject {
		private readonly Class _class;

		internal Property(Class @class, IntPtr handle) : base(handle) {
			_class = @class;
		}

		[DllImport(Runtime.LIBRARY)]
		private static extern IntPtr property_getName(Property property);

		public string Name {
			get {
				return Marshal.PtrToStringUTF8(property_getName(this));
			}
		}

		[DllImport(Runtime.LIBRARY)]
		private static extern unsafe IntPtr property_copyAttributeValue(Property property, byte* name);

		public unsafe Method? Getter {
			get {
				const string attributeName = "G";

				var attributeNameLengthBytes = Encoding.UTF8.GetMaxByteCount(attributeName.Length);

				byte* utf8AttributeNamePtr = stackalloc byte[attributeNameLengthBytes];

				fixed (char* attributeNamePtr = attributeName) {
					Encoding.UTF8.GetBytes(attributeNamePtr, attributeName.Length, utf8AttributeNamePtr, attributeNameLengthBytes);
				}

				var getterName = Marshal.PtrToStringUTF8(property_copyAttributeValue(this, utf8AttributeNamePtr));

				var selector = new Selector(getterName ?? Name);

				return _class.GetInstanceMethod(selector);
			}
		}

		public unsafe Method? Setter {
			get {
				const string attributeName = "S";

				var attributeNameLengthBytes = Encoding.UTF8.GetMaxByteCount(attributeName.Length);

				byte* utf8AttributeNamePtr = stackalloc byte[attributeNameLengthBytes];

				fixed (char* attributeNamePtr = attributeName) {
					Encoding.UTF8.GetBytes(attributeNamePtr, attributeName.Length, utf8AttributeNamePtr, attributeNameLengthBytes);
				}

				var setterName = Marshal.PtrToStringUTF8(property_copyAttributeValue(this, utf8AttributeNamePtr));

				var propertyName = Name;

				setterName ??= $"set{char.ToUpper(attributeName[0])}{propertyName.Substring(1)}";

				var selector = new Selector(setterName);

				return _class.GetInstanceMethod(selector);
			}
		}
	}
}

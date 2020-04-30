namespace Quilt.Mac.CodeGen {
  using System;
  using System.Collections.Generic;
  using System.Diagnostics.CodeAnalysis;
  using System.Reflection;
  using System.Runtime.InteropServices;
  using System.Text;
  using Quilt.Mac.Foundation;
  using Quilt.Mac.ObjectiveC;

  [SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
	public static class Types {
		public static readonly Type Object = typeof(object);
		public static readonly Type NativeInt = typeof(void*);
		public static readonly Type IntPtr = typeof(IntPtr);
		public static readonly Type Void = typeof(void);
		public static readonly Type Bool = typeof(bool);
		public static readonly Type Int8 = typeof(sbyte);
		public static readonly Type Int16 = typeof(short);
		public static readonly Type Int32 = typeof(int);
		public static readonly Type Int64 = typeof(long);
		public static readonly Type UInt8 = typeof(byte);
		public static readonly Type UInt16 = typeof(ushort);
		public static readonly Type UInt32 = typeof(uint);
		public static readonly Type UInt64 = typeof(ulong);
		public static readonly Type Single = typeof(float);
		public static readonly Type Double = typeof(double);
		
		public static readonly Type ByteArray = typeof(byte[]);
		public static readonly Type BytePtr = typeof(byte*);
		public static readonly Type String = typeof(string);

		public static readonly Type GCHandle = typeof(GCHandle);
		public static readonly Type Marshal = typeof(Marshal);

		public static readonly Type SafeHandle = typeof(SafeHandle);

		public static readonly Type Encoding = typeof(Encoding);

		public static readonly Type Runtime = typeof(Runtime);
		public static readonly Type Class = typeof(Class);
		public static readonly Type Selector = typeof(Selector);
		public static readonly Type SelectorArray = typeof(Selector[]);
		public static readonly Type ObjectRefList = typeof(ObjectRefList);

		public static readonly Type GenerationContext = typeof(GenerationContext);
		public static readonly Type ParameterInfo = typeof(ParameterInfo);
		public static readonly Type ListGeneric = typeof(List<>);
		public static readonly Type TypeMarshaler = typeof(TypeMarshaler);

		public static readonly Type NSObject = typeof(NSObject);
		public static readonly Type NSObjectGeneric1 = typeof(NSObject<>);
		public static readonly Type NSObjectGeneric2 = typeof(NSObject<,>);
		public static readonly Type NSObjectGeneric1MetaClass = typeof(NSObject<>.MetaClass);
	}
}

namespace Quilt.Mac.ObjectiveC {
	using System;
	using System.Collections.Generic;
  using System.IO;
  using System.Reflection;
  using System.Runtime.InteropServices;
	using System.Text;
  using Quilt.Mac.CodeGen;
  using Quilt.Mac.Foundation;

  public class Class : SafeHandle {
		protected Class() : base(IntPtr.Zero, false) {

		}

		public Class(IntPtr handle) : this() {
			SetHandle(handle);
		}

		[DllImport(Runtime.LIBRARY)]
		private static extern IntPtr class_getName(Class @class);

		public string Name {
			get {
				return Marshal.PtrToStringUTF8(class_getName(this));
			}
		}

		[DllImport(Runtime.LIBRARY)]
		private static extern Class class_getSuperclass(Class @class);

		public Class? Superclass {
			get {
				var superClass = class_getSuperclass(this);

				return superClass;
			}
		}

		[DllImport(Runtime.LIBRARY)]
		private static extern bool class_isMetaClass(Class @class);

		public bool IsMetaClass {
			get {
				return class_isMetaClass(this);
			}
		}

		[DllImport(Runtime.LIBRARY)]
		private static extern int class_getInstanceSize(Class @class);

		public int InstanceSize {
			get {
				return class_getInstanceSize(this);
			}
		}

		[DllImport(Runtime.LIBRARY)]
		private static extern unsafe IntPtr class_getInstanceVariable(Class @class, byte* name);

		public unsafe InstanceVariable? GetInstanceVariable(string name) {
			var nameLengthBytes = Encoding.UTF8.GetMaxByteCount(name.Length);
			var utf8NamePtr = stackalloc byte[nameLengthBytes];

			fixed (char* namePtr = name) {
				Encoding.UTF8.GetBytes(namePtr, name.Length, utf8NamePtr, nameLengthBytes);
			}

			var ivar = class_getInstanceVariable(this, utf8NamePtr);

			return ivar == IntPtr.Zero ? null : new InstanceVariable(this, ivar);
		}

		/*public unsafe InstanceVariable? GetInstanceVariable(string name) {
			return class_getInstanceVariable(this, Encoding.UTF8.GetBytes(name));
		}*/

		[DllImport(Runtime.LIBRARY)]
		private static extern unsafe IntPtr class_getClassVariable(Class @class, byte* name);

		public unsafe InstanceVariable? GetClassVariable(string name) {
			if (name is null) {
				throw new ArgumentNullException(nameof(name));
			}

			var nameLengthBytes = Encoding.UTF8.GetMaxByteCount(name.Length);
			var utf8NamePtr = stackalloc byte[nameLengthBytes];

			fixed (char* namePtr = name) {
				Encoding.UTF8.GetBytes(namePtr, name.Length, utf8NamePtr, nameLengthBytes);
			}

			var ivarPtr = class_getClassVariable(this, utf8NamePtr);

			return ivarPtr == IntPtr.Zero ? null : new InstanceVariable(this, ivarPtr);
		}

		[DllImport(Runtime.LIBRARY)]
		private static extern unsafe bool class_addIvar(Class @class, byte* name, IntPtr size, byte alignment, byte* types);

		public unsafe bool AddInstanceVariable<T>(string name) where T : struct {
			if(name is null) {
				throw new ArgumentNullException(nameof(name));
			}

			var type = typeof(T);

			var nameLengthBytes = Encoding.UTF8.GetMaxByteCount(name.Length);
			var utf8NamePtr = stackalloc byte[nameLengthBytes];

			fixed(char* namePtr = name) {
				Encoding.UTF8.GetBytes(namePtr, name.Length, utf8NamePtr, nameLengthBytes);
			}

			var size = Marshal.SizeOf(type);

			var builder = new StringBuilder();

			TypeEncodingForType(builder, type);

			var types = builder.ToString();
			var typesLengthBytes = Encoding.UTF8.GetMaxByteCount(types.Length);
			var utf8TypesPtr = stackalloc byte[typesLengthBytes];

			fixed(char* typesPtr = types) {
				Encoding.UTF8.GetBytes(typesPtr, types.Length, utf8TypesPtr, typesLengthBytes);
			}

			return class_addIvar(this, utf8NamePtr, (IntPtr)size, (byte)Math.Log(size, 2), utf8TypesPtr);
		}

		[DllImport(Runtime.LIBRARY)]
		private static extern IntPtr class_getInstanceMethod(Class @class, Selector name);

		public Method? GetInstanceMethod(Selector selector) {
			var methodPtr = class_getInstanceMethod(this, selector);

			return methodPtr == IntPtr.Zero ? null : new Method(this, methodPtr);
		}

		[DllImport(Runtime.LIBRARY)]
		private static extern IntPtr class_getClassMethod(Class @class, Selector selector);

		public Method? GetClassMethod(Selector selector) {
			var methodPtr = class_getClassMethod(this, selector);

			return methodPtr == IntPtr.Zero ? null : new Method(this, methodPtr);
		}

		[DllImport(Runtime.LIBRARY)]
		private static extern unsafe IntPtr* class_copyMethodList(Class @class, out uint count);

		public unsafe IEnumerable<Method> Methods {
			get {
				var methodPtr = class_copyMethodList(this, out var count);

				var methods = new Method[count];

				for (var i = 0; i < count; i++) {
					methods[i] = new Method(this, methodPtr[i]);
				}

				Runtime.Free((IntPtr)methodPtr);

				return methods;
			}
		}

		public override bool IsInvalid => handle == IntPtr.Zero;

		[DllImport(Runtime.LIBRARY)]
		private static extern unsafe IntPtr class_getProperty(Class @class, byte* name);

		public unsafe Property? GetProperty(string name) {
			if (name is null) {
				throw new ArgumentNullException(nameof(name));
			}

			var nameLengthBytes = Encoding.UTF8.GetMaxByteCount(name.Length);
			var utf8Name = stackalloc byte[nameLengthBytes];

			fixed(char* namePtr = name) {
				Encoding.UTF8.GetBytes(namePtr, name.Length, utf8Name, nameLengthBytes);
			}

			var propertyPtr = class_getProperty(this, utf8Name);

			return propertyPtr == IntPtr.Zero ? null : new Property(this, propertyPtr);
		}

		[DllImport(Runtime.LIBRARY)]
		private static extern unsafe IntPtr* class_copyPropertyList(Class @class, out uint count);

		public unsafe IEnumerable<Property> Properties {
			get {
				var propertyPtr = class_copyPropertyList(this, out var count);

				var properties = new Property[count];

				for (var i = 0; i < count; i++) {
					properties[i] = new Property(this, propertyPtr[i]);
				}

				Runtime.Free((IntPtr)propertyPtr);

				return properties;
			}
		}

		protected override bool ReleaseHandle() {
			return true;
		}

		[DllImport(Runtime.LIBRARY)]
		private static extern unsafe bool class_addMethod(Class @class, Selector selector, IntPtr imp, byte* types);

		public unsafe bool AddMethod<TFunc>(Selector selector, TFunc imp) where TFunc : MulticastDelegate {
			if(imp is null) {
				throw new ArgumentNullException(nameof(imp));
			}

			var builder = new StringBuilder();

			TypeEncodingForFunc(builder, imp);

			var typeEncoding = builder.ToString();
			var utf8Length = Encoding.UTF8.GetMaxByteCount(typeEncoding.Length);
			var utf8 = stackalloc byte[utf8Length];

			fixed (char* typeEncodingPtr = typeEncoding) {
				Encoding.UTF8.GetBytes(typeEncodingPtr, typeEncoding.Length, utf8, utf8Length);
			}

			return class_addMethod(this, selector, Marshal.GetFunctionPointerForDelegate(imp), utf8);
		}

		private static void TypeEncodingForFunc<TFunc>(StringBuilder builder, TFunc imp) where TFunc : MulticastDelegate {
			var method = imp.Method;

			TypeEncodingForParameter(builder, method.ReturnParameter);

			foreach(var parameter in imp.Method.GetParameters()) {
				TypeEncodingForParameter(builder, parameter);
			}
		}

		private static void TypeEncodingForParameter(StringBuilder builder, ParameterInfo parameter) {
			var type = parameter.ParameterType;

			if (parameter.IsOut) {
				builder.Append('o');
			}
		}
		private static void TypeEncodingForType(StringBuilder builder, Type type) {
			if(type.IsByRef) {
				builder.Append('^');
			}

			if (type.IsValueType) {
				if (type == Types.Int8) {
					builder.Append('c');
				} else if (type == Types.Int16) {
					builder.Append('s');
				} else if (type == Types.Int32) {
					builder.Append('i');
				} else if (type == Types.Int64) {
					builder.Append('q');
				} else if (type == Types.UInt8) {
					builder.Append('C');
				} else if (type == Types.UInt16) {
					builder.Append('S');
				} else if (type == Types.UInt32) {
					builder.Append('I');
				} else if (type == Types.UInt64) {
					builder.Append('Q');
				} else if (type == Types.Single) {
					builder.Append('f');
				} else if (type == Types.Double) {
					builder.Append('d');
				} else if (type == Types.Bool) {
					builder.Append('B');
				} else if (type == Types.Void) {
					builder.Append('v');
				} else {
					throw new NotSupportedException($"Type {type.FullName} not supported as an exported method parameter type.");
				}
			} else if (type == Types.String) {
				builder.Append('*');
			} else if (type == Types.NSObject) {
				builder.Append('@');
			} else if (type == Types.Class) {
				builder.Append('#');
			} else if (type == Types.Selector) {
				builder.Append(':');
			} else {
				throw new NotSupportedException($"Type {type.FullName} not supported as an exported method parameter type.");
			}
		}
	}
}

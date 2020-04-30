namespace Quilt.Mac.ObjectiveC {
	using System;
  using System.Collections.Generic;
  using System.Runtime.InteropServices;
  using Quilt.Mac.Foundation;

  public class Method : NSObject {
		private readonly Class _class;

		internal Method(Class @class, IntPtr handle) : base(handle) {
			_class = @class;
		}

		[DllImport(Runtime.LIBRARY)]
		private static extern Selector method_getName(Method method);

		public string Name {
			get {
				return method_getName(this).Name;
			}
		}

		[DllImport(Runtime.LIBRARY)]
		private static extern IntPtr method_getTypeEncoding(Method method);

		public string TypeEncoding {
			get {
				return Marshal.PtrToStringUTF8(method_getTypeEncoding(this));
			}
		}

		[DllImport(Runtime.LIBRARY)]
		private static extern IntPtr method_copyReturnType(Method method);

		public string ReturnTypeString {
			get {
				var ptr = method_copyReturnType(this);

				var returnType = Marshal.PtrToStringUTF8(ptr);

				Runtime.Free(ptr);

				return returnType;
			}
		}

		[DllImport(Runtime.LIBRARY)]
		private static extern uint method_getNumberOfArguments(Method method);

		public uint NumberOfArguments {
			get {
				return method_getNumberOfArguments(this);
			}
		}

		[DllImport(Runtime.LIBRARY)]
		private static extern IntPtr method_copyArgumentType(Method method, uint index);

		public IEnumerable<string> ArgumentTypeStrings {
			get {
				var argumentCount = NumberOfArguments;
				
				for(var i = 0u; i < argumentCount; i++) {
					var ptr = method_copyArgumentType(this, i);

					var result = Marshal.PtrToStringUTF8(ptr);

					Runtime.Free(ptr);

					yield return result;
				}
			}
		}
	}
}

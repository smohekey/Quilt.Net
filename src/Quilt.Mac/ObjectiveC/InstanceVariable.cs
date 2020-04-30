namespace Quilt.Mac.ObjectiveC {
  using System;
  using System.Runtime.InteropServices;
  using Quilt.Mac.Foundation;

  public class InstanceVariable : NSObject {
		private readonly Class _class;

		internal InstanceVariable(Class @class, IntPtr handle) : base(handle) {
			_class = @class;
		}

		[DllImport(Runtime.LIBRARY)]
		private static extern unsafe IntPtr ivar_getName(InstanceVariable ivar);

		public string Name {
			get {
				return Marshal.PtrToStringUTF8(ivar_getName(this));
			}
		}

		[DllImport(Runtime.LIBRARY)]
		private static extern unsafe int ivar_getOffset(InstanceVariable ivar);

		public int Offset {
			get {
				return ivar_getOffset(this);
			}
		}

		[DllImport(Runtime.LIBRARY)]
		private static extern unsafe IntPtr ivar_getTypeEncoding(InstanceVariable ivar);

		public string TypeEncoding {
			get {
				return Marshal.PtrToStringUTF8(ivar_getTypeEncoding(this));
			}
		}
	}
}

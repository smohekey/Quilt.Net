namespace Quilt.Mac.CodeGen {
	using System.Reflection;
	using System.Runtime.InteropServices;
	using System.Text;
  using Quilt.Mac.Foundation;
  using Quilt.Mac.ObjectiveC;

  public static class Methods {
		public static readonly MethodInfo Encoding_UTF8 = Types.Encoding.GetProperty(nameof(Encoding.UTF8)).GetMethod;
		public static readonly MethodInfo Encoding_GetBytes = Types.Encoding.GetMethod(nameof(Encoding.GetBytes), new[] { Types.String });
		public static readonly MethodInfo Encoding_GetString = Types.Encoding.GetMethod(nameof(Encoding.GetString), new[] { Types.BytePtr, Types.Int32 });

		public static readonly MethodInfo GCHandle_Alloc = Types.GCHandle.GetMethod(nameof(GCHandle.Alloc), new[] { Types.Object, Types.Int32 });
		public static readonly MethodInfo GCHandle_AddrOfPinnedObject = Types.GCHandle.GetMethod(nameof(GCHandle.AddrOfPinnedObject));
		public static readonly MethodInfo GCHandle_Free = Types.GCHandle.GetMethod(nameof(GCHandle.Free));

		public static readonly MethodInfo IntPtr_ExplicitCast = Types.IntPtr.GetMethod("op_Explicit", new[] { Types.NativeInt });

		public static readonly MethodInfo Marshal_PtrToStringUTF8 = Types.Marshal.GetMethod(nameof(Marshal.PtrToStringUTF8), new[] { Types.IntPtr });

		public static readonly MethodInfo NSObject_GetHandle = Types.NSObject.GetProperty(nameof(NSObject.Handle)).GetMethod;

		public static readonly MethodInfo Class_AddMethod = Types.Class.GetMethod(nameof(Class.AddMethod));

		public static readonly MethodInfo ObjectRefList_GetObject = Types.ObjectRefList.GetMethod(nameof(ObjectRefList.GetObject));
		public static readonly MethodInfo ObjectRefList_AddReference = Types.ObjectRefList.GetMethod(nameof(ObjectRefList.AddReference));
		public static readonly MethodInfo ObjectRefList_RemoveReference = Types.ObjectRefList.GetMethod(nameof(ObjectRefList.RemoveReference));

		public static readonly MethodInfo Runtime_GetClass = Types.Runtime.GetMethod(nameof(Runtime.GetClass));
	}
}

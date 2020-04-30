namespace Quilt.Mac.Foundation {
	using System;
  using Quilt.Mac.CodeGen;
  using Quilt.Mac.ObjectiveC;

	[Class]
	public abstract class NSString : NSObject<NSString, NSString.MetaClass> {
		[Import(Name = "UTF8String")]
		public abstract string UTF8String();

		public NSString(IntPtr handle) : base(handle) {

		}

		[Import]
		public unsafe abstract NSString InitWith(char* characters, int length);

		public static unsafe NSString From(string value) {
			if (value is null) {
				throw new ArgumentNullException(nameof(value));
			}

			var nsString = Alloc();

			fixed (char* ptr = value) {
				return nsString.InitWith(ptr, value.Length);
			}
		}

		public override string ToString() {
			return UTF8String();
			//return Marshal.PtrToStringUTF8(Runtime.IntPtr_MsgSend(this, "UTF8String"));
		}

		public new abstract class MetaClass : NSObject<NSString, MetaClass>.MetaClass {
			protected MetaClass(Class @class) : base(@class) {

			}
		}
	}
}

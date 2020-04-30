namespace Quilt.Mac.Foundation {
	using System;
  using Quilt.Mac.CodeGen;
  using Quilt.Mac.ObjectiveC;

	[Class]
  public abstract class NSError : NSObject<NSError, NSError.MetaClass> {
		protected NSError(IntPtr handle) : base(handle) {

		}

		public new abstract class MetaClass : NSObject<NSError, MetaClass>.MetaClass {
			protected MetaClass(Class @class) : base(@class) {

			}
		}
	}
}

namespace Quilt.Mac.Foundation {
	using System;
  using Quilt.Mac.CodeGen;
  using Quilt.Mac.ObjectiveC;

	[Class]
  public abstract class NSAutoreleasePool : NSObject<NSAutoreleasePool, NSAutoreleasePool.MetaClass> {
		protected NSAutoreleasePool(IntPtr handle) : base(handle) {

		}

		[Import]
		public abstract void Drain();

		protected override void Dispose(bool disposing) {
			Drain();

			base.Dispose(disposing);
		}

		public new abstract class MetaClass : NSObject<NSAutoreleasePool, MetaClass>.MetaClass {
			protected MetaClass(Class @class) : base(@class) {

			}
		}
	}
}

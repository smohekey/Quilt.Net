namespace Quilt.Mac.Foundation {
	using System;
	using Quilt.Mac.CodeGen;
	using Quilt.Mac.ObjectiveC;

	[Class]
	public abstract class NSThread : NSObject<NSThread, NSThread.MetaClass> {
		protected NSThread(IntPtr handle) : base(handle) {

		}

		public static void DetachNewThread(Selector selector, NSObject toTarget, NSObject withObject) => Meta.DetachNewThread(selector, toTarget, withObject);

		public new abstract class MetaClass : NSObject<NSThread, MetaClass>.MetaClass {
			protected MetaClass(Class @class) : base(@class) {

			}

			[Import]
			public abstract void DetachNewThread(Selector selector, NSObject toTarget, NSObject withObject);
		}
	}
}

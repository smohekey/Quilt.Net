namespace Quilt.Mac.AppKit {
	using System;
  using Quilt.Mac.CodeGen;
  using Quilt.Mac.Foundation;
	using Quilt.Mac.ObjectiveC;

	public abstract class NSRunningApplication : NSObject<NSRunningApplication, NSRunningApplication.MetaClass> {
		protected NSRunningApplication(IntPtr handle) : base(handle) {

		}

		public static NSRunningApplication CurrentApplication => Meta.CurrentApplication;

		[Import]
		public abstract bool FinishedLaunching { get; }

		public new abstract class MetaClass : NSObject<NSRunningApplication, MetaClass>.MetaClass {
			protected MetaClass(Class @class) : base(@class) {

			}

			[Import]
			public abstract NSRunningApplication CurrentApplication {
				get;
			}
		}
	}
}

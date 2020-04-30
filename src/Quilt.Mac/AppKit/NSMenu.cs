namespace Quilt.Mac.AppKit {
	using System;
  using Quilt.Mac.CodeGen;
  using Quilt.Mac.Foundation;
	using Quilt.Mac.ObjectiveC;

	[Class]
	public abstract class NSMenu : NSObject<NSMenu, NSMenu.MetaClass> {
		protected NSMenu(IntPtr handle) : base(handle) {

		}

		public new abstract class MetaClass : NSObject<NSMenu, MetaClass>.MetaClass {
			protected MetaClass(Class @class) : base(@class) {

			}
		}
	}
}

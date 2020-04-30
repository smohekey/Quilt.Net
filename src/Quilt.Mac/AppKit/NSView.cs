namespace Quilt.Mac.AppKit {
	using System;
	using Quilt.Mac.ObjectiveC;

	public abstract class NSView : NSResponder<NSView, NSView.MetaClass> {
		protected NSView(IntPtr handle) : base(handle) {

		}

		public new abstract class MetaClass : NSResponder<NSView, MetaClass>.MetaClass {
			protected MetaClass(Class @class) : base(@class) {

			}
		}
	}
}

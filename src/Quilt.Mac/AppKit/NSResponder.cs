namespace Quilt.Mac.AppKit {
	using System;
  using Quilt.Mac.CodeGen;
  using Quilt.Mac.Foundation;
  using Quilt.Mac.ObjectiveC;

	[Class]
	public abstract class NSResponder<TClass, TMetaClass> : NSObject<TClass, TMetaClass> 
		where TClass : NSResponder<TClass, TMetaClass>
		where TMetaClass : NSResponder<TClass, TMetaClass>.MetaClass {

		protected NSResponder(IntPtr handle) : base(handle) {

		}

		public new abstract class MetaClass : NSObject<TClass, TMetaClass>.MetaClass {
			protected MetaClass(Class @class) : base(@class) {

			}
		}
	}
}

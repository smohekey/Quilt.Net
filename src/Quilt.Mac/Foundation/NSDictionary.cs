namespace Quilt.Mac.Foundation {
	using System;
	using Quilt.Mac.CodeGen;
	using Quilt.Mac.ObjectiveC;

	[Class]
	public abstract class NSDictionary : NSObject<NSDictionary, NSDictionary.MetaClass> {
		protected NSDictionary(IntPtr handle) : base(handle) {

		}

		public new abstract class MetaClass : NSObject<NSDictionary, MetaClass>.MetaClass {
			protected MetaClass(Class @class) : base(@class) {

			}
		}
	}
}

namespace Quilt.Mac.Foundation {
	using System;
  using Quilt.Mac.CodeGen;
  using Quilt.Mac.ObjectiveC;

	[Class]
	public abstract class NSURL : NSObject<NSURL, NSURL.MetaClass> {
		[Import(Name = "fileURL")]
		public abstract bool FileURL { get; }

		[Import]
		public abstract NSString AbsoluteString {
			get;
		}

		[Import]
		public abstract NSString Path {
			get;
		}

		public NSURL(IntPtr handle) : base(handle) {

		}

		public new abstract class MetaClass : NSObject<NSURL, MetaClass>.MetaClass {
			protected MetaClass(Class @class) : base(@class) {

			}
		}
	}
}

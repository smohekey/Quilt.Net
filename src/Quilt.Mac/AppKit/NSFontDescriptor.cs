namespace Quilt.Mac.AppKit {
	using System;
  using Quilt.Mac.CodeGen;
  using Quilt.Mac.Foundation;
	using Quilt.Mac.ObjectiveC;

	[Class]
	public abstract class NSFontDescriptor : NSObject<NSFontDescriptor, NSFontDescriptor.MetaClass> {
		private const string kCTFontURLAttribute = "NSCTFontFileURLAttribute";

		//private readonly 
		public NSFontDescriptor(IntPtr handle) : base(handle) {

		}

		[Import]
		protected abstract IntPtr ObjectFor(NSString key);

		public NSURL? FontUrl {
			get {
				using var key = NSString.From(kCTFontURLAttribute);

				var ptr = ObjectFor(key);

				if (ptr != null) {
					return Generator.Instance.Create<NSURL, IntPtr>(ptr);
				}

				return null;
			}
		}

		public new abstract class MetaClass : NSObject<NSFontDescriptor, MetaClass>.MetaClass {
			protected MetaClass(Class @class) : base(@class) {

			}
		}
	}
}

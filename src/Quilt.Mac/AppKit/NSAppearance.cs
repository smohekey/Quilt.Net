namespace Quilt.Mac.AppKit {
	using System;
	using Quilt.Mac.CodeGen;
  using Quilt.Mac.Foundation;
	using Quilt.Mac.ObjectiveC;

	[Class]
	public abstract class NSAppearance : NSObject<NSAppearance, NSAppearance.MetaClass> {
		private static readonly Lazy<NSString> __aqua = new Lazy<NSString>(() => NSString.From("NSAppearanceNameAqua"));
		private static readonly Lazy<NSString> __darkAqua = new Lazy<NSString>(() => NSString.From("NSAppearanceNameDarkAqua"));

		protected NSAppearance(IntPtr handle) : base(handle) {

		}

		public static NSString Aqua => __aqua.Value;
		public static NSString DarkAqua => __darkAqua.Value;

		public static NSAppearance AppearanceNamed(NSString name) => Meta.AppearanceNamed(name);

		public new abstract class MetaClass : NSObject<NSAppearance, MetaClass>.MetaClass {
			protected MetaClass(Class @class) : base(@class) {

			}

			[Import(Name = "appearanceNamed:")]
			public abstract NSAppearance AppearanceNamed(NSString name);
		}
	}
}

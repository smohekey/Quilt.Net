namespace Quilt.Mac.Foundation {
	using System;
  using Quilt.Mac.CodeGen;
  using Quilt.Mac.ObjectiveC;

	[Class]
	public abstract class NSUserDefaults : NSObject<NSUserDefaults, NSUserDefaults.MetaClass> {
		protected NSUserDefaults(IntPtr handle) : base(handle) {

		}

		public static NSUserDefaults StandardUserDefaults => Meta.StandardUserDefaults();

		[Import(Name = "stringForKey:")]
		public abstract NSString StringForKey(NSString key);

		public new abstract class MetaClass : NSObject<NSUserDefaults, MetaClass>.MetaClass {
			protected MetaClass(Class @class) : base(@class) {

			}

			/// <summary>
			/// 
			/// </summary>
			/// <returns></returns>
			[Import]
			public abstract NSUserDefaults StandardUserDefaults(); // despite the documentation saying this is a property, it is actually a method
		}
	}
}

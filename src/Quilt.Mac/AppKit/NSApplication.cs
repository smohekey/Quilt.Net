namespace Quilt.Mac.AppKit {
	using System;
	using System.Runtime.InteropServices;
	using Quilt.Mac.CodeGen;
	using Quilt.Mac.ObjectiveC;

	[Class]
	public abstract class NSApplication : NSResponder<NSApplication, NSApplication.MetaClass> {
		private const string LIBRARY = "/System/Library/Frameworks/AppKit.framework/AppKit";
		[DllImport(LIBRARY)]
		private extern static int NSApplicationMain(int argc, string[] argv);

		public NSApplication(IntPtr handle) : base(handle) {

		}

		public static NSApplication SharedApplication => Meta.SharedApplication;

		public static void Main(string[] args) {
			NSApplicationMain(args.Length, args);
		}

		[Import]
		public abstract INSApplicationDelegate Delegate { get; }

		[Import(Name = "setDelegate:")]
		public abstract void SetDelegate(INSApplicationDelegate @delegate);

		[Import]
		public abstract void Run();

		[Import(Name = "setActivationPolicy:")]
		public abstract bool SetActivationPolicy(NSApplicationActivationPolicy activationPolicy);

		public new abstract class MetaClass : NSResponder<NSApplication, MetaClass>.MetaClass {
			[Import]
			public abstract NSApplication SharedApplication { get; }

			protected MetaClass(Class @class) : base(@class) {

			}
		}
	}
}

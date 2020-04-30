namespace Window {
	using System;
	using Quilt;
	using Quilt.GLFW;
	using Quilt.Mac.AppKit;
	using Quilt.Mac.CodeGen;
	using Quilt.Mac.Foundation;
	using Quilt.Mac.ObjectiveC;
	using Quilt.Unmanaged;

	class Program {
		static void Main(string[] args) {
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledException);

			/*var loader = UnmanagedLoader.Default;

			loader.LoadLibrary("/System/Library/Frameworks/AppKit.framework/AppKit");

			using (var pool = NSAutoreleasePool.New()) {
				var helper = Helper.New();

				NSThread.DetachNewThread(Selector.From<Helper>(h => h.DoNothing()), helper, helper);

				var app = NSApplication.SharedApplication;

				app.SetActivationPolicy(NSApplicationActivationPolicy.Regular);
				app.SetDelegate(ApplicationDelegate.New());

				var interfaceStyle = NSUserDefaults.StandardUserDefaults.StringForKey(NSString.From("AppleInterfaceStyle"));

				var window = NSWindow
				.Alloc()
				.InitWith(new NSRect(100, 100, 500, 500), NSWindowStyleMask.Titled | NSWindowStyleMask.Closable | NSWindowStyleMask.Resizable | NSWindowStyleMask.Miniaturizable, NSBackingStoreType.Buffered, false)
				.Autorelease();

				var pixelFormat = NSOpenGLPixelFormat.Alloc().InitWith(
					NSOpenGLPixelFormatAttribute.OpenGLProfile, (int)NSOpenGLProfile.Core4_1,
					NSOpenGLPixelFormatAttribute.Multisample,
					NSOpenGLPixelFormatAttribute.SampleBuffers, 1,
					NSOpenGLPixelFormatAttribute.Samples, 4
				).Autorelease();

				var glContext = NSOpenGLContext.Alloc().InitWith(pixelFormat, null).Autorelease();

				window.ContentView = glContext.View;

				window.CascadeTopLeftFromPoint(new NSPoint(100, 100));
				//window.Title = NSString.FromString("Hello There");
				window.Appearance = NSAppearance.AppearanceNamed(NSAppearance.DarkAqua);
				window.Title = interfaceStyle ?? NSString.From("Hello There");
				window.MakeKeyAndOrderFront(null);

				glContext.MakeCurrentContext();
				glContext.FlushBuffer();

				if (!NSRunningApplication.CurrentApplication.FinishedLaunching) {
					app.Run();
				}
			}*/

			var mainWindow = new Quilt.UI.Window() {
				Title = "Cadre",
				Position = (200, 200),
				Size = (2048, 2048)
			};

			mainWindow.Show();

			Application.Run();
		}

		static void UnhandledException(object sender, UnhandledExceptionEventArgs args) {
			var e = (Exception)args.ExceptionObject;

			Console.WriteLine($"Unhandled Exception: {e.Message}");
			Console.WriteLine($"Runtime terminating: {args.IsTerminating}");
		}
	}

	[Class]
	public abstract class ApplicationDelegate : NSObject<ApplicationDelegate>, INSApplicationDelegate {
		public ApplicationDelegate(IntPtr handle) : base(handle) {

		}

		public void ApplicationWillFinishLaunching(NSNotification notification) {

		}

		public void ApplicationDidFinishLaunching(NSNotification notification) {

		}

		public void ApplicationWillBecomeActive(NSNotification notification) {

		}

		public void ApplicationDidBecomeActive(NSNotification notification) {

		}

		public void ApplicationWillResignActive(NSNotification notification) {

		}

		public void ApplicationDidResignActive(NSNotification notification) {

		}

		public NSApplicationTerminateReply ApplicationShouldTerminate(NSApplication application) {
			return NSApplicationTerminateReply.Now;
		}

		public bool ApplicationShouldTerminateAfterLastWindowClosed(NSApplication application) {
			return true;
		}

		public void ApplicationWillTerminate(NSApplication application) {

		}

		public void ApplicationWillHide(NSNotification notification) {

		}

		public void ApplicationDidHide(NSNotification notification) {

		}

		public void ApplicationWillUnhide(NSNotification notification) {

		}

		public void ApplicationDidUnhide(NSNotification notification) {

		}

		public void ApplicationWillUpdate(NSNotification notification) {

		}

		public void ApplicationDidUpdate(NSNotification notification) {

		}

		public bool ApplicationShouldHandleReopen(NSApplication application, bool hasVisibleWindows) {
			return false;
		}

		public NSMenu ApplicationDockMenu(NSApplication application) {
			return null;
		}

		public NSError ApplicationWillPresentError(NSApplication application, NSError error) {
			return null;
		}

		public void ApplicationDidChangeScreenParameters(NSNotification notification) {

		}
	}

	[Class]
	public abstract class Helper : NSObject<Helper> {
		public Helper(IntPtr handle) : base(handle) {

		}

		[Export]
		public void DoNothing() {
			Console.WriteLine("New thread");
		}
	}
}

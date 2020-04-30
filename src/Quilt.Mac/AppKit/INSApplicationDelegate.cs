namespace Quilt.Mac.AppKit {
	using Quilt.Mac.CodeGen;
	using Quilt.Mac.Foundation;

	[Protocol]
	public interface INSApplicationDelegate : INSObject {
		[Export(Name = "applicationWillFinishLaunching:")]
		void ApplicationWillFinishLaunching(NSNotification notification);

		[Export(Name = "applicationDidFinishLaunching:")]
		void ApplicationDidFinishLaunching(NSNotification notification);

		[Export(Name = "applicationWillBecomeActive:")]
		void ApplicationWillBecomeActive(NSNotification notification);

		[Export(Name = "applicationDidBecomeActive:")]
		void ApplicationDidBecomeActive(NSNotification notification);

		[Export(Name = "applicationWillResignActive:")]
		void ApplicationWillResignActive(NSNotification notification);

		[Export(Name = "applicationDidResignActive:")]
		void ApplicationDidResignActive(NSNotification notification);

		[Export(Name = "applicationShouldTerminate:")]
		NSApplicationTerminateReply ApplicationShouldTerminate(NSApplication application);

		[Export(Name = "applicationShouldTerminateAfterLastWindowClosed:")]
		bool ApplicationShouldTerminateAfterLastWindowClosed(NSApplication application);

		[Export(Name = "applicationWillTerminate:")]
		void ApplicationWillTerminate(NSApplication application);

		[Export(Name = "applicationWillHide:")]
		void ApplicationWillHide(NSNotification notification);

		[Export(Name = "applicationDidHide:")]
		void ApplicationDidHide(NSNotification notification);

		[Export(Name = "applicationWillUnhide:")]
		void ApplicationWillUnhide(NSNotification notification);

		[Export(Name = "applicationDidUnhide:")]
		void ApplicationDidUnhide(NSNotification notification);

		[Export(Name = "applicationWillUpdate:")]
		void ApplicationWillUpdate(NSNotification notification);

		[Export(Name = "applicationDidUpdate:")]
		void ApplicationDidUpdate(NSNotification notification);

		[Export(Name = "applicationShouldHandleReopen:hasVisibleWindows:")]
		bool ApplicationShouldHandleReopen(NSApplication application, bool hasVisibleWindows);

		[Export(Name = "applicationDockMenu:")]
		NSMenu ApplicationDockMenu(NSApplication application);

		[Export(Name = "application:willPresentError:")]
		NSError ApplicationWillPresentError(NSApplication application, NSError error);

		[Export(Name = "applicationDidChangeScreenParameters:")]
		void ApplicationDidChangeScreenParameters(NSNotification notification);
	}
}

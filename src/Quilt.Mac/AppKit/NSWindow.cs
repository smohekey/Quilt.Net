namespace Quilt.Mac.AppKit {
	using System;
	using Quilt.Mac.CodeGen;
	using Quilt.Mac.Foundation;
	using Quilt.Mac.ObjectiveC;

	[Class]
	public abstract class NSWindow : NSResponder<NSWindow, NSWindow.MetaClass> {
		protected NSWindow(IntPtr handle) : base(handle) {

		}

		/// <summary>
		///		Initializes an allocated window with the specified values.
		/// </summary>
		/// <remarks>
		///		The primary screen is the one that contains the current key window or, if there is no key window, the one that contains the main menu. If there’s neither a key window nor a main menu (if there’s no active application), the primary screen is the one where the origin of the screen coordinate system is located.
		/// </remarks>
		/// <param name="contentRect">
		///		Origin and size of the window’s content area in screen coordinates. The origin is relative to the origin of the provided screen. Note that the window server limits window position coordinates to ±16,000 and sizes to 10,000.
		///	</param>
		/// <param name="styleMask">
		///		The window’s style. It can be NSBorderlessWindowMask, or it can contain any of the options described in <see cref=NSWindowStyleMask"/>, combined using the C bitwise OR operator. Borderless windows display none of the usual peripheral elements and are generally useful only for display or caching purposes; you should not usually need to create them. Also, note that a window’s style mask should include NSTitledWindowMask if it includes any of the others.
		/// </param>
		/// <param name="backing">
		///		Specifies how the drawing done in the window is buffered by the window device; possible values are described in <see cref="NSBackingStoreType"/>.
		/// </param>
		/// <param name="defer">
		///		Specifies whether the window server creates a window device for the window immediately. When YES, the window server defers creating the window device until the window is moved onscreen. All display messages sent to the window or its views are postponed until the window is created, just before it’s moved onscreen.
		/// </param>
		/// <returns>
		///		The initialized window.
		///	</returns>
		[Import]
		public abstract NSWindow InitWith(NSRect contentRect, NSWindowStyleMask styleMask, NSBackingStoreType backing, bool defer);

		/// <summary>
		///		The string that appears in the title bar of the window or the path to the represented file.
		/// </summary>
		[Import]
		public abstract NSString Title { get; set; }


		[Import]
		public abstract NSRect Frame { get; }

		/// <summary>
		///		Sets the origin and size of the window’s frame rectangle according to a given frame rectangle, thereby setting its position and size onscreen.
		/// </summary>
		/// <param name="frame">
		///		The frame rectangle for the window, including the title bar.
		///	</param>
		/// <param name="display">
		///		Specifies whether the window redraws the views that need to be displayed. When YES the window sends a displayIfNeeded message down its view hierarchy, thus redrawing all views.
		/// </param>
		[Import(Name = "setFrame:display:")]
		public abstract void SetFrame(NSRect frame, bool display);

		/// <summary>
		///		Returns the content rectangle used by a window with a given frame rectangle and window style.
		/// </summary>
		/// <remarks>
		///		When a NSWindow instance is available, you should use <see cref="NSWindow.ContentRectForFrameRect(NSRect)"/> instead of this method.
		/// </remarks>
		/// <param name="frameRect">
		///		The frame rectangle for the window expressed in screen coordinates.
		///	</param>
		/// <param name="styleMask">
		///		The window style for the window. See <see cref="NSWindowStyleMask"/> for a list of style mask values.
		/// </param>
		/// <returns>
		///		The content rectangle, expressed in screen coordinates, used by the window with fRect and style.
		///	</returns>
		public static NSRect ContentRectForFrameRect(NSRect frameRect, NSWindowStyleMask styleMask) => Meta.ContentRectForFrameRect(frameRect, styleMask);

		/// <summary>
		///		Returns the window’s content rectangle with a given frame rectangle.
		/// </summary>
		/// <remarks>
		///		The window uses its current style mask in computing the content rectangle. See NSWindowStyleMask for a list of style mask values. 
		///		main advantage of this instance-method counterpart to contentRectForFrameRect:styleMask: is that it allows you to take toolbars into account when converting between content and frame rectangles.
		///		(The toolbar is not included in the content rectangle.)
		/// </remarks>
		/// <param name="frameRect">
		///		The frame rectangle for the window expressed in screen coordinates.
		///	</param>
		/// <returns>
		///		The window’s content rectangle, expressed in screen coordinates, with frameRect.
		///	</returns>
		[Import(Name = "contentRectForFrameRect:")]
		public abstract NSRect ContentRectForFrameRect(NSRect frameRect);

		/// <summary>
		///		Returns the frame rectangle used by a window with a given content rectangle and window style.
		/// </summary>
		/// <param name="contentRect">
		///		The content rectangle for a window expressed in screen coordinates.
		/// </param>
		/// <param name="styleMask">
		///		The window style for the window. See <see cref="NSWindowStyleMask"/> for a list of style mask values.
		/// </param>
		/// <returns></returns>
		public static NSRect FrameRectForContentRect(NSRect contentRect, NSWindowStyleMask styleMask) => Meta.FrameRectForContentRect(contentRect, styleMask);

		/// <summary>
		///		Returns the window’s frame rectangle with a given content rectangle.
		/// </summary>
		/// <remarks>
		///		The window uses its current style mask in computing the frame rectangle. See NSWindowStyleMask for a list of style mask values.
		///		The major advantage of this instance-method counterpart to frameRectForContentRect:styleMask: is that it allows you to take toolbars into account when converting between content and frame rectangles.
		///		(The toolbar is included in the frame rectangle but not the content rectangle.)
		/// </remarks>
		/// <param name="contentRect">
		///		The content rectangle for the window expressed in screen coordinates.
		///	</param>
		/// <returns>
		///		The window’s frame rectangle, expressed in screen coordinates, with contentRect.
		///	</returns>
		[Import(Name = "frameRectForContentRect:")]
		public abstract NSRect FrameRectForContentRect(NSRect contentRect);

		/// <summary>
		///		Sets the size of the window’s content view to a given size, which is expressed in the window’s base coordinate system.
		/// </summary>
		/// <remarks>
		///		This size in turn alters the size of the NSWindow object itself. Note that the window server limits window sizes to 10,000; if necessary, be sure to limit aSize relative to the frame rectangle.
		/// </remarks>
		/// <param name="size">
		///		The new size of the window’s content view in the window’s base coordinate system.
		/// </param>
		[Import(Name = "setContentSize:")]
		public abstract void SetContentSize(NSSize size);

		[Import(Name = "makeKeyAndOrderFront:")]
		public abstract void MakeKeyAndOrderFront(NSObject? sender);

		[Import]
		public abstract void OrderFront(NSObject? sender);

		[Import]
		public abstract void OrderOut(NSObject? sender);

		[Import(Name = "cascadeTopLeftFromPoint:")]
		public abstract void CascadeTopLeftFromPoint(NSPoint point);

		[Import]
		public abstract NSView ContentView { get; set; }

		[Import]
		public abstract NSAppearance Appearance { get; set; }

		public new abstract class MetaClass : NSResponder<NSWindow, MetaClass>.MetaClass {
			protected MetaClass(Class @class) : base(@class) {

			}

			[Import(Name = "contentRectForFrameRect:styleMask:")]
			public abstract NSRect ContentRectForFrameRect(NSRect frameRect, NSWindowStyleMask styleMask);

			[Import(Name = "frameRectForContentRect:styleMask:")]
			public abstract NSRect FrameRectForContentRect(NSRect contentRect, NSWindowStyleMask styleMask);
		}
	}
}

namespace Quilt.UI.Mac {
  using System;
  using Quilt.Graphics;
  using Quilt.Mac.AppKit;
  using Quilt.Mac.Foundation;
	using Quilt.UI;
  using Quilt.Utilities;

  public class CocoaWindow : Window {
		private readonly NSWindow _window;

		protected override Context Context { get; }

		internal CocoaWindow(float left, float top, float width, float height, WindowStyle windowStyle) {
			var styleMask = WindowStyleToNSWindowStyleMask(windowStyle);

			var contentRect = NSWindow.ContentRectForFrameRect(new NSRect(left, top, width, height), styleMask);

			_window = NSWindow.Alloc().InitWith(contentRect, styleMask, NSBackingStoreType.Buffered, false);

			Context = Context.Create(
				new ContextOptions(
					new PixelFormat(
						dataType: PixelFormatDataType.Float,
						redBits: 8,
						greenBits: 8,
						blueBits: 8,
						alphaBits: 8,
						depthBits: 1,
						stencilBits: 1
					)
				),
				_window.DangerousGetHandle()
			) ?? throw new NotSupportedException();
		}

		public override string Title {
			get => _window.Title.ToString();
			set {
				var nsString = NSString.From(value);

				_window.Title = nsString;

				nsString.Release();
			}
		}

		public override Rectangle FrameRect {
			get {
				var (left, top, width, height) = _window.Frame;

				return new Rectangle(left.ToSingle(), top.ToSingle(), width.ToSingle(), height.ToSingle());
			}
			set {
				_window.SetFrame(new NSRect(value.Left, value.Top, value.Width, value.Height), false);
			}
		}

		public override Rectangle ContentRect {
			get {
				var (left, top, width, height) = _window.ContentRectForFrameRect(_window.Frame);

				return new Rectangle(left.ToSingle(), top.ToSingle(), width.ToSingle(), height.ToSingle());
			}
			set {
				var frame = _window.FrameRectForContentRect(new NSRect(value.Left, value.Top, value.Width, value.Height));

				_window.SetFrame(frame, true);
			}
		}

		public override void Show() {
			_window.OrderFront(null);
		}

		public override void Hide() {
			_window.OrderOut(null);
		}

		private static NSWindowStyleMask WindowStyleToNSWindowStyleMask(WindowStyle windowStyle) {
			var styleMask = (NSWindowStyleMask)0;

			if (windowStyle.IsSet(WindowStyle.HasTitle)) {
				styleMask |= NSWindowStyleMask.Titled;
			}

			if (!windowStyle.IsSet(WindowStyle.HasBorder)) {
				styleMask |= NSWindowStyleMask.Borderless;
			}

			if(windowStyle.IsSet(WindowStyle.HasCloseButton)) {
				styleMask |= NSWindowStyleMask.Closable;
			}

			// no simile for WindowStyle.HasMaximizeButton

			if(windowStyle.IsSet(WindowStyle.HasMinimizeButton)) {
				styleMask |= NSWindowStyleMask.Miniaturizable;
			}

			if (windowStyle.IsSet(WindowStyle.IsResizable)) {
				styleMask |= NSWindowStyleMask.Resizable;
			}

			// no simile for WindowStyle.IsMoveable

			return styleMask;
		}
	}
}

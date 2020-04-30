namespace Quilt.UI {
	using Quilt.Graphics;

	public abstract class Window {
		protected abstract Context Context { get; }

		public abstract string Title { get; set; }
		public abstract Rectangle FrameRect { get; set; }
		public abstract Rectangle ContentRect { get; set; }
		public abstract void Show();
		public abstract void Hide();

		public static Window Create(float left, float top, float width, float height, WindowStyle windowStyle) => Backend.Instance.CreateWindow(left, top, width, height, windowStyle);
	}
}

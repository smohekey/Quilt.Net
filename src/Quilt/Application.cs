namespace Quilt {
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Xml;
	using Quilt.GLFW;
	using Quilt.UI;

	public class Application {
		private static readonly Lazy<Application> __lazyInstance = new Lazy<Application>(() => new Application());
		internal static Application Instance => __lazyInstance.Value;

		internal readonly GLFWContext _glfw;
		internal readonly HashSet<Window> _windows;

		private Application() {
			_glfw = GLFWContext.Create();
			_glfw.WindowHint(Hint.Samples, 4);

			_windows = new HashSet<Window>();
		}

		private void RunInternal() {
			while (_windows.Any()) {
				_glfw.WaitEvents();
			}
		}

		public static void Run() {
			Instance.RunInternal();
		}
	}
}

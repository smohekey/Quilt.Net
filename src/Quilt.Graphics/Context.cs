namespace Quilt.Graphics {
	using System;
  
  public abstract class Context {
		public static Context? Create(ContextOptions options, IntPtr window) {
			return Backend.CreateContext((backend) => backend.CreateContext(options, window));
		}

		public static Context? Create(ContextOptions options, Context shareContext, IntPtr window) {
			return Backend.CreateContext((backend) => backend.CreateContext(options, shareContext, window));
		}
	}
}

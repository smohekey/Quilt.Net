namespace Quilt.UI {
	using System;
  using System.Reflection;

  internal abstract class Backend {
		private static readonly Lazy<Backend> __lazyBackend = new Lazy<Backend>(LoadBackend());

		private static Backend LoadBackend() {
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
				var backend = default(Backend);

				try {
					if (assembly.GetCustomAttribute<BackendAttribute>() is BackendAttribute backendAttribute) {
						backend = (Backend)Activator.CreateInstance(backendAttribute.BackendType);
					}
				} catch (Exception) {

				}

				if (backend?.IsSupported ?? false) {
					return backend;
				}
			}

			throw new NotSupportedException($"A backend supporting the current platform could not be found.");
		}

		public static Backend Instance => __lazyBackend.Value;

		public abstract bool IsSupported { get; }

		public abstract Window CreateWindow(float left, float top, float width, float height, WindowStyle windowStyle);
	}
}

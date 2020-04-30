namespace Quilt.Graphics {
	using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;

  internal abstract class Backend {
		private static readonly Lazy<Backend[]> __lazyBackends = new Lazy<Backend[]>(() => LoadBackends().OrderBy(b => b.IsPreferred).ToArray());
		private static readonly Lazy<Backend> __lazyPreferred = new Lazy<Backend>(SelectPreferred);

		private static IEnumerable<Backend> LoadBackends() {
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
				var backend = default(Backend);

				try {
					if (assembly.GetCustomAttribute<BackendAttribute>() is BackendAttribute backendAttribute) {
						backend = (Backend)Activator.CreateInstance(backendAttribute.BackendType);
					}
				} catch (Exception) {

				}

				if (backend?.IsSupported ?? false) {
					yield return backend;
				}
			}
		}

		private static Backend SelectPreferred() {
			foreach (var backend in __lazyBackends.Value) {
				if(backend.IsPreferred) {
					return backend;
				}
			}

			throw new NotSupportedException();
		}

		public static T? CreateContext<T>(Func<Backend, T?> create) where T : class {
			foreach(var backend in __lazyBackends.Value) {
				if(create(backend) is T resource) {
					return resource;
				}
			}

			return null;
		}

		public abstract bool IsSupported { get; }
		public abstract bool IsPreferred { get; }

		public abstract Context? CreateContext(ContextOptions options, IntPtr window);
		public abstract Context? CreateContext(ContextOptions options, Context shareContext, IntPtr window);
	}
}

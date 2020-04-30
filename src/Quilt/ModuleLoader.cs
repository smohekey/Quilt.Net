namespace Quilt {
  using System;
  using System.IO;
  using System.Threading;
	using Microsoft.Extensions.Logging;
	using Quilt.Utilities;

  public sealed class ModuleLoader : IDisposable {
		private readonly ILogger _logger;
		private readonly string _path;
		private readonly TimeSpan _reloadTimeout;
		private readonly FileSystemWatcher _fileSystemWatcher;
		private readonly Action _reload;

		private PatchAssemblyLoadContext? _context;

		public ModuleLoader(ILogger<ModuleLoader> logger, string path, TimeSpan reloadTimeout) {
			_logger = logger;
			_path = path;
			_reloadTimeout = reloadTimeout;

			_fileSystemWatcher = new FileSystemWatcher {
				Path = path,
				Filter = "*.dll",
				NotifyFilter = NotifyFilters.LastWrite,
				IncludeSubdirectories = true
			};

			_fileSystemWatcher.Changed += ModuleChanged;
			_fileSystemWatcher.EnableRaisingEvents = true;

			_reload = new Action(() => {
				Unload();
				Load();
			}).Debounce(_reloadTimeout);
		}

		private void ModuleChanged(object sender, FileSystemEventArgs args) {
			if(!IsDisposed) {
				Reload();
			}
		}

		public void Load() {
			ThrowIfDisposed();

			if(_context != null) {
				throw new InvalidOperationException("Already loaded.");
			}

			_context = new PatchAssemblyLoadContext(_path);
		}

		public void Unload() {
			ThrowIfDisposed();

			if(_context == null) {
				throw new InvalidOperationException("Not loaded.");
			}

			_context.Unload();

			GC.Collect();
			GC.WaitForPendingFinalizers();
		}

		public void Reload() {
			_reload();
		}

		public void ThrowIfDisposed() {
			if(IsDisposed) {
				throw new ObjectDisposedException(nameof(ModuleLoader));
			}
		}

		public bool IsDisposed => _isdisposed == 1;

		private int _isdisposed = 0;

		public void Dispose() {
			if (Interlocked.Increment(ref _isdisposed) == 1) {
				_fileSystemWatcher.Dispose();
			}
		}
	}
}

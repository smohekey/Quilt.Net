namespace Quilt {
	using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Reflection;
  using Microsoft.Extensions.Logging;
  using Quilt.Abstractions;

  public sealed class ModuleManager : IDisposable {
		private const string MISMATCHED_ID = "Module in folder {Folder} has mismatched {Id}";

		private readonly ILogger _logger;
		private readonly string _path;

		private readonly FileSystemWatcher _fileSystemWatcher;
		private readonly Dictionary<string, ModuleDescriptor> _descriptors = new Dictionary<string, ModuleDescriptor>();

		public ModuleManager(ILogger logger, string path) {
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_path = Path.GetFullPath(path ?? throw new ArgumentNullException(nameof(path)));
			_fileSystemWatcher = CreateFileSystemWatcher();
			
			var dir = new DirectoryInfo(_path);

			foreach(var patchDir in dir.EnumerateDirectories()) {
				LoadDescriptor(patchDir.Name);
			}
		}

		private FileSystemWatcher CreateFileSystemWatcher() {
			_logger.LogDebug(Resources.ModuleManagerStartingFileSystemWatcher, _path);

			var fileSystemWatcher = new FileSystemWatcher {
				Path = _path,
				IncludeSubdirectories = false,
				NotifyFilter = NotifyFilters.LastWrite
			};

			fileSystemWatcher.Created += FolderCreated;
			fileSystemWatcher.Deleted += FolderDeleted;
			fileSystemWatcher.Renamed += FolderRenamed;
			fileSystemWatcher.EnableRaisingEvents = true;

			return fileSystemWatcher;
		}

		private void FolderRenamed(object sender, RenamedEventArgs args) {
			RemoveDescriptor(args.OldName);
			LoadDescriptor(args.Name);
		}

		private void FolderDeleted(object sender, FileSystemEventArgs args) {
			RemoveDescriptor(args.Name);
		}

		private void FolderCreated(object sender, FileSystemEventArgs args) {
			LoadDescriptor(args.Name);
		}

		private void RemoveDescriptor(string id) {
			if (_descriptors.ContainsKey(id)) {
				_descriptors.Remove(id);
			}
		}

		private void LoadDescriptor(string id) {
			if(!Directory.Exists(id)) {
				return;
			}

			var filePath = Path.Combine(_path, id, $"{id}.dll");

			if(!File.Exists(filePath)) {
				return;
			}

			var assembly = Assembly.ReflectionOnlyLoadFrom(filePath);

			if(!(assembly.GetCustomAttribute<ModuleAttribute>() is ModuleAttribute moduleAttribute)) {
				return;
			}

			if(moduleAttribute.Id != id) {
				_logger.LogWarning(MISMATCHED_ID, id, moduleAttribute.Id);

				return;
			}

			var version = new SemanticVersion(moduleAttribute.MajorVersion, moduleAttribute.MinorVersion, moduleAttribute.PatchVersion, moduleAttribute.PreRelease, moduleAttribute.Build);

			_descriptors.Add(id, new ModuleDescriptor(moduleAttribute.Id, moduleAttribute.Name, version, moduleAttribute.SupportedPlatforms, moduleAttribute.Description));
		}

		public void Dispose() {
			_fileSystemWatcher.Dispose();
		}
	}
}

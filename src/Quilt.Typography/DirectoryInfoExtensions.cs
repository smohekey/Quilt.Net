using System.Runtime.CompilerServices;
namespace Quilt.Typography {
	using System.Collections.Generic;
	using System.IO;

	internal static class DirectoryInfoExtensions {
		public static IEnumerable<FileInfo> EnumerateFiles(this DirectoryInfo @this, params string[] patterns) {
			foreach (var pattern in patterns) {
				foreach (var file in @this.EnumerateFiles(pattern)) {
					yield return file;
				}
			}
		}

		public static IEnumerable<FileInfo> EnumerateFiles(this DirectoryInfo @this, SearchOption options, params string[] patterns) {
			foreach (var pattern in patterns) {
				foreach (var file in @this.EnumerateFiles(pattern, options)) {
					yield return file;
				}
			}
		}
	}
}

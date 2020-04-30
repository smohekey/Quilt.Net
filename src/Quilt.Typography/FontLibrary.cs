namespace Quilt.Typography {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
  using System.Linq;
  using System.Runtime.InteropServices;
  using System.Text.RegularExpressions;

  public class FontLibrary : IEnumerable<Font> {
		private static readonly Regex __windowsRegex = new Regex("%([a-zA-Z][a-zA-Z0-9]*)%", RegexOptions.Compiled);

		private static readonly string[] __linuxDirectories = new[] {
			"~/.fonts",
			"/usr/local/share/fonts",
			"/usr/share/fonts"
		};

		private static readonly string[] __windowsDirectories = new[] {
			"%WINDIR%/fonts"
		};

		private static readonly string[] __osxDirectories = new[] {
			"~/Library/Fonts",
			"/Library/Fonts"
		};

		private readonly Dictionary<string, Font> _fonts = new Dictionary<string, Font>();

		public FontLibrary() {

		}

		public void LoadDirectory(string path) => LoadDirectory(new DirectoryInfo(path));

		public void LoadDirectory(DirectoryInfo directory) {
			if (!directory.Exists) {
				throw new ArgumentException("Directory doesn't exist.", nameof(directory));
			}

			foreach (var file in directory.EnumerateFiles(SearchOption.AllDirectories, "*.ttf", "*.otf")) {
				LoadFont(file);
			}
		}

		public void LoadPlatformDirectories() {
			foreach (var path in GetPlatformDirectories()) {
				var directory = new DirectoryInfo(path);

				if (directory.Exists) {
					LoadDirectory(directory);
				}
			}
		}

		public Font? LoadFont(string path) {
			return LoadFont(new FileInfo(path));
		}

		private Font? LoadFont(FileInfo file) {
			if (Font.Load(file) is Font font) {
				_fonts.Add(file.FullName, font);

				return font;
			}

			return null;
		}

		public IEnumerator<Font> GetEnumerator() {
			return _fonts.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		private static IEnumerable<string> GetPlatformDirectories() {
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
				return __linuxDirectories;
			} else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
				return __windowsDirectories.Select(s => ExpandEnvVars(__windowsRegex, s));
			} else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
				return __osxDirectories;
			} else {
				return Array.Empty<string>();
			}
		}

		private static string ExpandEnvVars(Regex regex, string path) {
			return regex.Replace(path, new MatchEvaluator(m => {
				return Environment.GetEnvironmentVariable(m.Groups[1].Value);
			}));
		}
	}
}

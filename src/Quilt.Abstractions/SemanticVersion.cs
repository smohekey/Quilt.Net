namespace Quilt.Abstractions {
  using System;
  using System.Globalization;
  using System.Text.RegularExpressions;

	public struct SemanticVersion : IEquatable<SemanticVersion> {
		private static readonly Regex __regex = new Regex(@"^(0|[1-9]\d*)\.(0|[1-9]\d*)\.(0|[1-9]\d*)(?:-((?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+([0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?$", RegexOptions.Compiled);
		public int Major { get; }
		public int Minor { get; }
		public int Patch { get; }
		public string? PreRelease { get; }
		public string? Build { get; }

		public SemanticVersion(int major, int minor, int patch, string? prerelease = null, string? build = null) {
			Major = major;
			Minor = minor;
			Patch = patch;
			PreRelease = prerelease;
			Build = build;
		}

		public static SemanticVersion? TryParse(string value) {
			if(string.IsNullOrEmpty(value)) {
				throw new ArgumentNullException(nameof(value));
			}

			var match = __regex.Match(value);

			if(!match.Success) {
				return null;
			}

			var format = CultureInfo.InvariantCulture.NumberFormat;

			var major = int.Parse(match.Groups[1].Value, format);
			var minor = int.Parse(match.Groups[2].Value, format);
			var patch = int.Parse(match.Groups[3].Value, format);
			var prerelease = match.Groups[4].Value;
			var build = match.Groups[5].Value;

			return new SemanticVersion( major, minor, patch, prerelease, build);
		}

		public override bool Equals(object obj) {
			return obj is SemanticVersion other && Equals(other);
		}

		public bool Equals(SemanticVersion other) {
			return
				Major == other.Major &&
				Minor == other.Minor &&
				Patch == other.Patch &&
				PreRelease == other.PreRelease &&
				Build == other.Build;
		}

		public override int GetHashCode() {
			var hash = 17;

			hash = hash * 23 + Major.GetHashCode();
			hash = hash * 23 + Minor.GetHashCode();
			hash = hash * 23 + Patch.GetHashCode();

			hash = (hash * 23 + PreRelease?.GetHashCode(StringComparison.InvariantCulture)) ?? hash;
			hash = (hash * 23 + Build?.GetHashCode(StringComparison.InvariantCulture)) ?? hash;

			return hash;
		}

		public static bool operator ==(SemanticVersion left, SemanticVersion right) {
			return left.Equals(right);
		}

		public static bool operator !=(SemanticVersion left, SemanticVersion right) {
			return !(left == right);
		}
	}
}

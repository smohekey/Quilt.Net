namespace Quilt.Abstractions {
	using System;
  
  [AttributeUsage(AttributeTargets.Assembly)]
	public sealed class ModuleAttribute : Attribute {
		public string Id { get; }
		public string Name { get; }
		public int MajorVersion { get; }
		public int MinorVersion { get; }
		public int PatchVersion { get; }
		public string? PreRelease { get; set; }
		public string? Build { get; set; }
		public SupportedPlatforms SupportedPlatforms { get; set; } = SupportedPlatforms.All;
		public string? Description { get; set; }

		public ModuleAttribute(string id, string name, int majorVersion, int minorVersion, int patchVersion) {
			Id = id;
			Name = name;
			MajorVersion = majorVersion;
			MinorVersion = minorVersion;
			PatchVersion = patchVersion;
		}
	}
}

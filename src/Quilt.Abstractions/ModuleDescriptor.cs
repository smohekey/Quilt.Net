namespace Quilt.Abstractions {
	public class ModuleDescriptor {
		public string Id { get; }
		public string Name { get; }
		public SemanticVersion Version { get; }
		public SupportedPlatforms SupportedPlatforms { get; }
		public string? Description { get; }

		public ModuleDescriptor(string id, string name, SemanticVersion version, SupportedPlatforms supportedPlatforms, string? description = null) {
			Id = id;
			Name = name;
			SupportedPlatforms = supportedPlatforms;
			Version = version;
			Description = description;
		}
	}
}

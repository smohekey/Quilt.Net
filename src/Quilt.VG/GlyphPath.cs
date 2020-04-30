namespace Quilt.VG {
	using Quilt.Typography;

	public class GlyphPath {
		public Glyph Glyph { get; }
		public Path Path { get; }

		public GlyphPath(Glyph glyph, Path path) {
			Glyph = glyph;
			Path = path;
		}
	}
}

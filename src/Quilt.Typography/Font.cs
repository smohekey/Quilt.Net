namespace Quilt.Typography {
  using System;
  using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using Quilt.Typography.SFNT;

	public abstract class Font : IEnumerable<Glyph> {
		protected readonly Func<Stream> _openStream;

		protected Font(Func<Stream> openStream) {
			_openStream = openStream;
		}

		public abstract int Ascender { get; }
		public abstract int Descender { get; }
		public abstract int UnitsPerEM { get; }

		public abstract uint GetGlyphIndex(char codePoint);
		public abstract Glyph GetGlyph(uint index);
		public Glyph GetGlyph(char codePoint) {
			return GetGlyph(GetGlyphIndex(codePoint));
		}

		public static Font? Load(FileInfo file) {
			using var stream = file.OpenRead();

			return Load(() => file.OpenRead());
		}

		public static Font? Load(byte[] buffer) {
			return Load(() => new MemoryStream(buffer));
		}

		public static Font? Load(Func<Stream> openStream) {
			if (SFNTFont.Load(openStream) is Font font) {
				return font;
			}

			return null;
		}

		internal Stream OpenStream() {
			return _openStream();
		}

		public abstract IEnumerator<Glyph> GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
}

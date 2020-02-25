using System;
using System.Runtime.InteropServices;

namespace Quilt.FreeType.Unmanaged {
	public struct FaceRec {
		public long num_faces;
		public long face_index;

		public long face_flags;
		public long style_flags;

		public long num_glyphs;

		public string family_name;
		public string style_name;

		public int num_fixed_sizes;
		//FT_Bitmap_Size* available_sizes;
		public IntPtr available_sizes;

		public int num_charmaps;

		//FT_CharMap* charmaps;
		public IntPtr charmaps;

		public Generic generic;

		/*# The following member variables (down to `underline_thickness`) */
		/*# are only relevant to scalable outlines; cf. @FT_Bitmap_Size    */
		/*# for bitmap fonts.                                              */
		public BBox bbox;

		public ushort units_per_EM;
		public short ascender;
		public short descender;
		public short height;

		public short max_advance_width;
		public short max_advance_height;

		public short underline_position;
		public short underline_thickness;

		//FT_GlyphSlot glyph;
		public IntPtr glyph;
		public Size size;
		FT_CharMap charmap;

		/*@private begin */

		FT_Driver driver;
		FT_Memory memory;
		FT_Stream stream;

		FT_ListRec sizes_list;

		Generic autohint;   /* face-specific auto-hinter data */
		IntPtr extensions; /* unused                         */

		FT_Face_Internal internal;

    /*@private end */
	}
}

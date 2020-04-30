namespace Quilt.Typography.SFNT {
	using System;
	using System.Collections.Generic;
	using Quilt.Utilities;

	public class Glyph : Typography.Glyph {
		private GlyphContour[] _contours = Array.Empty<GlyphContour>();

		public short NumberOfContours { get; set; }
		public override short XMin { get; }
		public override short YMin { get; }
		public override short XMax { get; }
		public override short YMax { get; }

		public Glyph(ReadOnlySpan<byte> span, ushort advanceWidth, short leftSideBearing) : base(advanceWidth, leftSideBearing) {
			var offset = 0;

			NumberOfContours = span.ReadInt16(ref offset);
			XMin = span.ReadInt16(ref offset);
			YMin = span.ReadInt16(ref offset);
			XMax = span.ReadInt16(ref offset);
			YMax = span.ReadInt16(ref offset);

			if (NumberOfContours > 0) {
				ReadSimpleGlyph(span.Slice(offset));
			} else if (NumberOfContours < 0) {
				ReadCompositeGlyph(span.Slice(offset));
			}
		}

		public Glyph(ushort advanceWidth, short leftSideBearing) : base(advanceWidth, leftSideBearing) {
			NumberOfContours = 0;
			XMin = 0;
			YMin = 0;
			XMax = 0;
			YMax = 0;
		}

		private void ReadSimpleGlyph(ReadOnlySpan<byte> span) {
			_contours = new GlyphContour[NumberOfContours];

			var offset = 0;
			var endIndexes = new uint[NumberOfContours];

			for (var i = 0; i < NumberOfContours; i++) {
				endIndexes[i] = span.ReadUInt16(ref offset);
			}

			// skip over instructions
			var instrCount = span.ReadUInt16(ref offset);

			for (var i = 0; i < instrCount; i++) {
				offset++;
			}

			var pointCount = endIndexes[^1] + 1;
			var flags = new SimpleGlyphFlag[pointCount];

			for (var i = 0; i < pointCount;) {
				var flag = (SimpleGlyphFlag)span[offset++];

				flags[i++] = flag;

				if (flag.IsSet(SimpleGlyphFlag.Repeat)) {
					var count = span[offset++];

					for (var j = 0; j < count; j++) {
						flags[i++] = flag;
					}
				}
			}

			var points = new GlyphPoint[pointCount];
			var contourIndex = 0;
			var lastStartIndex = 0u;

			for (var i = 0; i < pointCount; i++) {
				points[i] = new GlyphPoint {
					OnCurve = flags[i].IsSet(SimpleGlyphFlag.OnCurve)
				};

				var endIndex = endIndexes[contourIndex];

				if (i == endIndex) {
					var length = endIndex - lastStartIndex + 1;

					_contours[contourIndex++] = new GlyphContour(points, lastStartIndex, length);

					lastStartIndex = endIndex + 1;
				}
			}

			ReadCoords(span, ref offset, pointCount, (i, v) => points[i].X = v, flags, SimpleGlyphFlag.XShortVector, SimpleGlyphFlag.PositiveXShortVector);
			ReadCoords(span, ref offset, pointCount, (i, v) => points[i].Y = v, flags, SimpleGlyphFlag.YShortVector, SimpleGlyphFlag.PositiveYShortVector);
		}

		private void ReadCoords(ReadOnlySpan<byte> span, ref int offset, uint pointCount, Action<int, short> setValue, SimpleGlyphFlag[] flags, SimpleGlyphFlag byteFlag, SimpleGlyphFlag deltaFlag) {
			short value = 0;

			for (var i = 0; i < pointCount; i++) {
				var flag = flags[i];

				if (flag.IsSet(byteFlag)) {
					if (flag.IsSet(deltaFlag)) {
						value += span[offset++];
					} else {
						value -= span[offset++];
					}
				} else if ((~flag).IsSet(deltaFlag)) {
					value += span.ReadInt16(ref offset);
				}

				setValue(i, value);
			}
		}

		private void ReadCompositeGlyph(ReadOnlySpan<byte> span) {
			var offset = 0;
			var moreComponents = true;
			var hasInstructions = false;

			while (moreComponents) {
				var flags = (CompositeGlyphFlag)span.ReadUInt16(ref offset);
				var glyphIndex = span.ReadUInt16(ref offset);

				moreComponents = flags.IsSet(CompositeGlyphFlag.MoreComponents);
				hasInstructions = flags.IsSet(CompositeGlyphFlag.WeHaveInstructions);

				if (flags.IsSet(CompositeGlyphFlag.Arg1And2AreWords)) {
					//if(flags.IsSet(CompositeGlyphFlag.ArgsAreXYValues))
					span.ReadUInt16(ref offset);
					span.ReadUInt16(ref offset);
				} else {
					//span[offset++];
					//span[offset++];

					offset += 2;
				}

				if (flags.IsSet(CompositeGlyphFlag.WeHaveAScale)) {
					span.ReadUInt16(ref offset);
				} else if (flags.IsSet(CompositeGlyphFlag.WeHaveAnXAndYScale)) {
					span.ReadUInt16(ref offset);
					span.ReadUInt16(ref offset);
				} else if (flags.IsSet(CompositeGlyphFlag.WeHaveATwoByTwo)) {
					span.ReadUInt16(ref offset);
					span.ReadUInt16(ref offset);
					span.ReadUInt16(ref offset);
					span.ReadUInt16(ref offset);
				}
			}

			if (hasInstructions) {
				var instrCount = span.ReadUInt16(ref offset);

				for (int i = 0; i < instrCount; i++) {
					offset++;
				}
			}
		}

		public override IEnumerator<GlyphContour> GetEnumerator() {
			return ((IEnumerable<GlyphContour>)_contours).GetEnumerator();
		}

		[Flags]
		private enum SimpleGlyphFlag : byte {
			OnCurve = 0x01,
			XShortVector = 0x02,
			YShortVector = 0x04,
			Repeat = 0x08,
			XIsSame = 0x10,
			PositiveXShortVector = 0x10,
			YIsSame = 0x20,
			PositiveYShortVector = 0x20,
			OverlapSimple = 0x40
		}

		[Flags]
		private enum CompositeGlyphFlag : ushort {
			Arg1And2AreWords = 0x0001,
			ArgsAreXYValues = 0x0002,
			RoundXYToGrid = 0x0004,
			WeHaveAScale = 0x0008,
			MoreComponents = 0x0020,
			WeHaveAnXAndYScale = 0x0040,
			WeHaveATwoByTwo = 0x0080,
			WeHaveInstructions = 0x0100,
			UseMyMetrics = 0x0200,
			OverlapCompound = 0x0400,
			ScaledCompoundOffset = 0x0800,
			UnscaledCompoundOffset = 0x1000
		}
	}
}

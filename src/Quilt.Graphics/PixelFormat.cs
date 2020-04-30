namespace Quilt.Graphics {
	using System;
  using System.Diagnostics.CodeAnalysis;

  [SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "<Pending>")]
	public struct PixelFormat : IEquatable<PixelFormat> {
		public readonly PixelFormatDataType DataType;
		public readonly byte RedBits;
		public readonly byte GreenBits;
		public readonly byte BlueBits;
		public readonly byte AlphaBits;
		public readonly byte DepthBits;
		public readonly byte StencilBits;

		public PixelFormat(PixelFormatDataType dataType, byte redBits, byte greenBits, byte blueBits, byte alphaBits, byte depthBits, byte stencilBits) {
			DataType = dataType;
			RedBits = redBits;
			GreenBits = greenBits;
			BlueBits = blueBits;
			AlphaBits = alphaBits;
			DepthBits = depthBits;
			StencilBits = stencilBits;
		}

		public override bool Equals(object obj) {
			return obj is PixelFormat other && Equals(other);
		}

		public bool Equals(PixelFormat other) {
			return
				DataType == other.DataType &&
				RedBits == other.RedBits &&
				GreenBits == other.GreenBits &&
				BlueBits == other.BlueBits &&
				AlphaBits == other.AlphaBits &&
				DepthBits == other.DepthBits &&
				StencilBits == other.StencilBits;
		}

		public override int GetHashCode() {
			unchecked {
				var hash = 17;

				hash = hash * 23 + DataType.GetHashCode();
				hash = hash * 23 + RedBits.GetHashCode();
				hash = hash * 23 + GreenBits.GetHashCode();
				hash = hash * 23 + BlueBits.GetHashCode();
				hash = hash * 23 + AlphaBits.GetHashCode();
				hash = hash * 23 + DepthBits.GetHashCode();
				hash = hash * 23 + StencilBits.GetHashCode();

				return hash;
			}
		}

		public static bool operator ==(PixelFormat left, PixelFormat right) {
			return left.Equals(right);
		}

		public static bool operator !=(PixelFormat left, PixelFormat right) {
			return !(left == right);
		}
	}
}

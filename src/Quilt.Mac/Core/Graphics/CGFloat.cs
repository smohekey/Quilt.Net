namespace Quilt.Mac.Core.Graphics {
	using System;
  using System.Runtime.InteropServices;
  using Quilt.Mac.CodeGen;

  [StructLayout(LayoutKind.Sequential)]
	[MarshalWith(typeof(CGFloatMarshaler))]
	public struct CGFloat : IEquatable<CGFloat> {
		private IntPtr _value;

		public CGFloat(IntPtr value) {
			_value = value;
		}

		public unsafe CGFloat(double value) {
			if (IntPtr.Size == 4) {
				float f = (float)value;

				_value = *(IntPtr*)&f;
			} else if (IntPtr.Size == 8) {
				_value = *(IntPtr*)&value;
			} else {
				throw new NotSupportedException();
			}
		}

		public unsafe CGFloat(float value) {
			if(IntPtr.Size == 4) {
				_value = *(IntPtr*)&value;
			} else if(IntPtr.Size == 8) {
				var d = (double)value;

				_value = *(IntPtr*)&d;
			} else {
				throw new NotSupportedException();
			}
		}

		public unsafe float ToSingle() {
			fixed (void* valuePtr = &_value) {
				if (IntPtr.Size == 4) {
					return *(float*)valuePtr;
				} else if (IntPtr.Size == 8) {
					return (float)*(double*)valuePtr;
				} else {
					throw new NotSupportedException();
				}
			}
		}

		public unsafe double ToDouble() {
			fixed(void* valuePtr = &_value) {
				if (IntPtr.Size == 4) {
					return *(float*)valuePtr;
				} else if (IntPtr.Size == 8) {
					return *(double*)valuePtr;
				} else {
					throw new NotSupportedException();
				}
			}
		}

		public static implicit operator CGFloat(double value) => new CGFloat(value);
		
		public static implicit operator CGFloat(float value) => new CGFloat(value);

		public override bool Equals(object obj) {
			return obj is CGFloat other && Equals(other);
		}

		public bool Equals(CGFloat other) {
			return
				_value == other._value;
		}

		public override int GetHashCode() {
			return _value.GetHashCode();
		}

		public static bool operator ==(CGFloat left, CGFloat right) {
			return left.Equals(right);
		}

		public static bool operator !=(CGFloat left, CGFloat right) {
			return !(left == right);
		}

		public override string ToString() {
			return ToDouble().ToString();
		}
	}
}

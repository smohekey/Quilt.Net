namespace Quilt.GL.Unmanaged {
	using System;
	using System.Runtime.CompilerServices;
	using System.Runtime.InteropServices;
	using System.Text;

	public static class IGLExtensionMethods {
		public static void ShaderSource(this IGL @this, Shader shader, params string[] sources) {
			var lengths = new int[sources.Length];

			for (int i = 0; i < lengths.Length; i++) {
				lengths[i] = -1;
			}

			@this.ShaderSource(shader, sources.Length, sources, lengths);
		}

		public static string GetShaderInfoLog(this IGL @this, Shader shader) {
			var infoLog = new StringBuilder(256);
			int length = 0;

			do {
				@this.GetShaderInfoLog(shader, infoLog.Capacity, out length, infoLog);
			} while (length > infoLog.Capacity);

			return infoLog.ToString();
		}

		public static string GetShaderSource(this IGL @this, Shader shader) {
			var source = new StringBuilder(256);
			int length = 0;

			do {
				@this.GetShaderSource(shader, source.Capacity, out length, source);
			} while (length > source.Capacity);

			return source.ToString();
		}

		public static string GetProgramInfoLog(this IGL @this, Program program) {
			var infoLog = new StringBuilder(256);
			int length = 0;

			do {
				@this.GetProgramInfoLog(program, infoLog.Capacity, out length, infoLog);
			} while (length > infoLog.Capacity);

			return infoLog.ToString();
		}

		public unsafe static Buffer[] GenBuffers(this IGL @this, int count) {
			var result = new Buffer[count];

			fixed (Buffer* resultPtr = result) {
				@this.GenBuffers(count, resultPtr);
			}

			return result;
		}

		public unsafe static Buffer GenBuffer(this IGL @this) {
			var result = new Buffer[1];

			fixed(Buffer* resultPtr = result) {
				@this.GenBuffers(1, resultPtr);
			}

			return result[0];
		}

		public unsafe static void BufferData<T>(this IGL @this, BufferType target, T[] data, BufferUsage usage) where T : unmanaged {
			var size = new IntPtr(Marshal.SizeOf<T>() * data.Length);

			fixed(void* dataPtr = data) {
				@this.BufferData(target, size, new IntPtr(dataPtr), usage);
			}
		}

		public unsafe static void NamedBufferData<T>(this IGL @this, Buffer buffer, T[] data, BufferUsage usage) where T : unmanaged {
			var size = new IntPtr(Marshal.SizeOf<T>() * data.Length);

			fixed (void* dataPtr = data) {
				@this.NamedBufferData(buffer, size, new IntPtr(dataPtr), usage);
			}
		}

		public unsafe static VertexArray[] GenVertexArrays(this IGL @this, int count) {
			var result = new VertexArray[count];

			fixed (VertexArray* resultPtr = result) {
				@this.GenVertexArrays(count, resultPtr);
			}

			return result;
		}

		public unsafe static VertexArray GenVertexArray(this IGL @this) {
			var result = new VertexArray[1];

			fixed (VertexArray* resultPtr = result) {
				@this.GenVertexArrays(1, resultPtr);
			}

			return result[0];
		}
	}
}

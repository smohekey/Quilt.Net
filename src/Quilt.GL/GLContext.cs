namespace Quilt.GL {
  using System;
  using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Runtime.InteropServices;
	using Quilt.GL.Unmanaged;
	using Quilt.Unmanaged;

	[UnmanagedObject(CallingConvention = CallingConvention.Cdecl, Prefix = "gl")]
	public abstract class GLContext : GLObject<byte> {
		protected GLContext(UnmanagedLibrary library) : base(library, 0) {

		}

		public abstract void ClearColor(float r, float g, float b, float a);
		public abstract void Clear(BufferBit buffer);
		public abstract void Viewport(int x, int y, int width, int height);

		protected abstract uint CreateShader(ShaderType type);

		public GLVertexShader CreateVertexShader(string source) {
			var shader = CreateShader(ShaderType.Vertex);

			CheckError();

			return _library.CreateObject<GLVertexShader>(shader, source);
		}

		public GLVertexShader CreateVertexShader(TextReader reader) {
			return CreateVertexShader(reader.ReadToEnd());
		}

		public GLVertexShader CreateVertexShader(Stream stream) {
			return CreateVertexShader(new StreamReader(stream));
		}

		public GLFragmentShader CreateFragmentShader(string source) {
			var shader = CreateShader(ShaderType.Fragment);

			CheckError();

			return _library.CreateObject<GLFragmentShader>(shader, source);
		}

		public GLFragmentShader CreateFragmentShader(TextReader reader) {
			return CreateFragmentShader(reader.ReadToEnd());
		}

		public GLFragmentShader CreateFragmentShader(Stream stream) {
			return CreateFragmentShader(new StreamReader(stream));
		}

		protected abstract uint CreateProgram();

		public GLProgram CreateProgram(params GLShader[] shaders) {
			var program = CreateProgram();

			CheckError();

			return _library.CreateObject<GLProgram>(program, shaders);
		}

		protected abstract unsafe void GenBuffers(GLsizei count, uint* result);

		private unsafe uint[] GenBuffers(int count) {
			var result = new uint[count];

			fixed (uint* resultPtr = result) {
				GenBuffers(count, resultPtr);
			}

			CheckError();

			return result;
		}

		public GLBuffer CreateBuffer() {
			var buffers = GenBuffers(1);

			return _library.CreateObject<GLBuffer>(buffers[0]);
		}

		public IEnumerable<GLBuffer> CreateBuffers(int count) {
			var buffers = GenBuffers(count);

			return buffers.Select(buffer => _library.CreateObject<GLBuffer>(buffer));
		}

		protected abstract unsafe void GenVertexArrays(GLsizei count, uint* result);

		private unsafe uint[] GenVertexArrays(int count) {
			var result = new uint[count];

			fixed (uint* resultPtr = result) {
				GenVertexArrays(count, resultPtr);
			}

			CheckError();

			return result;
		}

		public GLVertexArray CreateVertexArray() {
			var vertexArrays = GenVertexArrays(1);

			return _library.CreateObject<GLVertexArray>(vertexArrays[0]);
		}

		public IEnumerable<GLVertexArray> CreateVertexArrays(int count) {
			var vertexArrys = GenBuffers(count);

			return vertexArrys.Select(vertexArray => _library.CreateObject<GLVertexArray>(vertexArray));
		}

		public abstract void DrawArrays(DrawMode mode, int first, GLsizei count);

		protected abstract void DrawElements(DrawMode mode, GLsizei count, DataType type, GLsizei offset);
		public void DrawElements(DrawMode mode, int count, DataType type, int offset) {
			DrawElements(mode, (GLsizei)count, type, (GLsizei)offset);

			CheckError();
		}

		public abstract void Begin(DrawMode mode);
		public abstract void End();
		protected abstract void Color3f(float r, float g, float b);

		public void Color(float r, float g, float b) {
			Color3f(r, g, b);
		}

		protected abstract void Vertex2f(float x, float y);

		public void Vertex(float x, float y) {
			Vertex2f(x, y);
		}

		public abstract void Flush();

		protected override void DisposeUnmanaged() {

		}
	}
}

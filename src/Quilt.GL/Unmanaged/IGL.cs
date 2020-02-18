namespace Quilt.GL.Unmanaged {
	using System;
	using System.Runtime.InteropServices;
	using System.Text;
	using Quilt.Unmanaged;

	/*[UnmanagedObject(CallingConvention = CallingConvention.Cdecl, Prefix = "gl")]
	public abstract class GL {
		public abstract Error GetError();

		public abstract void ClearColor(float red, float green, float blue, float alpha);

		public abstract void Clear();

		public abstract int Viewport(int x, int y, int width, int height);

		public abstract Shader CreateShader(ShaderType type);

		public abstract void ShaderSource(Shader shader, int count, string[] sources, int[] lengths);

		public abstract void CompileShader(Shader shader);

		public abstract void GetShaderiv(Shader shader, ShaderProperty property, out int value);

		public abstract void GetShaderSource(Shader shader, int maxLength, out int length, StringBuilder source);

		public abstract void GetShaderInfoLog(Shader shader, int maxLength, out int length, StringBuilder infoLog);

		public abstract bool IsShader(Shader shader);

		public abstract void DeleteShader(Shader shader);

		public abstract Program CreateProgram();

		public abstract void AttachShader(Program program, Shader shader);

		public abstract void DetachShader(Program program, Shader shader);

		public abstract void LinkProgram(Program program);

		public abstract void GetProgramiv(Program program, ProgramProperty property, out int value);

		public abstract void GetProgramInfoLog(Program program, int maxLength, out int length, StringBuilder infoLog);

		public abstract bool IsProgram(Program program);

		public abstract void UseProgram(Program program);

		public abstract void DeleteProgram(Program program);

		public abstract void VertexAttribPointer(uint index, int size, DataType type, bool normalized, int stride, int offset);

		public abstract void VertexAttribIPointer(uint index, int size, DataType type, int stride, int offset);

		public abstract void VertexAttribLPointer(uint index, int size, DataType type, int stride, int offset);

		public abstract void EnableVertexAttribArray(uint index);

		public abstract void DisableVertexAttribArray(uint index);

		public abstract void EnableVertexArrayAttrib(VertexArray array, uint index);

		public abstract void DisableVertexArrayAttrib(VertexArray array, uint index);

		public abstract unsafe void GenBuffers(int count, Buffer* result);

		public abstract void BindBuffer(BufferType target, Buffer buffer);

		public abstract void BufferData(BufferType target, IntPtr size, IntPtr data, BufferUsage usage);

		public abstract void NamedBufferData(Buffer buffer, IntPtr size, IntPtr data, BufferUsage usage);

		public abstract unsafe void GenVertexArrays(int count, VertexArray* result);

		public abstract void BindVertexArray(VertexArray array);

		public abstract void DrawArrays(DrawMode mode, int first, int count);
		public abstract void DrawElements(DrawMode mode, int count, DataType type, int offset);

		public abstract int GetUniformLocation(Shader shader, string name);


	}*/
}

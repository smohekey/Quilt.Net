namespace Quilt.GL.Unmanaged {
  using System;
  using System.Runtime.InteropServices;
  using System.Text;
  using Quilt.Unmanaged;

  [UnmanagedInterface(CallingConvention = CallingConvention.Cdecl, Prefix = "gl")]
	public interface IGL {
		Error GetError();

		void ClearColor(float red, float green, float blue, float alpha);

		void Clear();

		int Viewport(int x, int y, int width, int height);

		Shader CreateShader(ShaderType type);

		void ShaderSource(Shader shader, int count, string[] sources, int[] lengths);

		void CompileShader(Shader shader);

		void GetShaderiv(Shader shader, ShaderProperty property, out int value);

		void GetShaderSource(Shader shader, int maxLength, out int length, StringBuilder source);

		void GetShaderInfoLog(Shader shader, int maxLength, out int length, StringBuilder infoLog);

		bool IsShader(Shader shader);

		void DeleteShader(Shader shader);

		Program CreateProgram();

		void AttachShader(Program program, Shader shader);

		void LinkProgram(Program program);

		void GetProgramiv(Program program, ProgramProperty property, out int value);

		void GetProgramInfoLog(Program program, int maxLength, out int length, StringBuilder infoLog);

		bool IsProgram(Program program);

		void UseProgram(Program program);

		void VertexAttribPointer(uint index, int size, DataType type, bool normalized, int stride, int offset);

		void VertexAttribIPointer(uint index, int size, DataType type, int stride, int offset);

		void VertexAttribLPointer(uint index, int size, DataType type, int stride, int offset);

		void EnableVertexAttribArray(uint index);

		void DisableVertexAttribArray(uint index);

		void EnableVertexArrayAttrib(VertexArray array, uint index);

		void DisableVertexArrayAttrib(VertexArray array, uint index);

		unsafe void GenBuffers(int count, Buffer* result);

		void BindBuffer(BufferType target, Buffer buffer);

		void BufferData(BufferType target, IntPtr size, IntPtr data, BufferUsage usage);

		void NamedBufferData(Buffer buffer, IntPtr size, IntPtr data, BufferUsage usage);

		unsafe void GenVertexArrays(int count, VertexArray* result);

		void BindVertexArray(VertexArray array);

		void DrawArrays(DrawMode mode, int first, int count);
		void DrawElements(DrawMode mode, int count, DataType type, int offset);

		int GetUniformLocation(Shader shader, string name);

		void Uniform1f(int location, float v0);
		void Uniform2f(int location, float v0, float v1);
		void Uniform3f(int location, float v0, float v1, float v2);
		void Uniform4f(int location, float v0, float v1, float v2, float v3);

		void Uniform1i(int location, int v0);
		void Uniform2i(int location, int v0, int v1);
		void Uniform3i(int location, int v0, int v1, int v2);
		void Uniform4i(int location, int v0, int v1, int v2, int v3);

		void Uniform1ui(int location, uint v0);
		void Uniform2ui(int location, uint v0, uint v1);
		void Uniform3ui(int location, uint v0, uint v1, uint v2);
		void Uniform4ui(int location, uint v0, uint v1, uint v2, uint v3);

		void Uniform1fv(int location, out float v0);
		void Uniform2fv(int location, out float v0, out float v1);
		void Uniform3fv(int location, out float v0, out float v1, out float v2);
		void Uniform4fv(int location, out float v0, out float v1, out float v2, out float v3);

		void Uniform1iv(int location, out int v0);
		void Uniform2iv(int location, out int v0, out int v1);
		void Uniform3iv(int location, out int v0, out int v1, out int v2);
		void Uniform4iv(int location, out int v0, out int v1, out int v2, out int v3);

		void Uniform1uiv(int location, out uint v0);
		void Uniform2uiv(int location, out uint v0, out uint v1);
		void Uniform3uiv(int location, out uint v0, out uint v1, out uint v2);
		void Uniform4uiv(int location, out uint v0, out uint v1, out uint v2, out uint v3);

	}
}

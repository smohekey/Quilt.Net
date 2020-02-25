using System.Xml;
using System.Linq;
namespace Quilt.GL {
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Numerics;
	using System.Runtime.InteropServices;
	using Quilt.GL.Unmanaged;
	using Quilt.Unmanaged;

	public delegate void DebugMessageCallback(DebugSource source, DebugType type, uint id, DebugSeverity severity, GLsizei length, string message, IntPtr userParam);

	[UnmanagedObject(CallingConvention = CallingConvention.Cdecl, Prefix = "gl")]
	public abstract class GLContext : GLObject {
		private readonly DebugMessageCallback _debugMessage;

		protected GLContext(UnmanagedLibrary library) : base(library, null!, 0) {
			DebugMessageCallback(_debugMessage = new DebugMessageCallback(HandleDebugMessage), IntPtr.Zero);
			DebugMessageControl(DebugSource.DontCare, DebugType.DontCare, DebugSeverity.DontCare, 0, null, true);

#if DEBUG
			foreach (var name in Enum.GetNames(typeof(StringName))) {
				Console.WriteLine(GetString(Enum.Parse<StringName>(name)));
			}
#endif
		}

		private void HandleDebugMessage(DebugSource source, DebugType type, uint id, DebugSeverity severity, GLsizei length, string message, IntPtr userParam) {
			Console.WriteLine($"{source} {type} {severity} {message}");
		}

		public abstract void ClearColor(float r, float g, float b, float a);
		public abstract void Clear(BufferBit buffer);
		public abstract void Viewport(int x, int y, GLsizei width, GLsizei height);

		#region Shaders
		protected abstract uint CreateShader(ShaderType type);

		public GLVertexShader CreateVertexShader(string source) {
			var shader = CreateShader(ShaderType.Vertex);

			CheckError();

			return _library.CreateObject<GLVertexShader>(this, shader, source);
		}

		public GLVertexShader CreateVertexShader(TextReader reader) {
			return CreateVertexShader(reader.ReadToEnd());
		}

		public GLVertexShader CreateVertexShader(Stream stream) {
			return CreateVertexShader(new StreamReader(stream));
		}

		public GLGeometryShader CreateGeometryShader(string source) {
			var shader = CreateShader(ShaderType.Geometry);

			CheckError();

			return _library.CreateObject<GLGeometryShader>(this, shader, source);
		}

		public GLGeometryShader CreateGeometryShader(TextReader reader) {
			return CreateGeometryShader(reader.ReadToEnd());
		}

		public GLGeometryShader CreateGeometryShader(Stream stream) {
			return CreateGeometryShader(new StreamReader(stream));
		}

		public GLFragmentShader CreateFragmentShader(string source) {
			var shader = CreateShader(ShaderType.Fragment);

			CheckError();

			return _library.CreateObject<GLFragmentShader>(this, shader, source);
		}

		public GLFragmentShader CreateFragmentShader(TextReader reader) {
			return CreateFragmentShader(reader.ReadToEnd());
		}

		public GLFragmentShader CreateFragmentShader(Stream stream) {
			return CreateFragmentShader(new StreamReader(stream));
		}
		#endregion

		#region Programs
		protected abstract uint CreateProgram();

		public GLProgram CreateProgram(params GLShader?[] shaders) {
			var program = CreateProgram();

			CheckError();

			return _library.CreateObject<GLProgram>(this, program, shaders);
		}

		protected abstract void UseProgram(uint program);

		public void UseProgram(GLProgram program) {
			UseProgram(program._handle);

			CheckError();
		}

		protected abstract int GetUniformLocation(uint program, string name);

		public int GetUniformLocation(GLProgram program, string name) {
			var index = GetUniformLocation(program._handle, name);

			CheckError();

			return index;
		}

		protected abstract void Uniform1f(int location, float v0);
		public void Uniform(int location, float v0) {
			Uniform1f(location, v0);

			CheckError();
		}

		protected abstract void Uniform2f(int location, float v0, float v1);
		public void Uniform(int location, float v0, float v1) {
			Uniform2f(location, v0, v1);

			CheckError();
		}

		protected abstract void Uniform3f(int location, float v0, float v1, float v2);
		public void Uniform(int location, float v0, float v1, float v2) {
			Uniform3f(location, v0, v1, v2);

			CheckError();
		}

		protected abstract void Uniform4f(int location, float v0, float v1, float v2, float v3);
		public void Uniform(int location, float v0, float v1, float v2, float v3) {
			Uniform4f(location, v0, v1, v2, v3);

			CheckError();
		}

		protected abstract void Uniform1i(int location, int v0);
		public void Uniform(int location, int v0) {
			Uniform1i(location, v0);

			CheckError();
		}

		protected abstract void Uniform2i(int location, int v0, int v1);
		public void Uniform(int location, int v0, int v1) {
			Uniform2i(location, v1, v1);

			CheckError();
		}

		protected abstract void Uniform3i(int location, int v0, int v1, int v2);
		public void Uniform(int location, int v0, int v1, int v2) {
			Uniform3i(location, v0, v1, v2);

			CheckError();
		}

		protected abstract void Uniform4i(int location, int v0, int v1, int v2, int v3);
		public void Uniform(int location, int v0, int v1, int v2, int v3) {
			Uniform4i(location, v0, v1, v2, v3);

			CheckError();
		}

		protected abstract void Uniform1ui(int location, uint v0);
		public void Uniform(int location, uint v0) {
			Uniform1ui(location, v0);

			CheckError();
		}

		protected abstract void Uniform2ui(int location, uint v0, uint v1);
		public void Uniform(int location, uint v0, uint v1) {
			Uniform2ui(location, v0, v1);

			CheckError();
		}

		protected abstract void Uniform3ui(int location, uint v0, uint v1, uint v2);
		public void Uniform(int location, uint v0, uint v1, uint v2) {
			Uniform3ui(location, v0, v1, v2);

			CheckError();
		}

		protected abstract void Uniform4ui(int location, uint v0, uint v1, uint v2, uint v3);
		public void Uniform(int location, uint v0, uint v1, uint v2, uint v3) {
			Uniform4ui(location, v0, v1, v2, v3);

			CheckError();
		}

		protected abstract void Uniform1fv(int location, out float v0);
		public void Uniform(int location, out float v0) {
			Uniform1fv(location, out v0);

			CheckError();
		}

		protected abstract void Uniform2fv(int location, out float v0, out float v1);
		public void Uniform(int location, out float v0, out float v1) {
			Uniform2fv(location, out v0, out v1);

			CheckError();
		}

		protected abstract void Uniform3fv(int location, out float v0, out float v1, out float v2);
		public void Uniform(int location, out float v0, out float v1, out float v2) {
			Uniform3fv(location, out v0, out v1, out v2);

			CheckError();
		}

		protected abstract void Uniform4fv(int location, out float v0, out float v1, out float v2, out float v3);
		public void Uniform(int location, out float v0, out float v1, out float v2, out float v3) {
			Uniform4fv(location, out v0, out v1, out v2, out v3);

			CheckError();
		}

		protected abstract void Uniform1iv(int location, out int v0);
		public void Uniform(int location, out int v0) {
			Uniform1iv(location, out v0);

			CheckError();
		}

		protected abstract void Uniform2iv(int location, out int v0, out int v1);
		public void Uniform(int location, out int v0, out int v1) {
			Uniform2iv(location, out v0, out v1);

			CheckError();
		}

		protected abstract void Uniform3iv(int location, out int v0, out int v1, out int v2);
		public void Uniform(int location, out int v0, out int v1, out int v2) {
			Uniform3iv(location, out v0, out v1, out v2);

			CheckError();
		}

		protected abstract void Uniform4iv(int location, out int v0, out int v1, out int v2, out int v3);
		public void Uniform(int location, out int v0, out int v1, out int v2, out int v3) {
			Uniform4iv(location, out v0, out v1, out v2, out v3);

			CheckError();
		}

		protected abstract void Uniform1uiv(int location, out uint v0);
		public void Uniform(int location, out uint v0) {
			Uniform1uiv(location, out v0);

			CheckError();
		}

		protected abstract void Uniform2uiv(int location, out uint v0, out uint v1);
		public void Uniform(int location, out uint v0, out uint v1) {
			Uniform2uiv(location, out v0, out v1);

			CheckError();
		}

		protected abstract void Uniform3uiv(int location, out uint v0, out uint v1, out uint v2);
		public void Uniform(int location, out uint v0, out uint v1, out uint v2) {
			Uniform3uiv(location, out v0, out v1, out v2);

			CheckError();
		}

		protected abstract void Uniform4uiv(int location, out uint v0, out uint v1, out uint v2, out uint v3);
		public void Uniform(int location, out uint v0, out uint v1, out uint v2, out uint v3) {
			Uniform4uiv(location, out v0, out v1, out v2, out v3);

			CheckError();
		}

		protected abstract unsafe void UniformMatrix4fv(int location, GLsizei count, bool transpose, IntPtr matrix);
		public unsafe void UniformMatrix(int location, int count, bool transpose, Matrix4x4 matrix) {
			var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<Matrix4x4>());

			Marshal.StructureToPtr(matrix, ptr, false);

			UniformMatrix4fv(location, count, transpose, ptr);

			Marshal.FreeHGlobal(ptr);

			CheckError();
		}

		#endregion

		#region Vertex Arrays
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

			return _library.CreateObject<GLVertexArray>(this, vertexArrays[0]);
		}

		public IEnumerable<GLVertexArray> CreateVertexArrays(int count) {
			var vertexArrys = GenVertexArrays(count);

			return vertexArrys.Select(vertexArray => _library.CreateObject<GLVertexArray>(this, vertexArray));
		}

		protected abstract void BindVertexArray(uint vertexArray);

		public void BindVertexArray(GLVertexArray vertexArray) {
			BindVertexArray(vertexArray._handle);

			CheckError();
		}

		protected abstract void VertexAttribPointer(uint index, int size, DataType type, bool normalized, GLsizei stride, GLsizei offset);
		protected abstract void VertexAttribIPointer(uint index, int size, DataType type, GLsizei stride, GLsizei offset);
		protected abstract void VertexAttribLPointer(uint index, int size, DataType type, GLsizei stride, GLsizei offset);

		[UnmanagedMethod(Name = "glEnableVertexAttribArray")]
		protected abstract void _EnableVertexAttribArray(uint index);

		[UnmanagedMethod(Name = "glDisableVertexAttribArray")]
		protected abstract void _DisableVertexAttribArray(uint index);
		protected abstract void EnableVertexArrayAttrib(uint vertexArray, uint index);
		protected abstract void DisableVertexArrayAttrib(uint vertexArray, uint index);

		public void VertexAttribPointer(uint index, int size, DataType type, bool normalized, int stride, int offset) {
			VertexAttribPointer(index, size, type, normalized, (GLsizei)stride, (GLsizei)offset);
			CheckError();
		}

		public void VertexAttribIPointer(uint index, int size, DataType type, int stride, int offset) {
			VertexAttribIPointer(index, size, type, (GLsizei)stride, (GLsizei)offset);
			CheckError();
		}

		public void VertexAttribLPointer(uint index, int size, DataType type, int stride, int offset) {
			VertexAttribLPointer(index, size, type, (GLsizei)stride, (GLsizei)offset);
			CheckError();
		}

		public void EnableVertexAttribArray(uint index) {
			_EnableVertexAttribArray(index);

			CheckError();
		}

		public void DisableVertexAttribArray(uint index) {
			_DisableVertexAttribArray(index);

			CheckError();
		}

		protected abstract unsafe void DeleteVertexArrays(GLsizei count, uint[] vertexArrays);

		private unsafe void DeleteVertexArrays(params GLVertexArray[] vertexArrays) {
			var vertexArrayUInts = vertexArrays.Select(va => va._handle).ToArray();

			DeleteVertexArrays(vertexArrays.Length, vertexArrayUInts);
		}
		#endregion

		#region Buffers
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

			return _library.CreateObject<GLBuffer>(this, buffers[0]);
		}

		public IEnumerable<GLBuffer> CreateBuffers(int count) {
			var buffers = GenBuffers(count);

			return buffers.Select(buffer => _library.CreateObject<GLBuffer>(this, buffer));
		}

		protected abstract void BindBuffer(BufferTarget target, uint buffer);

		public void BindBuffer(BufferTarget target, GLBuffer buffer) {
			BindBuffer(target, buffer._handle);

			CheckError();
		}

		protected abstract unsafe void BufferData(BufferTarget target, GLsizei size, void* data, BufferUsage usage);

		public unsafe void BufferData<T>(BufferTarget target, T[] data, int length, BufferUsage usage) where T : unmanaged {
			fixed (void* ptr = data) {
				BufferData(target, length * Marshal.SizeOf<T>(), ptr, usage);
			}

			CheckError();
		}

		public void BufferData<T>(BufferTarget target, T[] data, BufferUsage usage) where T : unmanaged {
			BufferData(target, data, data.Length, usage);
		}

		public unsafe void BufferData<T>(BufferTarget target, int length, BufferUsage usage) where T : unmanaged {
			BufferData(target, length * Marshal.SizeOf<T>(), null, usage);
		}

		protected abstract unsafe void BufferSubData(BufferTarget target, IntPtr offset, GLsizei size, void* data);

		public unsafe void BufferSubData<T>(BufferTarget target, int offset, T[] data, int length) where T : unmanaged {
			fixed (void* ptr = data) {
				BufferSubData(target, new IntPtr(offset), length, ptr);
			}

			CheckError();
		}

		public void BufferSubData<T>(BufferTarget target, int offset, T[] data) where T : unmanaged {
			BufferSubData(target, offset, data, data.Length);
		}

		protected abstract void DeleteBuffers(GLsizei count, uint[] buffers);

		public void DeleteBuffers(IEnumerable<GLBuffer> buffers) {
			var buffersArray = buffers.Select(b => b._handle).ToArray();

			DeleteBuffers(buffersArray.Length, buffersArray);

			CheckError();
		}

		public void DeleteBuffers(params GLBuffer[] buffers) {
			DeleteBuffers((IEnumerable<GLBuffer>)buffers);
		}

		public void DeleteBuffer(GLBuffer buffer) {
			DeleteBuffers(buffer);
		}
		#endregion

		[UnmanagedMethod(Name = "glPolygonMode")]
		protected abstract void _PolygonMode(FaceSelection faceSelection, PolygonMode mode);

		public void PolygonMode(FaceSelection faceSelection, PolygonMode mode) {
			_PolygonMode(faceSelection, mode);
		}
		public abstract void DrawArrays(DrawMode mode, int first, GLsizei count);

		protected abstract void DrawElements(DrawMode mode, GLsizei count, DataType type, GLsizei offset);
		public void DrawElements(DrawMode mode, int count, DataType type, int offset) {
			DrawElements(mode, (GLsizei)count, type, (GLsizei)offset);

			CheckError();
		}

		[UnmanagedMethod(Name = "glBlendFunc")]
		protected abstract void _BlendFunc(BlendFactor sourceFactor, BlendFactor destFactor);

		public void BlendFunc(BlendFactor sourceFactor, BlendFactor destFactor) {
			_BlendFunc(sourceFactor, destFactor);

			CheckError();
		}

		[UnmanagedMethod(Name = "glEnable")]
		protected abstract void _Enable(Capability capability);
		public void Enable(Capability capability) {
			_Enable(capability);

			CheckError();
		}

		[UnmanagedMethod(Name = "glDisable")]
		protected abstract void _Disable(Capability capability);
		public void Disable(Capability capability) {
			_Disable(capability);

			CheckError();
		}

		[UnmanagedMethod(Name = "glGetString")]
		protected abstract string _GetString(StringName name);

		public string GetString(StringName name) {
			var result = _GetString(name);

			CheckError();

			return result;
		}

		[UnmanagedMethod(Name = "glStencilMask")]
		protected abstract void _StencilMask(uint mask);

		public void StencilMask(uint mask) {
			_StencilMask(mask);

			CheckError();
		}

		[UnmanagedMethod(Name = "glStencilOp")]
		protected abstract void _StencilOp(StencilOperation stencilFail, StencilOperation depthTestFail, StencilOperation depthTestPass);
		public void StencilOp(StencilOperation stencilFail, StencilOperation depthTestFail, StencilOperation depthTestPass) {
			_StencilOp(stencilFail, depthTestFail, depthTestPass);

			CheckError();
		}

		[UnmanagedMethod(Name = "glStencilFunc")]
		protected abstract void _StencilFunc(StencilFunc func, int @ref, uint mask);
		public void StencilFunc(StencilFunc func, int @ref, uint mask) {
			_StencilFunc(func, @ref, mask);

			CheckError();
		}

		[UnmanagedMethod(Name = "glClearStencil")]
		protected abstract void _ClearStencil(int index);
		public void ClearStencil(int index) {
			_ClearStencil(index);

			CheckError();
		}

		[UnmanagedMethod(Name = "glColorMask")]
		protected abstract void _ColorMask(bool red, bool green, bool blue, bool alpha);
		public void ColorMask(bool red, bool green, bool blue, bool alpha) {
			_ColorMask(red, green, blue, alpha);

			CheckError();
		}

		protected abstract void DebugMessageControl(DebugSource source​, DebugType type​, DebugSeverity severity​, GLsizei count​, uint[]? ids​, bool enabled​);

		protected abstract void DebugMessageCallback(DebugMessageCallback callback, IntPtr userParam);

		protected override void DisposeUnmanaged() {

		}
	}
}

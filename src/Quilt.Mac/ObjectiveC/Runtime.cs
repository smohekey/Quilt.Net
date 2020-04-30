namespace Quilt.Mac.ObjectiveC {
	using System;
	using System.Runtime.InteropServices;
	using System.Text;
  using Quilt.Mac.Foundation;

  public static class Runtime {
		public const string LIBRARY = "/usr/lib/libobjc.A.dylib";

		[DllImport(LIBRARY)]
		private static extern unsafe Class objc_getClass(byte* name);

		public static unsafe Class? GetClass(string name) {
			var nameLengthBytes = Encoding.UTF8.GetMaxByteCount(name.Length);
			var utf8NamePtr = stackalloc byte[nameLengthBytes];

			fixed (char* namePtr = name) {
				Encoding.UTF8.GetBytes(namePtr, name.Length, utf8NamePtr, nameLengthBytes);
			}

			var @class = objc_getClass(utf8NamePtr);

			return @class.IsInvalid ? null : @class;
		}

		[DllImport(LIBRARY)]
		private static extern unsafe Class objc_getMetaClass(byte* name);
		
		public static unsafe Class? GetMetaClass(string name) {
			var nameLengthBytes = Encoding.UTF8.GetMaxByteCount(name.Length);
			var utf8NamePtr = stackalloc byte[nameLengthBytes];

			fixed (char* namePtr = name) {
				Encoding.UTF8.GetBytes(namePtr, name.Length, utf8NamePtr, nameLengthBytes);
			}

			var @class = objc_getMetaClass(utf8NamePtr);

			return @class.IsInvalid ? null : @class;
		}

		[DllImport(LIBRARY)]
		private static extern unsafe Class objc_allocateClassPair(Class superclass, byte* name, UIntPtr extraBytes);

		/// <summary>
		/// Creates a new class and metaclass.
		/// 
		/// You can get a pointer to the new metaclass by calling object_getClass(newClass).
		/// 
		/// To create a new class, start by calling objc_allocateClassPair. Then set the class's attributes with functions like <see cref="Class.AddMethod"/> and <see cref="Class.AddInstanceVariable"/>. 
		/// When you are done building the class, call <see cref="Runtime.RegisterClassPair"/>. The new class is now ready for use.
		/// Instance methods and instance variables should be added to the class itself. Class methods should be added to the metaclass.
		/// </summary>
		/// <param name="superclass">The class to use as the new class's superclass, or null to create a new root class.</param>
		/// <param name="name">The string to use as the new class's name. The string will be copied.</param>
		/// <param name="extraBytes">The number of bytes to allocate for indexed ivars at the end of the class and metaclass objects. This should usually be 0.</param>
		/// <returns>The new class, or null if the class could not be created (for example, the desired name is already in use).</returns>
		public static unsafe Class? AllocateClassPair(Class superclass, string name, ulong extraBytes) {
			var nameLengthBytes = Encoding.UTF8.GetMaxByteCount(name.Length);
			var utf8NamePtr = stackalloc byte[nameLengthBytes];

			fixed(char* namePtr = name) {
				Encoding.UTF8.GetBytes(namePtr, name.Length, utf8NamePtr, nameLengthBytes);
			}

			var @class = objc_allocateClassPair(superclass, utf8NamePtr, (UIntPtr)extraBytes);

			return @class.IsInvalid ? null : @class;
		}

		[DllImport(LIBRARY)]
		private static extern void objc_registerClassPair(Class @class);

		/// <summary>
		/// Registers a class that was allocated using objc_allocateClassPair.
		/// </summary>
		/// <param name="class">The class you want to register.</param>
		public static void RegisterClassPair(Class @class) {
			objc_registerClassPair(@class);
		}

		[DllImport(LIBRARY)]
		private static extern void free(IntPtr ptr);

		public static void Free(IntPtr ptr) => free(ptr);

		[DllImport(LIBRARY)]
		public static extern IntPtr SendMsg(IntPtr @object, Selector selector);
	}
}

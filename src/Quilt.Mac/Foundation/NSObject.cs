namespace Quilt.Mac.Foundation {
	using System;
  using System.Runtime.InteropServices;
	using Quilt.Mac.CodeGen;
	using Quilt.Mac.ObjectiveC;

	[MarshalWith(typeof(NSObjectMarshaler))]
	[Class]
	public abstract class NSObject : SafeHandle {
		private NSObject() : base(IntPtr.Zero, false) {

		}

		protected NSObject(IntPtr handle) : base(IntPtr.Zero, false) {
			SetHandle(handle);
		}

		public override bool IsInvalid => handle == IntPtr.Zero;

		public IntPtr Handle => handle;

		public static TClass Alloc<TClass>() where TClass : NSObject {
			return Helper<TClass>.Alloc();
		}

		protected override bool ReleaseHandle() {
			// TODO: call objc_release?

			return true;
		}

		public abstract class MetaClass : NSObject {
			protected MetaClass(Class @class) : base(@class.DangerousGetHandle()) {

			}
		}

		private static class Helper<TClass> where TClass : NSObject {
			private static readonly Type __type = typeof(TClass);
			private static readonly Class __metaClass = Runtime.GetMetaClass(typeof(TClass).Name) ?? throw new ClassNotFoundException(__type);
			private static readonly Selector __allocSelector = "alloc";

			public static TClass Alloc() {
				return (TClass)Generator.Instance.Create(__type, Runtime.SendMsg(__metaClass.DangerousGetHandle(), __allocSelector));
			}
		}
	}

	[Class]
	public abstract class NSObject<TClass> : NSObject where TClass : NSObject<TClass> {
		private static readonly Lazy<MetaClass> __lazyMeta = new Lazy<MetaClass>(CreateMetaInstance);

		protected NSObject(IntPtr handle) : base(handle) {

		}

		private static Type? FindMetaClassType(Type abstractType) {
			var type = abstractType;

			while(type != null) {
				if(type.GetNestedType(nameof(MetaClass)) is Type metaClassType) {
					if(metaClassType == Types.NSObjectGeneric1MetaClass) {
						return metaClassType.MakeGenericType(abstractType);
					}

					return metaClassType;
				}

				type = type.BaseType;
			}

			return null;
		}

		private static MetaClass CreateMetaInstance() {
			var abstractType = typeof(TClass);
			var metaType = FindMetaClassType(abstractType) ?? throw new NotSupportedException($"Couldn't find meta class type for type {abstractType.FullName}.");
			var concreteType = Generator.Instance.GetConcreteType(abstractType);

			if (!(Runtime.GetClass(abstractType.Name) is Class @class)) {
				throw new ClassNotFoundException(abstractType);
			}

			return (MetaClass)Generator.Instance.Create(metaType, @class);
		}

		protected static MetaClass Meta => __lazyMeta.Value;

		public static TClass Alloc() => Meta.Alloc();
		public static TClass New() => Meta.New();

		public static Class Class => Meta.Class();

		public static Class Superclass => Meta.Superclass();

		public static bool IsSubclassOf(Class @class) => Meta.IsSubclassOf(@class);

		[Import(Name = "init")]
		protected abstract IntPtr InternalInit();

		public TClass Init() {
			return Generator.Instance.Create<TClass, IntPtr>(InternalInit());
		}

		[Import]
		public abstract TClass Retain();

		[Import]
		public abstract void Release();

		[Import]
		public abstract TClass Autorelease();

		[Class]
		public new abstract class MetaClass : NSObject.MetaClass {
			protected MetaClass(Class @class) : base(@class) {

			}

			[Import]
			public abstract TClass Alloc();

			[Import]
			public abstract TClass New();

			[Import]
			public abstract Class Class();

			[Import]
			public abstract Class Superclass();

			[Import]
			public abstract bool IsSubclassOf(Class @class);
		}
	}

	[Class]
	public abstract class NSObject<TClass, TMetaClass> : NSObject<TClass>
		where TClass : NSObject<TClass, TMetaClass>
		where TMetaClass : NSObject<TClass, TMetaClass>.MetaClass {


		protected NSObject(IntPtr handle) : base(handle) {

		}
		protected static new TMetaClass Meta => (TMetaClass)NSObject<TClass>.Meta;

		[Class]
		public new abstract class MetaClass : NSObject<TClass>.MetaClass {
			protected MetaClass(Class @class) : base(@class) {

			}
		}
	}
}

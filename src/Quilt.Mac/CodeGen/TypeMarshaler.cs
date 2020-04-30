namespace Quilt.Mac.CodeGen {
	using System;
  using System.Collections.Concurrent;
  using System.Collections.Generic;
  using System.Diagnostics.CodeAnalysis;
  using System.Linq.Expressions;
  using System.Reflection;
  using Quilt.Mac.Foundation;
  using Sigil.NonGeneric;

	[SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Only called by GenerationContext and guaranteed not to be null.")]
	public abstract class TypeMarshaler {
		public GenerationContext Context { get; }
		public ParameterInfo Parameter { get; }

		protected TypeMarshaler(GenerationContext context, ParameterInfo parameter) {
			Context = context;
			Parameter = parameter;
		}

		public virtual string MsgSendName => "objc_msgSend";

		public virtual Type NativeParameterType => Parameter.ParameterType;

		public virtual Type NativeReturnType => Parameter.ParameterType;

		public virtual void EmitParameterSetup(Emit emit, ushort index) {
			
		}

		public virtual void EmitMarshalParameterIn(Emit emit, ushort index) {
			emit.LoadArgument(index);
		}

		public virtual void EmitMarshalParameterOutPrologue(Emit emit, ushort index) {

		}

		public virtual void EmitMarshalParameterOutEpilogue(Emit emit, ushort index) {

		}

		public virtual void EmitParameterCleanup(Emit emit, ushort index) {

		}

		public virtual void EmitMarshalReturnParameter(Emit emit) {

		}

		private static Dictionary<Type, Func<GenerationContext, ParameterInfo, TypeMarshaler>> __defaultTypeMarshalers = new Dictionary<Type, Func<GenerationContext, ParameterInfo, TypeMarshaler>>() {
			{ Types.String, (context, parameter) => new StringMarshaler(context, parameter) }
		};

		private static ConcurrentDictionary<Type, Func<GenerationContext, ParameterInfo, TypeMarshaler>> __typeMarshalers = new ConcurrentDictionary<Type, Func<GenerationContext, ParameterInfo, TypeMarshaler>>();

		public static TypeMarshaler CreateTypeMarshaler(GenerationContext context, ParameterInfo parameter) {
			if(parameter == null) {
				throw new ArgumentNullException(nameof(parameter));
			}

			var type = parameter.ParameterType;

			if(!(FindMarshalWithAttribute(type) is MarshalWithAttribute marshalWith)) {
				if(__defaultTypeMarshalers.TryGetValue(type, out var typeMarshalerFunc)) {
					return typeMarshalerFunc(context, parameter);
				}

				return new DefaultMarshaler(context, parameter);
			}

			if(!Types.TypeMarshaler.IsAssignableFrom(marshalWith.Type)) {
				throw new NotSupportedException($"Type {type.FullName} is marked as MarshalWith({marshalWith.Type.FullName} which refers to a type that doesn't implement {nameof(TypeMarshaler)}.");
			}

			return __typeMarshalers.GetOrAdd(marshalWith.Type, (marshalerType) => CreateTypeMarshalerFunc(marshalerType))(context, parameter);
		}

		private static MarshalWithAttribute? FindMarshalWithAttribute(Type type) {
			if(type.GetCustomAttribute<MarshalWithAttribute>() is MarshalWithAttribute marshalWithAttribute) {
				return marshalWithAttribute;
			}

			foreach(var @interface in type.GetInterfaces()) {
				if(@interface.GetCustomAttribute<MarshalWithAttribute>() is MarshalWithAttribute marshalWithAttribute1) {
					return marshalWithAttribute1;
				}
			}

			return null;
		}

		private static Func<GenerationContext, ParameterInfo, TypeMarshaler> CreateTypeMarshalerFunc(Type type) {
			var constructor = type.GetConstructor(new[] { typeof(GenerationContext), typeof(ParameterInfo) });
			var parameters = new ParameterExpression[] {
					Expression.Parameter(Types.GenerationContext),
					Expression.Parameter(Types.ParameterInfo)
			};

			return Expression.Lambda<Func<GenerationContext, ParameterInfo, TypeMarshaler>>(
				Expression.New(constructor, parameters), parameters
			).Compile();
		}

		public static void EmitNativeToManaged<TObject>(GenerationContext context, Emit emit) where TObject : NSObject {
			EmitNativeToManaged(context, emit, typeof(TObject));
		}

		public static void EmitNativeToManaged(GenerationContext context, Emit emit, Type type) {
			if (type == context.BaseType) {
				foreach (var (constructor, parameters) in context.Constructors) {
					if (parameters.Length == 1 && parameters[0] == Types.IntPtr) {
						emit.NewObject(constructor, parameters);

						return;
					}
				}

				throw new NotSupportedException();
			} else {
				var concreteType = context.Generator.GetConcreteType(type);

				emit.NewObject(concreteType, Types.IntPtr);

				return;
			}
		}
	}
}

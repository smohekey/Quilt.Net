namespace Quilt {
	using System;
	using System.ComponentModel;
	using System.Reflection;

	abstract class QuiltAttribute {
		protected static readonly Type __stringType = typeof(string);
		protected static readonly TypeConverter __stringTypeConverter = TypeDescriptor.GetConverter(__stringType);

		private readonly Element _element;
		private readonly Type _valueType;
		private readonly MethodInfo _getMethod;
		private readonly MethodInfo _setMethod;
		private readonly TypeConverter _valueTypeConverter;

		protected QuiltAttribute(Element element, Type valueType, MethodInfo getMethod, MethodInfo setMethod) {
			_element = element;
			_valueType = valueType;
			_getMethod = getMethod;
			_setMethod = setMethod;

			_valueTypeConverter = TypeDescriptor.GetConverter(_valueType);
		}

		public string? Value {
			get {
				object? value = _getMethod.Invoke(_element, Array.Empty<object>());

				// if the incoming value is null for reference types
				// or the default value for value types
				// we store null
				if (value == null) {
					return null;
				}

				// try to convert from the value type to string
				if (__stringTypeConverter != null) {
					if (__stringTypeConverter.CanConvertFrom(_valueType)) {
						return (string)__stringTypeConverter.ConvertFrom(value);
					}
				}

				// try to convert to string from the value type
				if (_valueTypeConverter != null) {
					if (_valueTypeConverter.CanConvertTo(__stringType)) {
						return (string)_valueTypeConverter.ConvertTo(value, __stringType);
					}
				}

				// fall back to attempting a basic ChangeType
				return (string)Convert.ChangeType(value, __stringType);
			}
			set {
				// otherwise, if the value is the empty string
				// we create an instance of the value type
				// this will return the default value for value types (the same as above),
				// but will return an empty instance for reference types
				if (string.IsNullOrEmpty(value)) {
					_setMethod.Invoke(_element, new object?[] { Activator.CreateInstance(_valueType) });

					return;
				}

				// try to convert from string to the value type
				if (__stringTypeConverter != null) {
					if (__stringTypeConverter.CanConvertTo(_valueType)) {
						_setMethod.Invoke(_element, new object[] { __stringTypeConverter.ConvertTo(value, _valueType) });

						return;
					}
				}

				// try to convert to the value type from string
				if (_valueTypeConverter != null) {
					if (_valueTypeConverter.CanConvertFrom(__stringType)) {
						_setMethod.Invoke(_element, new object[] { _valueTypeConverter.ConvertFrom(value) });

						return;
					}
				}

				// fall back to attempting a basic ChangeType
				_setMethod.Invoke(_element, new object[] { Convert.ChangeType(value, _valueType) });
			}
		}
	}
}

namespace Quilt.Mac.ObjectiveC {
	using System;

	public class PropertyNotFoundException : Exception {
		private Class _class;
		private string _propertyName;

		public PropertyNotFoundException(Class @class, string propertyName) {
			_class = @class;
			_propertyName = propertyName;
		}

		public override string Message => $"Couldn't find property named {_propertyName} on class {_class.Name}";
	}
}

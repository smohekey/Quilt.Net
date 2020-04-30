namespace Quilt.UI {
	using System;

	[AttributeUsage(AttributeTargets.Assembly)]
	internal class BackendAttribute : Attribute {
		public Type BackendType { get; }
		
		public BackendAttribute(Type backendType) {
			BackendType = backendType;
		}
	}
}

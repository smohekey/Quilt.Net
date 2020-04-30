namespace Quilt.Graphics {
	using System;
	
	[AttributeUsage(AttributeTargets.Assembly)]
	internal sealed class BackendAttribute : Attribute {
		public Type BackendType { get; }

		public BackendAttribute(Type backendType) {
			BackendType = backendType;
		}
	}
}

namespace Quilt.Mac.ObjectiveC {
	using System;
	
	public class ClassNotFoundException : Exception {
		private Type _classType;


		public ClassNotFoundException(Type classType) {
			_classType = classType;
		}
	}
}

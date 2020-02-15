namespace Quilt.GL.Exceptions {
	using System;
	using System.Runtime.Serialization;

	[Serializable]
	public class GLOutOfMemoryException : Exception {
		internal GLOutOfMemoryException() {

		}

		protected GLOutOfMemoryException(SerializationInfo info, StreamingContext context) : base(info, context) {

		}
	}
}

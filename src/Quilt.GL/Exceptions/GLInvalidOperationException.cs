namespace Quilt.GL.Exceptions {
	using System;
	using System.Runtime.Serialization;

	[Serializable]
	public class GLInvalidOperationException : Exception {
		internal GLInvalidOperationException() {

		}

		protected GLInvalidOperationException(SerializationInfo info, StreamingContext context) : base(info, context) {

		}
	}
}

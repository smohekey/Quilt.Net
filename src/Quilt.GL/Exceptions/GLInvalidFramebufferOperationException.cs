namespace Quilt.GL.Exceptions {
	using System;
	using System.Runtime.Serialization;

	[Serializable]
	public class GLInvalidFramebufferOperationException : Exception {
		internal GLInvalidFramebufferOperationException() {

		}

		protected GLInvalidFramebufferOperationException(SerializationInfo info, StreamingContext context) : base(info, context) {

		}
	}
}

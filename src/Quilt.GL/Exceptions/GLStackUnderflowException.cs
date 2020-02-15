namespace Quilt.GL.Exceptions {
	using System;
	using System.Runtime.Serialization;

	[Serializable]
	public class GLStackUnderflowException : Exception {
		internal GLStackUnderflowException() {

		}

		protected GLStackUnderflowException(SerializationInfo info, StreamingContext context) : base(info, context) {

		}
	}
}

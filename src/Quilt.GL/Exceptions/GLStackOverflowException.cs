namespace Quilt.GL.Exceptions {
	using System;
	using System.Runtime.Serialization;

	[Serializable]
	public class GLStackOverflowException : Exception {
		internal GLStackOverflowException() {

		}

		protected GLStackOverflowException(SerializationInfo info, StreamingContext context) : base(info, context) {

		}
	}
}

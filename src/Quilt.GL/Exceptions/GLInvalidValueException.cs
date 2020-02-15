namespace Quilt.GL.Exceptions {
	using System;
	using System.Runtime.Serialization;

	[Serializable]
	public class GLInvalidValueException : Exception {
		internal GLInvalidValueException() {

		}

		protected GLInvalidValueException(SerializationInfo info, StreamingContext context) : base(info, context) {

		}
	}
}

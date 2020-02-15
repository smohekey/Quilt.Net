namespace Quilt.GL.Exceptions {
	using System;
  using System.Runtime.Serialization;

  [Serializable]
	public class GLInvalidEnumException : Exception {
		internal GLInvalidEnumException() {

		}

		protected GLInvalidEnumException(SerializationInfo info, StreamingContext context) : base(info, context) {

		}
	}
}

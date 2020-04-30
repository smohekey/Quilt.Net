namespace Quilt.VG {
  using System;
  using System.Collections;
	using System.Collections.Generic;
  using System.Text;
  using Quilt.Collections;
  using Quilt.GL;

  public partial class Path : IEnumerable<PathContour>, IDisposable {
		private readonly List<Command> _commands;
		private readonly IEnumerable<PathContour> _contours;
		internal readonly GLVertexArray _vertexArray;
		internal readonly GLBuffer _vertexBuffer;

		public IEnumerable<Command> Commands => _commands;

		private Path(List<Command> commands, IEnumerable<PathContour> contours, GLVertexArray vertexArray, GLBuffer vertexBuffer) {
			_commands = commands;
			_contours = contours;
			_vertexArray = vertexArray;
			_vertexBuffer = vertexBuffer;
		}

		public IEnumerator<PathContour> GetEnumerator() {
			return _contours.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public string ToSVGString() {
			var builder = new StringBuilder();

			foreach(var command in _commands) {
				command.ToSVGString(builder);
			}

			return builder.ToString();
		}

		public void Dispose() {
			_vertexArray.Dispose();
			_vertexBuffer.Dispose();
		}
	}
}

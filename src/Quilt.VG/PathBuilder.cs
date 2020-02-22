namespace Quilt.VG {
	using System.Numerics;

	public class PathBuilder : IPathBuilder {
		public IFrameBuilder Frame { get; }
		public CommandList Commands { get; }

		private Vector2 _position;
		public Vector2 Position {
			get {
				return _position;
			}
			set {
				_position = value;

				Commands.Append(new Command {
					Type = CommandType.SetPosition,
					Position = value
				});
			}
		}

		private Vector4 _strokeColor;
		public Vector4 StrokeColor {
			get {
				return _strokeColor;
			}
			set {
				_strokeColor = value;

				Commands.Append(new Command {
					Type = CommandType.SetStrokeColor,
					StrokeColor = value
				});
			}
		}

		private float _strokeWidth;
		public float StrokeWidth {
			get {
				return _strokeWidth;
			}
			set {
				_strokeWidth = value;

				Commands.Append(new Command {
					Type = CommandType.SetStrokeWidth,
					StrokeWidth = value
				});
			}
		}

		private float _strokeMiter;
		public float StrokeMiter {
			get {
				return _strokeMiter;
			}
			set {
				_strokeMiter = value;

				Commands.Append(new Command {
					Type = CommandType.SetStrokeMiter,
					StrokeMiter = value
				});
			}
		}

		private StrokeFlags _strokeFlags;
		public StrokeFlags StrokeFlags {
			get {
				return _strokeFlags;
			}
			set {
				_strokeFlags = value;

				Commands.Append(new Command {
					Type = CommandType.SetStrokeFlags,
					StrokeFlags = value
				});
			}
		}

		private Vector4 _fillColor;
		public Vector4 FillColor {
			get {
				return _fillColor;
			}
			set {
				_fillColor = value;

				Commands.Append(new Command {
					Type = CommandType.SetFillColor,
					FillColor = value
				});
			}
		}

		public PathBuilder(IFrameBuilder frame, CommandList commandList) {
			Frame = frame;
			Commands = commandList;
		}

		public IPathBuilder SetPosition(Vector2 position) {
			Position = position;

			return this;
		}

		public IPathBuilder SetStrokeColor(Vector4 strokeColor) {
			StrokeColor = strokeColor;

			return this;
		}

		public IPathBuilder SetStrokeWidth(float strokeWidth) {
			StrokeWidth = strokeWidth;

			return this;
		}

		public IPathBuilder SetStrokeMiter(float strokeMiter) {
			StrokeMiter = strokeMiter;

			return this;
		}

		public IPathBuilder SetStrokeFlags(StrokeFlags strokeFlags) {
			StrokeFlags = strokeFlags;

			return this;
		}

		public IPathBuilder SetFillColor(Vector4 fillColor) {
			FillColor = fillColor;

			return this;
		}

		public IPathBuilder Fill() {
			Frame.FillPath(this);

			return this;
		}

		public IPathBuilder Stroke() {
			Frame.StrokePath(this);

			return this;
		}
	}
}

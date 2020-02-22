namespace Quilt.VG {
	using System.Numerics;

	public class PathBuilder : IPathBuilder, IFinishingPathBuilder {
		public IFrameBuilder Frame { get; }
		public Path Path { get; }

		private Vector2 _position;
		public Vector2 Position {
			get {
				return _position;
			}
			set {
				_position = value;

				Path.Append(new Command {
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

				Path.Append(new Command {
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

				Path.Append(new Command {
					Type = CommandType.SetStrokeWidth,
					StrokeWidth = value
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

				Path.Append(new Command {
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

				Path.Append(new Command {
					Type = CommandType.SetFillColor,
					FillColor = value
				});
			}
		}

		public PathBuilder(IFrameBuilder frame, Path commandList) {
			Frame = frame;
			Path = commandList;
		}

		public IPathBuilder SetPosition(Vector2 position) {
			Position = position;

			return this;
		}

		IFinishingPathBuilder IBasePathBuilder<IFinishingPathBuilder>.SetPosition(Vector2 position) {
			SetPosition(position);

			return this;
		}

		public IPathBuilder SetStrokeColor(Vector4 strokeColor) {
			StrokeColor = strokeColor;

			return this;
		}

		IFinishingPathBuilder IBasePathBuilder<IFinishingPathBuilder>.SetStrokeColor(Vector4 strokeColor) {
			SetStrokeColor(strokeColor);

			return this;
		}

		public IPathBuilder SetStrokeWidth(float strokeWidth) {
			StrokeWidth = strokeWidth;

			return this;
		}

		IFinishingPathBuilder IBasePathBuilder<IFinishingPathBuilder>.SetStrokeWidth(float strokeWidth) {
			SetStrokeWidth(strokeWidth);

			return this;
		}

		public IPathBuilder SetStrokeFlags(StrokeFlags strokeFlags) {
			StrokeFlags = strokeFlags;

			return this;
		}

		IFinishingPathBuilder IBasePathBuilder<IFinishingPathBuilder>.SetStrokeFlags(StrokeFlags strokeFlags) {
			SetStrokeFlags(strokeFlags);

			return this;
		}

		public IPathBuilder SetFillColor(Vector4 fillColor) {
			FillColor = fillColor;

			return this;
		}

		IFinishingPathBuilder IBasePathBuilder<IFinishingPathBuilder>.SetFillColor(Vector4 fillColor) {
			SetFillColor(fillColor);

			return this;
		}

		public IFinishingPathBuilder MoveTo(Vector2 position) {
			SetPosition(position);

			return this;
		}

		public IFinishingPathBuilder Fill() {
			Frame.FillPath(this);

			return this;
		}

		public IFinishingPathBuilder Stroke() {
			Frame.StrokePath(this);

			return this;
		}

		public IFrameBuilder Finish() {
			return Frame;
		}
	}
}

namespace Quilt.VG {
	using System;
	using System.Collections.Generic;
	using System.Numerics;
	using Quilt.Typography;

	public ref struct Context {
		private readonly Surface _surface;
		private readonly Matrix4x4 _projection;
		private readonly Vector2 _viewport;
		private readonly float _xScale;
		private readonly float _yScale;

		private Stack<State>? _stateStack;
		internal State _state;

		public Context(Surface surface, Matrix4x4 projection, Vector2 viewport, float xScale, float yScale) {
			_surface = surface;
			_projection = projection;
			_viewport = viewport;
			_xScale = xScale;
			_yScale = yScale;

			_stateStack = null;

			_state = new State {
				_transform = surface._transform,
				_font = surface._font,
				_fontSize = surface._fontSize,
				_strokeColor = Color.Black,
				_strokeWidth = 1,
				_strokeMiterLimit = 10,
				_strokeJoinStyle = VG.StrokeJoinStyle.Miter,
				_strokeCapStyle = VG.StrokeCapStyle.Butt,
				_fillColor = Color.White
			};
		}

		public void PushState() {
			if (_stateStack == null) {
				_stateStack = new Stack<State>();
			}

			_stateStack.Push(_state);

			_state = new State(_state);
		}

		public void PopState() {
			if (_stateStack == null || _stateStack.Count == 0) {
				throw new InvalidOperationException();
			}

			_state = _stateStack.Pop();
		}

		public void StrokeColor(Color strokeColor) {
			_state._strokeColor = strokeColor;
		}

		public void StrokeWidth(float strokeWidth) {
			_state._strokeWidth = strokeWidth * (_xScale + _yScale / 2);
		}

		public void StrokeMiterLimit(float strokeMiterLimit) {
			_state._strokeMiterLimit = strokeMiterLimit;
		}

		public void StrokeJoin(StrokeJoinStyle strokeJoin) {
			_state._strokeJoinStyle = strokeJoin;
		}

		public void StrokeCap(StrokeCapStyle strokeCap) {
			_state._strokeCapStyle = strokeCap;
		}

		public void FillColor(Color fillColor) {
			_state._fillColor = fillColor;
		}

		public void Transform(Matrix3x2 transform) {
			_state._transform = transform;
		}

		public void Font(Font font) {
			_state._font = font;
		}

		public void FontSize(ushort fontSize) {
			_state._fontSize = fontSize;
		}

		private (float, float) GetTextScale() {
			var fontSize = _state._fontSize;
			var dpix = _xScale * 96;
			var dpiy = _yScale * 96;

			var ppux = (dpix / 72 * fontSize) / _state._font.UnitsPerEM;
			var ppuy = (dpiy / 72 * fontSize) / _state._font.UnitsPerEM;

			return (ppux, ppuy);
		}

		public void Text(float x, float y, string text) {
			var (ppux, ppuy) = GetTextScale();

			var font = _state._font;
			var (tw, th) = TextExtents(text);

			PushState();

#if DEBUG
			StrokeColor(Color.White);
			StrokeWidth(1);

			using var bbox = _surface.CreatePath()
				.Rectangle(x, y, tw, th)
				.Build();

			//Stroke(bbox);
#endif

			Translate(x, y + _state._font.Ascender * ppuy);
			Scale(ppux, -ppuy);

			for (var i = 0; i < text.Length; i++) {
				var codePoint = text[i];
				var glyphPath = _surface._glyphSet.GetGlyphPath(codePoint);
				var (glyph, path) = (glyphPath.Glyph, glyphPath.Path);

				Fill(path);

#if DEBUG
				using var glyphBBox = _surface.CreatePath()
					.Rectangle(glyph.XMin, glyph.YMin, glyph.XMax - glyph.XMin, glyph.YMax - glyph.YMin)
					.Build();

				//Stroke(glyphBBox);
#endif

				Translate(glyph.AdvanceWidth, 0);
			}

			PopState();
		}

		public (int Width, int Height) TextExtents(string text) {
			var (ppux, ppuy) = GetTextScale();

			var w = 0;
			var h = (_state._font.Ascender - _state._font.Descender);

			for (var i = 0; i < text.Length; i++) {
				var codePoint = text[i];
				var glyph = _surface._glyphSet.GetGlyphPath(codePoint).Glyph;

				w += glyph.AdvanceWidth;
			}

			return ((int)MathF.Floor(w * ppux), (int)MathF.Floor(h * ppuy));
		}

		public void Identity() {
			_state._transform = Matrix3x2.Identity;
		}

		public void Translate(float dx, float dy) {
			_state._transform = Matrix3x2.Multiply(Matrix3x2.CreateTranslation(new Vector2(dx, dy)), _state._transform);
		}

		public void Scale(float sx, float sy) {
			_state._transform = Matrix3x2.Multiply(Matrix3x2.CreateScale(new Vector2(sx, sy)), _state._transform);
		}

		public void Rotate(float x, float y, float angle) {
			_state._transform = Matrix3x2.Multiply(Matrix3x2.CreateRotation(angle * Constants.DEG_2_RAD, new Vector2(x, y)), _state._transform);
		}

		public void Fill(Path path) {
			_surface._fillRenderer.Render(ref this, Matrix4x4.Multiply(new Matrix4x4(_state._transform), _projection), _viewport, path);
		}

		public void Stroke(Path path) {
			_surface._strokeRenderer.Render(ref this, Matrix4x4.Multiply(new Matrix4x4(_state._transform), _projection), _viewport, path);
		}

		internal struct State {
			public Matrix3x2 _transform;
			public Font _font;
			public float _fontSize;
			public Color _strokeColor;
			public float _strokeWidth;
			public float _strokeMiterLimit;
			public StrokeJoinStyle _strokeJoinStyle;
			public StrokeCapStyle _strokeCapStyle;
			public Color _fillColor;

			public State(State other) {
				_transform = other._transform;
				_font = other._font;
				_fontSize = other._fontSize;
				_strokeColor = other._strokeColor;
				_strokeWidth = other._strokeWidth;
				_strokeMiterLimit = other._strokeMiterLimit;
				_strokeJoinStyle = other._strokeJoinStyle;
				_strokeCapStyle = other._strokeCapStyle;
				_fillColor = other._fillColor;
			}
		}
	}
}

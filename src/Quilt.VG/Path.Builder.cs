namespace Quilt.VG {
	using System;
  using System.Collections.Generic;
  using System.Numerics;
	using System.Runtime.InteropServices;
	using Quilt.Collections;
	using Quilt.GL;

	public partial class Path {
		internal class Builder : IPathBuilder {

			private readonly Surface _vg;
			private readonly List<Command> _commands = new List<Command>();
			private readonly List<ContourDef> _contours = new List<ContourDef>();
			internal readonly List<PathPoint> _points = new List<PathPoint>();

			internal int _contourOffset;
			internal int _contourLength;
			private Vector2 _start;
			private Vector2 _position;
			private Vector2? _prevPosition;

			internal Builder(Surface vg) {
				_vg = vg;
				_contourOffset = 0;
				_contourLength = 0;
				_start = _position = Vector2.Zero;
			}

			private void AddCommand(Command command) {
				_commands.Add(command);
			}

			public IPathBuilder MoveTo(Vector2 position) {
				AddCommand(new MoveCommand(CommandFlags.None, position));

				return this;
			}

			public IPathBuilder MoveBy(Vector2 delta) {
				AddCommand(new MoveCommand(CommandFlags.IsDelta, delta));

				return this;
			}

			public IPathBuilder LineTo(Vector2 position) {
				AddCommand(new LineCommand(CommandFlags.None, position));

				return this;
			}

			public IPathBuilder LineBy(Vector2 delta) {
				AddCommand(new LineCommand(CommandFlags.IsDelta, delta));

				return this;
			}

			public IPathBuilder HorizontalLineTo(float x) {
				AddCommand(new HorizontalLineCommand(CommandFlags.None, x));

				return this;
			}

			public IPathBuilder HorizontalLineBy(float deltaX) {
				AddCommand(new HorizontalLineCommand(CommandFlags.IsDelta, deltaX));

				return this;
			}

			public IPathBuilder VerticalLineTo(float y) {
				AddCommand(new VerticalLineCommand(CommandFlags.None, y));

				return this;
			}

			public IPathBuilder VerticalLineBy(float deltaY) {
				AddCommand(new VerticalLineCommand(CommandFlags.IsDelta, deltaY));

				return this;
			}

			public IPathBuilder ArcTo(Vector2 p1, float radius, bool isClockwise) {
				var flags = isClockwise ? CommandFlags.EllipseOrArcIsClockwise : CommandFlags.None;

				AddCommand(new ArcCommand(flags, p1, radius));

				return this;
			}

			public IPathBuilder ArcBy(Vector2 d1, float radius, bool isClockwise) {
				var flags = CommandFlags.IsDelta;

				flags |= isClockwise ? CommandFlags.EllipseOrArcIsClockwise : CommandFlags.None;

				AddCommand(new ArcCommand(flags, d1, radius));

				return this;
			}

			public IPathBuilder EllipseTo(Vector2 p1, Vector2 radius, float angle, bool isLarge, bool isClockwise) {
				var flags = CommandFlags.None;

				flags |= isLarge ? CommandFlags.EllipseIsLarge : CommandFlags.None;
				flags |= isClockwise ? CommandFlags.EllipseOrArcIsClockwise : CommandFlags.None;

				AddCommand(new EllipseCommand(flags, p1, radius, angle));

				return this;
			}

			public IPathBuilder EllipseBy(Vector2 d1, Vector2 radius, float angle, bool isLarge, bool isClockwise) {
				var flags = CommandFlags.IsDelta;

				flags |= isLarge ? CommandFlags.EllipseIsLarge : CommandFlags.None;
				flags |= isClockwise ? CommandFlags.EllipseOrArcIsClockwise : CommandFlags.None;

				AddCommand(new EllipseCommand(flags, d1, radius, angle));

				return this;
			}

			public IPathBuilder BezierTo(Vector2 p1, Vector2 p2, Vector2 p3) {
				AddCommand(new CubicBezierCommand(CommandFlags.None, p1, p2, p3));

				return this;
			}

			public IPathBuilder BezierBy(Vector2 d1, Vector2 d2, Vector2 d3) {
				AddCommand(new CubicBezierCommand(CommandFlags.IsDelta, d1, d2, d3));

				return this;
			}

			public IPathBuilder SmoothBezierTo(Vector2 p2, Vector2 p3) {
				AddCommand(new CubicBezierCommand(CommandFlags.BezierIsSmooth, Vector2.Zero, p2, p3));

				return this;
			}

			public IPathBuilder SmoothBezierBy(Vector2 d2, Vector2 d3) {
				AddCommand(new CubicBezierCommand(CommandFlags.BezierIsSmooth | CommandFlags.IsDelta, Vector2.Zero, d2, d3));

				return this;
			}

			public IPathBuilder BezierTo(Vector2 p1, Vector2 p2) {
				AddCommand(new QuadraticBezierCommand(CommandFlags.None, p1, p2));

				return this;
			}

			public IPathBuilder BezierBy(Vector2 d1, Vector2 d2) {
				AddCommand(new QuadraticBezierCommand(CommandFlags.IsDelta, d1, d2));

				return this;
			}

			public IPathBuilder SmoothBezierTo(Vector2 p2) {
				AddCommand(new QuadraticBezierCommand(CommandFlags.BezierIsSmooth, Vector2.Zero, p2));

				return this;
			}

			public IPathBuilder SmoothBezierBy(Vector2 d2) {
				AddCommand(new QuadraticBezierCommand(CommandFlags.BezierIsSmooth | CommandFlags.IsDelta, Vector2.Zero, d2));

				return this;
			}

			public IPathBuilder Close() {
				AddCommand(new CloseCommand());

				return this;
			}

			public Path Build() {
				for (var i = 0; i < _commands.Count; i++) {
					var command = _commands[i];

					command.Execute(this, ref _start, ref _position, ref _prevPosition);
				}

				if (_contourLength > 0) {
					EndContour(false);
				}

				var vertices = new PathPoint[_points.Count + _contours.Count * 2];
				var p = 0;
				var v = 0;

				foreach (var contour in _contours) {
					// add a start adjacency point

					if (contour.IsClosed) {
						// the start adjacency point is the second to last point in the contour

						vertices[v++] = _points[contour.Offset + contour.Length - 2];
					} else {
						// we have to extrapolate the start adjacency point from the first and second points
						// in the contour

						// TODO: add bounds check?
						var current = _points[p];
						var next = _points[p + 1];

						vertices[v++] = new PathPoint(
							ExtrapolatePoint(next.Position, current.Position, 1)
						);
					}

					do {
						vertices[v++] = _points[p++];
					} while (p < contour.Offset + contour.Length);

					// add an end adjacency point

					if (contour.IsClosed) {
						// the end adjacency point is the second point in the contour

						vertices[v++] = _points[contour.Offset + 1];
					} else {
						// we have to extrapolate the end adjacency point from the second to last and last points
						// in the contour

						var prev = _points[p - 2];
						var current = _points[p];

						vertices[v++] = new PathPoint(
							ExtrapolatePoint(prev.Position, current.Position, 1)
						);
					}
				}

				var gl = _vg._gl;

				var vertexArray = gl.CreateVertexArray();
				var vertexBuffer = gl.CreateBuffer();

				gl.BindVertexArray(vertexArray);
				gl.BindBuffer(BufferTarget.Array, vertexBuffer);
				gl.BufferData(BufferTarget.Array, vertices, BufferUsage.DynamicDraw);

				gl.EnableVertexAttribArray(0);
				gl.VertexAttribPointer(0, 2, DataType.Float, false, PathPoint.SIZE, 0);

				var contours = new List<PathContour>();
				var offset = 0;

				for (var i = 0; i < _contours.Count; i++) {
					var contour = _contours[i];

					contours.Add(new PathContour(vertices, offset, contour.Length + 2, contour.IsClosed));

					offset += contour.Length + 2;
				}

				return new Path(_commands, contours, vertexArray, vertexBuffer);
			}

			public void AddPoint(float x, float y) {
				AddPoint(new Vector2(x, y));
			}

			public void AddPoint(Vector2 p) {
				_points.Add(new PathPoint(p));

				_contourLength++;
				_prevPosition = _position;
				_position = p;
			}

			public void EndContour(bool isClosed) {
				_start = _position;

				_contours.Add(new ContourDef(_contourOffset, _contourLength, isClosed));

				_contourOffset = _points.Count;
				_contourLength = 0;
			}

			private static Vector2 ExtrapolatePoint(Vector2 p0, Vector2 p1, float d) {
				float length = MathF.Sqrt(MathF.Pow(p1.X - p0.X, 2) + MathF.Pow(p1.Y - p0.Y, 2));

				float slopeX = (p1.X - p0.X) / length;
				float slopeY = (p1.Y - p0.Y) / length;

				return new Vector2(
					p1.X + slopeX * d,
					p1.Y + slopeY * d
				);
			}

			private struct ContourDef {
				public readonly int Offset;
				public readonly int Length;
				public readonly bool IsClosed;

				public ContourDef(int offset, int length, bool isClosed) {
					Offset = offset;
					Length = length;
					IsClosed = isClosed;
				}
			}
		}
	}
}

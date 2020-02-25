#version 430
layout (location = 0) in vec2 _position;
layout (location = 1) in vec4 _color;

uniform mat4 _projection;

out vec4 color;

void main() {
	color = _color;
	gl_Position = _projection * vec4(_position.xy, 0.0, 1.0);
}
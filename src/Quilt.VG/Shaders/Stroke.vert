#version 330

layout (location = 0) in vec2 position;

uniform mat4 _projection;

void main() {
	gl_Position = _projection * vec4(position.xy, 0, 1.0);
}
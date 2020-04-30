#version 330

layout (location = 0) in vec2 position;

uniform mat4 _mvp;

void main() {
	gl_Position = _mvp * vec4(position.xy, 0.0, 1.0);
}
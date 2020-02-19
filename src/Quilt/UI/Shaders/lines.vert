#version 330 core
layout (location = 0) in vec2 position;

uniform mat4 u_model, u_view, u_projection;

void main() {
		gl_Position = u_projection * vec4(position.x, position.y, 0, 1.0);
}
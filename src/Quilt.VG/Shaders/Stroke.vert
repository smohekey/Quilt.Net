#version 430
layout (location = 0) in vec2 position;
layout (location = 1) in vec4 color;
layout (location = 2) in float width;

uniform mat4 _projection;

out VS_OUT {
    vec4 color;
		float width;
} vs_out;

void main() {
		vs_out.color = color;
		vs_out.width = width;
		gl_Position = _projection * vec4(position.xy, 1.0, 1.0);
}
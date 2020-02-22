#version 430
layout (location = 0) in vec2 position;
layout (location = 1) in uint flags;
layout (location = 2) in vec4 color;
layout (location = 3) in float width;

uniform mat4 _projection;

out VS_OUT {
	uint flags;
	vec4 color;
	float width;
} vs_out;

void main() {
	vs_out.flags = flags;
	vs_out.color = color;
	vs_out.width = width;
	
	gl_Position = _projection * vec4(position.xy, 0, 1.0);
}
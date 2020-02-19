#version 330
layout (lines) in;
layout (triangle_strip, max_vertices=6) out;

vec2 normal(in vec4 start, in vec4 end) {
	return vec2(-(end.y - start.y), (end.x - start.x));
}

void main() {
	vec2 normal = normal(gl_in[0].gl_Position, gl_in[1].gl_Position);

	gl_Position = gl_in[0].gl_Position;
	EmitVertex();

	gl_Position = gl_in[0].gl_Position + vec4(normal, 0.0f, 0.0f);
	EmitVertex();

	gl_Position = gl_in[1].gl_Position + vec4(normal, 0.0f, 0.0f);
	EmitVertex();

	gl_Position = gl_in[1].gl_Position;
	EmitVertex();

	gl_Position = gl_in[0].gl_Position - vec4(normal, 0.0f, 0.0f);
	EmitVertex();

	gl_Position = gl_in[1].gl_Position - vec4(normal, 0.0f, 0.0f);
	EmitVertex();

	EndPrimitive();
}
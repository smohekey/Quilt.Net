#version 430

layout (lines) in;
layout (triangle_strip, max_vertices = 7) out;

uniform vec2 _viewport;
uniform vec2 _centerPosition;
uniform vec4 _centerColor;

in VS_OUT {
    vec4 color;
} gs_in[];

out vec4 color;

vec2 toScreenSpace(vec4 vertex) {
	return vec2(vertex.xy / vertex.w) * _viewport;
}

void main() {
	vec2 p0 = toScreenSpace(gl_in[0].gl_Position);
	vec2 p1 = toScreenSpace(gl_in[1].gl_Position);
	vec2 c = toScreenSpace(vec4(_centerPosition.xy, 0, 1.0));
	
    // generate the triangle strip
    color = gs_in[0].color;;
    gl_Position = vec4(p0 / _viewport, 0, 1.0);
    EmitVertex();

    color = gs_in[1].color;
    gl_Position = vec4(p1 / _viewport, 0, 1.0);
    EmitVertex();

    color = _centerColor;
    gl_Position = vec4(c / _viewport, 0, 1.0);
    EmitVertex();

    EndPrimitive();
}
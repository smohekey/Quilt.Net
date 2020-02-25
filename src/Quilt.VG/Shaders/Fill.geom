#version 430

layout (lines) in;
layout (triangle_strip, max_vertices = 3) out;

uniform mat4 _projection;
uniform vec2 _centerPosition;
uniform vec4 _centerColor;

in VS_OUT {
	vec4 color;
} gs_in[];

out vec4 color;

void main() {
	vec2 p0 = gl_in[0].gl_Position.xy;
	vec2 p1 = gl_in[1].gl_Position.xy;

	// generate the triangle strip
	color = gs_in[0].color;
	gl_Position = _projection * vec4(p0, 0, 1.0);
	EmitVertex();

	color = gs_in[1].color;
	gl_Position = _projection * vec4(p1, 0, 1.0);
	EmitVertex();

	color = _centerColor;
	gl_Position = _projection * vec4(_centerPosition, 0, 1.0);
	EmitVertex();

	EndPrimitive();

	/*
	vec2 c = toScreenSpace(vec4(_centerPosition.xy, 0, 1.0));
	
    // generate the triangle strip
    color = gs_in[0].color;
    gl_Position = vec4(p0 / _viewport, 0, 1.0);
    EmitVertex();

    color = gs_in[1].color;
    gl_Position = vec4(p1 / _viewport, 0, 1.0);
    EmitVertex();

    color = _centerColor;
    gl_Position = vec4(c / _viewport, 0, 1.0);
    EmitVertex();

    EndPrimitive();
	*/
}
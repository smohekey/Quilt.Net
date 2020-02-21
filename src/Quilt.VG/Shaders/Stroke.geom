#version 430

layout (lines_adjacency) in;
layout (triangle_strip, max_vertices = 7) out;

uniform vec2 _viewport;
uniform float _strokeWidth;
uniform float _miterLimit;
uniform float _alpha;

in VS_OUT {
    vec4 color;
	float width;
} gs_in[];

out vec4 color;

vec2 toScreenSpace(vec4 vertex) {
	return vec2(vertex.xy / vertex.w) * _viewport;
}

void main() {
	vec2 p0 = toScreenSpace(gl_in[0].gl_Position);
	vec2 p1 = toScreenSpace(gl_in[1].gl_Position);
	vec2 p2 = toScreenSpace(gl_in[2].gl_Position);
	vec2 p3 = toScreenSpace(gl_in[3].gl_Position);
	
	vec2 v0 = normalize(p1 - p0);
	vec2 v1 = normalize(p2 - p1);
	vec2 v2 = normalize(p3 - p2);

	vec2 n0 = vec2(-v0.y, v0.x);
	vec2 n1 = vec2(-v1.y, v1.x);
	vec2 n2 = vec2(-v2.y, v2.x);

	vec2 miter_a = normalize(n0 + n1);
	vec2 miter_b = normalize(n1 + n2);

	float an1 = dot(miter_a, n1);
	float bn1 = dot(miter_b, n2);
	if (an1==0) an1 = 1;
	if (bn1==0) bn1 = 1;
	float length_a = gs_in[1].width / an1;
	float length_b = gs_in[2].width / bn1;

	if( dot( v0, v1 ) < -_miterLimit ) {
        miter_a = n1;
        length_a = _strokeWidth;

        /* close the gap */
        if( dot( v0, n1 ) > 0 ) {
			color = gs_in[1].color;
            gl_Position = vec4((p1 + gs_in[1].width * n0) / _viewport, 0, 1.0);
			EmitVertex();

			color = gs_in[1].color;
            gl_Position = vec4((p1 + gs_in[1].width * n1) / _viewport, 0, 1.0);
            EmitVertex();

			color = gs_in[1].color;
            gl_Position = vec4( p1 , 0, 1.0 );
            EmitVertex();

            EndPrimitive();
        } else {
			color = gs_in[1].color;
            gl_Position = vec4( ( p1 - gs_in[1].width * n1 ) / _viewport, 0, 1.0 );
            EmitVertex();

            color = gs_in[1].color;
            gl_Position = vec4( ( p1 - gs_in[1].width * n0 ) / _viewport, 0, 1.0 );
            EmitVertex();

			color = gs_in[1].color;
            gl_Position = vec4( p1 , 0, 1.0 );
            EmitVertex();

            EndPrimitive();
        }
    }

    if(dot(v1, v2) < -_miterLimit) {
        miter_b = n1;
        length_b = gs_in[1].width;
    }

    // generate the triangle strip
    color = gs_in[1].color;;
    gl_Position = vec4( ( p1 + length_a * miter_a ) / _viewport, 0, 1.0 );
    EmitVertex();

    color = gs_in[1].color;
    gl_Position = vec4( ( p1 - length_a * miter_a ) / _viewport, 0, 1.0 );
    EmitVertex();

    color = gs_in[2].color;
    gl_Position = vec4( ( p2 + length_b * miter_b ) / _viewport, 0, 1.0 );
    EmitVertex();

    color = gs_in[2].color;
    gl_Position = vec4( ( p2 - length_b * miter_b ) / _viewport, 0, 1.0 );
    EmitVertex();

    EndPrimitive();
}
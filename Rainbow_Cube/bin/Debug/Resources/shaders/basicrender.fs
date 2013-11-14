#version 330 core
precision highp float;
invariant in vec2 in_UV;
invariant in vec4 in_Color;
invariant in vec4 position;

uniform sampler2D ngl_texture0;


layout(location = 0) out vec4 color;

vec3 hsv2rgb(vec3 c)
{
    vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}


void main()
{
	
	//vec4 col = mix(vec4(1.0, 0.0, 0.0, 1.0), vec4(0.0,1.0,0.0, 1.0), abs(position.x));
	//color = col;
	//vec4 col = vec4(hsv2rgb(vec3(abs(position.x), 1.0, 1.0)), 1.0);
	color = (texture2D( ngl_texture0, in_UV.xy));
}
#version 330 core
precision highp float;
invariant in vec2 in_UV;
invariant in vec4 in_Color;
invariant in vec3 in_position;

uniform sampler2D ngl_texture0;
uniform float time;
uniform vec4 colors[256];
layout(location = 0) out vec4 color;

void main()
{
	vec4 tex_color = texture2D(ngl_texture0, in_UV);
	vec4 color2 = colors[int(floor(tex_color.z))];
	if(tex_color.w == 255. || color2.w == 0.)
	{
		color = tex_color;
	}
	else
	{
		color = color2;
	}
}
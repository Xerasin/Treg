#version 330 core
precision highp float;
invariant in vec2 in_UV;
invariant in vec4 in_Color;
invariant in vec3 in_position;

uniform sampler2D ngl_texture0;
uniform float time;
layout(location = 0) out vec4 color;

void main()
{
	color = vec4(in_UV.x,0.1f, 0.0f, 1.0f);
}
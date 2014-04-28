#version 330 core
precision highp float;
invariant in vec2 in_UV;
invariant in vec4 in_Color;
invariant in vec3 position;

uniform sampler2D ngl_texture0;
uniform sampler2D ngl_texture1;
uniform sampler2D ngl_texture2;
uniform vec3 sunPos;
uniform float time;
layout(location = 0) out vec4 color;

void main()
{
	vec3 posNormal = normalize(position);
	vec3 sunNormal = normalize(sunPos);
	
	float distance = dot(posNormal, sunNormal);
	vec4 skyColor = texture2D( ngl_texture1, vec2((posNormal.y), (sunPos.y + 1.0) / 2.0));
	color = texture2D( ngl_texture0, in_UV.xy) * (1.0 - skyColor.a) + skyColor;
	
}
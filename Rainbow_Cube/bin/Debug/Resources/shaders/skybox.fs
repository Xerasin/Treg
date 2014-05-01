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
float clamp(float a, float b, float c)
{
	if (a > c)
	{
		return c;
	}
	if (a < b)
	{
		return b;
	}
	return a;
}
void main()
{
	vec3 posNormal = normalize(position);
	vec3 sunNormal = normalize(sunPos);
	
	float distance = dot(sunNormal, posNormal);
	
	float y = clamp((sunNormal.y + 1.0) / 2.0, 0.0, 1.0);
	vec4 skyColor = texture2D( ngl_texture1, vec2(posNormal.y, y));
	vec4 sunColor = texture2D( ngl_texture2, vec2(abs(1.0 - distance), 0.5));
	color = vec4(0.0, 0.0, abs(posNormal.x), 1.0); //texture2D( ngl_texture0, in_UV.xy) * (1.0 - skyColor.a) + skyColor + sunColor * sunColor.a;
	
}
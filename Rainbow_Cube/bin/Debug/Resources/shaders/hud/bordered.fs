#version 330 core
const float BorderSize = 64;
precision highp float;
invariant in vec2 in_UV;
invariant in vec4 in_Color;
invariant in vec3 in_position;

uniform sampler2D ngl_texture0;
uniform float time;
uniform float Width;
uniform float Height;
uniform mat4 ModelMatrix;
uniform vec4 colors[256];
layout(location = 0) out vec4 color;
vec2 wrap(vec2 ratio, float width, float height, float X, float Y)
{
	float ratX = (1.0 / width);
	float ratY = (1.0 / height);
	float addX = ratX * X;
	float addY = ratY * Y;
	return vec2(ratio.x / width + addX, ratio.y / height + addY);
	
}
vec2 flip(vec2 pos, bool x, bool y)
{
	if(x)
	{
		pos.x = 1.0 - pos.x;
	}
	if(y)
	{
		pos.y = 1.0 - pos.y;
	}
	return pos;
}
void main()
{
	vec4 tex_color = texture2D(ngl_texture0, in_UV);
	vec3 corner = (ModelMatrix * vec4(1.0, 1.0, 0.0, 1.0)).xyz;
	vec3 corner2 = (ModelMatrix * vec4(0.0, 0.0, 0.0, 1.0)).xyz;
	float distX = corner.x - in_position.x;
	float distY = corner.y - in_position.y;
	float distX2 = in_position.x - corner2.x;
	float distY2 = in_position.y - corner2.y;
	bool xSet = false;
	bool ySet = false;
	vec2 UV = vec2(0.0, 0.0);
	if (distX < BorderSize)
	{
		UV.x = distX / BorderSize;
		xSet = true;
	}
	else if (distX2 < BorderSize)
	{
		UV.x = distX2 / BorderSize;
		xSet = true;
	}
	if(distY < BorderSize)
	{
		UV.y = distY / BorderSize;
		ySet = true;
	}
	else if (distY2 < BorderSize)
	{
		UV.y = distY2 / BorderSize;
		ySet = true;
	}
	if(!xSet || !ySet)
	{
		if(!xSet && !ySet)
		{
			UV = wrap(vec2(0.5, 0.5), 4, 4, 2, 0);
		}
		else if(!xSet)
		{
			UV = wrap(flip(UV, true, true), 4, 4, 0, 0);
		}
		else
		{
			float oldX = UV.x;
			UV.x = UV.y;
			UV.y = oldX;
			UV = wrap(flip(UV, true, true), 4, 4, 0, 0);
		}
	}
	else
	{
		UV = wrap(flip(UV, true, true), 4, 4, 1, 0);
	}
	
	tex_color = texture2D(ngl_texture0, UV);
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
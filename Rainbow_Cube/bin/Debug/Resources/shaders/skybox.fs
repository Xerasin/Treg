#version 330 core
precision highp float;
invariant in vec2 in_UV;
invariant in vec4 in_Color;
invariant in vec3 position;

uniform sampler2D ngl_texture0;
uniform vec3 sunPos;
uniform vec4 skyCols[16][16];
uniform float skyColPoses[16][16];
uniform float skyColPosesY[16];
uniform float skyColNums[16];
uniform float skyColNum;
uniform float time;
layout(location = 0) out vec4 color;

vec4 mixWeighted(vec4 cols[16], float positions[16], float colors, float dif)
{
	if (colors > 1)
	{
		vec4 t;
		for(int I = 1; I < colors; I++)
		{
			float lastPosition = positions[I - 1];
			vec4 lastColor = cols[I - 1];
			float position = positions[I];
			vec4 color = cols[I];
			if(dif > lastPosition && dif <= position)
			{
				float difScaled = (dif - lastPosition) * (1.0 / (position - lastPosition));
				return lastColor + (color - lastColor) * difScaled;
			}
			t = color;
		}
		return color;
	}
	else
	{
		return cols[0];
	}
}

vec4 mixWeighted(vec4 cols[16][16], float positions[16][16], float positions2[16], float colors[16], float rows, float difX, float difY)
{
	if (rows > 1)
	{
		vec4 t;
		for(int Y = 1; Y < rows; Y++)
		{
			float lastPositionY = positions2[Y - 1];
			float positionY = positions2[Y];
			float lastPositions[16] = positions[Y - 1];
			float positions3[16] = positions[Y];
			vec4 lastColors[16] = cols[Y - 1];
			vec4 Colors[16] = cols[Y];
			float colorNumLast = colors[Y - 1];
			float colorNum = colors[Y];
			if (difY > lastPositionY && difY <= positionY)
			{
				vec4 left = mixWeighted(lastColors, lastPositions, colorNumLast, difX);
				vec4 right = mixWeighted(Colors, positions3, colorNum, difX);
				float difScaled = (difY - lastPositionY) * (1.0 / (positionY - lastPositionY));
				t = right;
				return mix(left, right, difScaled);
				
			}
		}
		return t;
	}
	else
	{
		return mixWeighted(cols[0], positions[0], colors[0], difX);
	}
}


void main()
{
	vec3 posNormal = normalize(position);
	vec3 sunNormal = normalize(sunPos);
	
	float distance = dot(posNormal, sunNormal);
	//vec4 skyColor = mix(vec4(1.0, 1.0, 1.0, 1.0), vec4(0.5, 0.5, 1.0, 1.0), (posNormal.y + 1) / 2);
	//vec4 sunColor = mix(vec4(0.0, 0.0, 0.0, 0.0), vec4(0.5, 0, 0.0, 1.0), distance);
	
	//vec4 skyColor = mixWeighted(skyCols, skyColPoses, skyColNum, (posNormal.y + 1.0) / 2.0);
	vec4 skyColor = mixWeighted(skyCols, skyColPoses, skyColPosesY, skyColNums, skyColNum, (posNormal.y), (sunPos.y + 1.0) / 2.0);
	color = texture2D( ngl_texture0, in_UV.xy) * (1.0 - skyColor.a) + vec4(skyColor.rgb, skyColor.a);
	
}
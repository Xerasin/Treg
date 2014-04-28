#version 330 core
precision highp float;
invariant in vec2 in_UV;
invariant in vec4 in_Color;
invariant in vec3 in_position;

uniform sampler2D ngl_texture0;
uniform vec4 Cols[16][16];
uniform float ColPosesX[16][16];
uniform float ColPosesY[16];
uniform float ColNums[16];
uniform float ColNum;
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
			float position = positions[I];
			if(dif > lastPosition && dif <= position)
			{
				vec4 lastColor = cols[I - 1];
				vec4 color = cols[I];
				float difScaled = (dif - lastPosition) * (1.0 / (position - lastPosition));
				t = color;
				return lastColor + (color - lastColor) * difScaled;
			}
			
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
			if (difY > lastPositionY && difY <= positionY)
			{
				float lastPositions[16] = positions[Y - 1];
				float positions3[16] = positions[Y];
				vec4 lastColors[16] = cols[Y - 1];
				vec4 Colors[16] = cols[Y];
				float colorNumLast = colors[Y - 1];
				float colorNum = colors[Y];
				
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
	vec4 skyColor = mixWeighted(Cols, ColPosesX, ColPosesY, ColNums, ColNum, in_position.x, in_position.y);
	color = skyColor; //texture2D( ngl_texture0, in_UV.xy);
	
}
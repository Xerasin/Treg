#version 330 core
const int MAX_GRADIENTS_Y = 8;
const int MAX_GRADIENTS_X = 8;
invariant in vec2 in_UV;
invariant in vec4 in_Color;
invariant in vec3 in_position;

struct Gradient
{
    vec4 Cols[MAX_GRADIENTS_X];
	float PosesX[MAX_GRADIENTS_X];
	float PosY;
	float NumX;
};
uniform sampler2D ngl_texture0;
uniform Gradient grads[MAX_GRADIENTS_Y];
uniform float ColNum;
uniform float time;
layout(location = 0) out vec4 color;

vec4 mixWeighted(vec4 cols[MAX_GRADIENTS_X], float positions[MAX_GRADIENTS_X], float colors, float dif)
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

vec4 mixWeighted(Gradient gradients[MAX_GRADIENTS_Y], float rows, float difX, float difY)
{
	if (rows > 1)
	{
		vec4 t;
		for(int Y = 1; Y < rows; Y++)
		{
			Gradient lastGradient = gradients[Y - 1];
			Gradient cGradient = gradients[Y];
			float lastPositionY = lastGradient.PosY;
			float positionY = cGradient.PosY;
			if (difY > lastPositionY && difY <= positionY)
			{
				float colorNumLast = lastGradient.NumX;
				float colorNum = cGradient.NumX;
				
				vec4 left = mixWeighted(lastGradient.Cols,  lastGradient.PosesX, colorNumLast, difX);
				vec4 right = mixWeighted(cGradient.Cols, cGradient.PosesX, colorNum, difX);
				float difScaled = (difY - lastPositionY) * (1.0 / (positionY - lastPositionY));
				t = right;
				return mix(left, right, difScaled);
				
			}
		}
		return t;
	}
	else
	{
		return mixWeighted(gradients[0].Cols, gradients[0].PosesX, gradients[0].NumX, difX);
	}
}


void main()
{
	vec4 skyColor = mixWeighted(grads, ColNum, in_position.x, in_position.y);
	color = skyColor; //texture2D( ngl_texture0, in_UV.xy);
	
}
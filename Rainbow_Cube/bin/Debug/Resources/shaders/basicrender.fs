#version 330 core
precision highp float;
invariant in vec2 in_UV;
invariant in vec4 in_Color;
invariant in vec4 in_position;
invariant in vec3 in_normal;

uniform sampler2D ngl_texture0;
uniform mat4 ModelMatrix;
uniform mat4 ProjectionMatrix;
uniform mat4 ViewMatrix;
uniform vec3 eyePos;

struct BaseLight
{
        vec3 Color;
        float AmbientIntensity;
        float DiffuseIntensity;
};

struct DirectionalLight
{
        BaseLight Base;
        vec3 Direction;
};

struct Attenuation
{
        float Constant;
        float Linear;
        float Exp;
};

struct PointLight
{
        BaseLight Base;
        vec3 Position;
        Attenuation Atten;
};

struct SpotLight
{
        PointLight Base;
        vec3 Direction;
        float Cutoff;
};

uniform DirectionalLight gEnviromentalLight;	
layout(location = 0) out vec4 color;



vec3 hsv2rgb(vec3 c)
{
    vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

vec4 calcLight(BaseLight Light, vec3 Dir, vec3 Normal)
{
	vec4 AmbientColor = vec4(Light.Color, 1.0f) * Light.AmbientIntensity;  
	vec3 LightColor = Light.Color;
	
	vec3 n = normalize( Normal );
	vec3 l = normalize( Dir );
	
	float cosTheta = dot( n, l );
	vec4 DiffuseColor  = vec4(0, 0, 0, 0); 
	vec4 SpecularColor = vec4(0, 0, 0, 0);
	
	if(cosTheta > 0)
	{
		DiffuseColor = vec4(LightColor, 1) * Light.DiffuseIntensity * cosTheta;
		
		vec3 difference = normalize(eyePos - in_position.xyz);
		vec3 reflect = normalize(reflect(-Dir, Normal));
		float specFactor = dot(difference, reflect);
		if(specFactor > 0)
		{
			SpecularColor = vec4(Light.Color, 1.0f) * pow(specFactor, 5) * 0.1;  
		}
	}
	return AmbientColor + (DiffuseColor + SpecularColor);
}

vec4 CalcEnviromentalLighting( vec3 Normal )
{
	return calcLight(gEnviromentalLight.Base, gEnviromentalLight.Direction, Normal);
}

void main()
{
	
	//vec4 col = mix(vec4(1.0, 0.0, 0.0, 1.0), vec4(0.0,1.0,0.0, 1.0), abs(position.x));
	//color = col;
	//vec4 col = vec4(hsv2rgb(vec3(abs(position.x), 1.0, 1.0)), 1.0);
	vec4 LightCol = CalcEnviromentalLighting( in_normal );
	color = (texture2D( ngl_texture0, in_UV.xy)) * LightCol;
}
#version 330 core
precision highp float;
layout( location = 0 ) in vec3 in_Position;
layout( location = 1 ) in vec3 Normal;
layout( location = 2 ) in vec2 vertexUV;
layout( location = 3 ) in vec4 dgl_Color;
uniform mat4 ModelMatrix;
uniform mat4 ProjectionMatrix;
uniform mat4 ViewMatrix;
uniform float time;
invariant out vec4 in_Color;
invariant out vec3 in_position;
invariant out vec2 in_UV;
invariant out vec3 in_normal;
void main() 
{
	vec4 pos = vec4( in_Position.x, in_Position.y, in_Position.z, 1.0);
    gl_Position = (ProjectionMatrix * (ViewMatrix * ModelMatrix)) * pos;
	in_position = (ModelMatrix * pos).xyz;
	in_Color = dgl_Color;
	in_normal = (ModelMatrix * vec4(Normal, 0.0)).xyz;
	in_UV = vertexUV;
}
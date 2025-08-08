#version 330 core
layout (location = 0) in vec4 vertices;
layout (location = 1) in vec2 coords;

out vec2 TexCoord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 proj;

void main()
{
	gl_Position = vertices * model * view * proj;
	TexCoord = coords;
}
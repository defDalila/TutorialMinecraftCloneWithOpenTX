#version 330 core
layout (location = 0) in vec3 vertices;
layout (location = 1) in vec2 coords;

out vec2 TexCoord;

void main()
{
	gl_Position = vec4(vertices, 1.0f);
	TexCoord = coords;
}
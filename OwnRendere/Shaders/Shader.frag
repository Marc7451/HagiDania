#version 330 core

out vec4 FragColor;
in vec3 v2fColor;

void main()
{
	FragColor = vec4(v2fColor, 1);
}
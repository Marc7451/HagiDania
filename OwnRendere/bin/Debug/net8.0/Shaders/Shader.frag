#version 330 core
out vec4 outputColor; // Output color of the fragment

in vec2 texCoord; // Input from vertex shader

uniform sampler2D texture0;
uniform sampler2D texture1;

void main()
{
    // Mix two textures with a 60% blend from texture1
    outputColor = mix(texture(texture0, texCoord), texture(texture1, texCoord), 0.6);
}
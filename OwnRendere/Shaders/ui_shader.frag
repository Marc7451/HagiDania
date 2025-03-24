#version 330 core
out vec4 FragColor;

in vec2 texCoord;

uniform sampler2D uiTexture; // The texture sampler

void main()
{
    FragColor = texture(uiTexture, texCoord); // Sample the texture
}

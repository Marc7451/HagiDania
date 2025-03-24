#version 330 core

in vec2 TexCoords;
out vec4 FragColor;

uniform sampler2D texture0;  // UI Texture

void main()
{
    vec4 texColor = texture(texture0, TexCoords);
    
    // Fjern transparente pixels
    if (texColor.a < 0.1)
        discard;
    
    FragColor = texColor;
}
#version 330 core
out vec4 outputColor; // Output color of the fragment

in vec2 texCoord; // Input from vertex shader

uniform sampler2D texture0;
uniform sampler2D texture1;
uniform float time;

void main()
{
    vec2 movingTexCoord = texCoord;
    movingTexCoord.x += time * 0.3; 

    vec4 color0 = texture(texture0, texCoord); // Destination
    vec4 color1 = texture(texture1, movingTexCoord); // Source
    
    float alpha = color1.a;
    outputColor = color1 * alpha + color0 * (1.0 - alpha);
}
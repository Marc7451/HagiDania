#version 330 core
out vec4 outputColor; // Output color of the fragment

in vec2 texCoord; // Input from vertex shader

in vec3 vColor;
out vec4 CylinderColor;

uniform sampler2D texture0;
uniform sampler2D texture1;
uniform float time;

float speed = 0.3f;
vec2 pivot = vec2(0.5, 0.5);

void main()
{

    CylinderColor = vec4(vColor, 1.0);
    float angle = time * speed; // Rotation over tid
    mat2 rotationMatrix = mat2(
        cos(angle), -sin(angle),
        sin(angle),  cos(angle)
    );
    
    vec2 movingTexCoord = rotationMatrix * texCoord;

    vec4 color0 = texture(texture0, texCoord); // Destination
    vec4 color1 = texture(texture1, movingTexCoord); // Source
    
    float alpha = color1.a;
    outputColor = color1 * alpha + color0 * (1.0 - alpha);
}
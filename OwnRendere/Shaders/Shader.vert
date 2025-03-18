#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 aTexCoord;

out vec2 texCoord; // This passes the texture coordinates to the fragment shader

uniform vec2 offset;

void main()
{    
    texCoord = aTexCoord; // Pass texture coordinates to fragment shader
    gl_Position = vec4(aPos.x + offset.x, aPos.y + offset.y, aPos.z, 1.0);
}
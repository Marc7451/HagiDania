#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 aTexCoord;

out vec2 texCoord;
uniform mat4 mvp;

void main()
{    
    texCoord = aTexCoord; // Pass texture coordinates to fragment shader
    gl_Position = vec4(aPos, 1.0) * mvp;
}
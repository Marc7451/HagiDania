#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 aTexCoord;

layout (location = 2) in vec3 aPosition; 
layout (location = 3) in vec3 aColor;  

out vec3 vColor;

out vec2 texCoord; // This passes the texture coordinates to the fragment shader
uniform mat4 mvp;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{    
    texCoord = aTexCoord; // Pass texture coordinates to fragment shader
    gl_Position = vec4(aPos, 1.0) * mvp;

    gl_Position = projection * view * model * vec4(aPosition, 1.0);
    vColor = aColor;
}
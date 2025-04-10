﻿using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using static System.Net.Mime.MediaTypeNames;

namespace OwnRendere
{
    public class Material
    {
        private Shader shader;
        private Dictionary<string, object> uniforms = new Dictionary<string, object>();
        private Dictionary<int, Texture> textures = new Dictionary<int, Texture>();

        private bool isUI;
        public Vector2 spriteSize { get; private set; }

        public Material(string vertPath, string fragPath, Dictionary<string, object> uniforms, bool isUI = false)
        {
            this.isUI = isUI;
            shader = new Shader(vertPath, fragPath);

            foreach (KeyValuePair<string, object> uniform in uniforms)
            {
                SetUniform(uniform.Key, uniform.Value);
            }

            foreach (var uniform in uniforms.Values)
            {
                if (uniform is Texture tex)
                {
                    spriteSize = new Vector2(tex.Width, tex.Height) * tex.PixelScale;
                    break;
                }
            }
        }

        public Material(string vertPath, string fragPath, string name, Texture texture, bool isUI = false)
        {
            this.isUI = isUI;
            shader = new Shader(vertPath, fragPath);

            SetUniform(name, texture);
            spriteSize = new Vector2(texture.Width, texture.Height) * texture.PixelScale;
        }

        public void SetUniform(string name, object uniform)
        {
            shader.Use();

            if (uniform is int uniformInt)
            {
                shader.SetInt(name, uniformInt);
            }
            else if (uniform is float uniformFloat)
            {
                shader.SetFloat(name, uniformFloat);
            }
            else if (uniform is Matrix4 uniformMatrix)
            {
                shader.SetMatrix(name, uniformMatrix);
            }
            else if (uniform is Vector4 uniformVector4)
            {
                shader.SetVector4(name, uniformVector4);
            }
            else if (uniform is Texture tex)
            {
                int addedTextures = textures.Count;
                shader.SetInt(name, addedTextures);
                textures.Add(addedTextures, tex);
            }
            else
            {
                Console.WriteLine($"Unsupported shader uniform type: {uniform.GetType()}");
                return;
            }

            uniforms[name] = uniform;
        }

        public void UseShader()
        {
            if (isUI)
            {
                // UI kræver blending for gennemsigtighed
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            }
            else
            {
                // Normal rendering slår blending fra
                GL.Disable(EnableCap.Blend);
            }

            foreach (KeyValuePair<int, Texture> texWithIndex in textures)
            {
                texWithIndex.Value.Use(TextureUnit.Texture0 + texWithIndex.Key);
            }

            shader.Use();
        }

    }
}

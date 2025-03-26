using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace OwnRendere
{
    public class Shader
    {
        public int handle;
        private bool disposedValue = false;

        public Shader(string vertexPath, string fragmentPath)
        {
            string vertexSource = File.ReadAllText(vertexPath);
            string fragmentSource = File.ReadAllText(fragmentPath);

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexSource);
            CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentSource);
            CompileShader(fragmentShader);

            // Opret program og link shaderne
            handle = GL.CreateProgram();
            GL.AttachShader(handle, vertexShader);
            GL.AttachShader(handle, fragmentShader);
            LinkProgram(handle);

            // Slet shaders efter de er linket
            GL.DetachShader(handle, vertexShader);
            GL.DetachShader(handle, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        private void CompileShader(int shader)
        {
            GL.CompileShader(shader);
            GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
            if (code != (int)All.True)
            {
                string infoLog = GL.GetShaderInfoLog(shader);
                Console.WriteLine($"Shader Compilation Error ({shader}):\n{infoLog}");
                throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
            }
        }

        private void LinkProgram(int program)
        {
            GL.LinkProgram(program);
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
            if (code != (int)All.True)
            {
                string infoLog = GL.GetProgramInfoLog(program);
                Console.WriteLine($"Shader Linking Error:\n{infoLog}");
                throw new Exception($"Error occurred whilst linking Program({program}):\n{infoLog}");
            }
        }

        public void Use()
        {
            GL.UseProgram(handle);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(handle);
                disposedValue = true;
            }
        }

        ~Shader()
        {
            if (!disposedValue)
            {
                Console.WriteLine("GPU Resource leak! call Dispose() ?");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(handle, attribName);
        }

        private int GetUniformLocation(string name)
        {
            int location = GL.GetUniformLocation(handle, name);
            if (location == -1)
            {
                Console.WriteLine($"Warning: Uniform '{name}' not found in shader.");
            }
            return location;
        }

        public void SetInt(string name, int value)
        {
            int location = GetUniformLocation(name);
            if (location != -1)
                GL.Uniform1(location, value);
        }

        public void SetFloat(string name, float value)
        {
            int location = GetUniformLocation(name);
            if (location != -1)
                GL.Uniform1(location, value);
        }

        public void SetMatrix(string name, Matrix4 transform)
        {
            int location = GetUniformLocation(name);
            if (location != -1)
                GL.UniformMatrix4(location, true, ref transform);
        }

        public void SetVector3(string name, Vector3 vec)
        {
            int location = GetUniformLocation(name);
            if (location != -1)
                GL.Uniform3(location, vec);
        }

        public void SetVector4(string name, Vector4 vec)
        {
            int location = GetUniformLocation(name);
            if (location != -1)
                GL.Uniform4(location, vec);
        }
    }
}

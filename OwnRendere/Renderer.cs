using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnRendere
{
    public class Renderer
    {
        public Material material;
        Mesh mesh;
        public Renderer(Material material, Mesh mesh)
        {
            this.material = material;
            this.mesh = mesh;
        }
        public void Draw(Matrix4 mvp)
        {
            material.UseShader();
            material.SetUniform("mvp", mvp);
            mesh.Draw();
        }
    }
}

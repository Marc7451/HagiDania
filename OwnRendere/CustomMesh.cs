using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnRendere
{

    internal class CustomMesh : Mesh
    {
        public CustomMesh(float[] vertices, uint[] indices)
        {
            this.vertices = vertices;
            this.indices = indices;
            GenerateBuffers();
        }

    }
}

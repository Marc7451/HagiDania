using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnRendere
{
    public class TriangleMesh : Mesh
    {
        public override float[] vertices { get; set; } =
{
            // Position         // Texture
            0.0f,  0.5f, 0.0f,  0.5f, 1.0f, // Top
           -0.5f, -0.5f, 0.0f,  0.0f, 0.0f, // Bottom left
            0.5f, -0.5f, 0.0f,  1.0f, 0.0f  // Bottom right
        };

        public override uint[] indices { get; set; } =
        {
            0, 1, 2 // Én trekant
        };

        public TriangleMesh()
        {
            GenerateBuffers();
        }
    }
}

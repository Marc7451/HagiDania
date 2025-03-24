using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnRendere.Primitive
{
    public class UI_Plane : Mesh
    {
        protected override float[] vertices { get; set; } =
        {
            // Positions (skaleret til UI-space) // Texture Coords
            0.0f,  0.0f,  0.0f,  0.0f, 0.0f,
            1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
            1.0f,  1.0f,  0.0f,  1.0f, 1.0f,
            0.0f,  1.0f,  0.0f,  0.0f, 1.0f,
        };

        protected override uint[] indices { get; set; } =
        {
            0, 1, 2,  2, 3, 0,
        };
    }
}

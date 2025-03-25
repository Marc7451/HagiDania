using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnRendere
{
    class CylinderMesh : Mesh
    {
        private const int Segments = 36; // Antal segmenter rundt om cylinderen
        private const float Radius = 0.5f;
        private const float Height = 1.0f;

        public CylinderMesh()
        {
            GenerateCylinder();  // Opret vertices & indices
            GenerateBuffers();   // Brug Mesh-metoden til at loade bufferne
        }

        private void GenerateCylinder()
        {
            List<float> verts = new List<float>();
            List<uint> inds = new List<uint>();

            uint topCenterIndex = 0;
            uint bottomCenterIndex = 1;

            // Tilføj centerpunkter til top og bund
            verts.AddRange(new float[] { 0f, Height / 2, 0f, 0.5f, 1.0f }); // Top center
            verts.AddRange(new float[] { 0f, -Height / 2, 0f, 0.5f, 0.0f }); // Bottom center

            // Tilføj top- og bundcirkler samt side
            for (int i = 0; i <= Segments; i++)
            {
                float angle = i * MathHelper.TwoPi / Segments;
                float x = MathF.Cos(angle) * Radius;
                float z = MathF.Sin(angle) * Radius;
                float u = i / (float)Segments;

                // Top punkt
                verts.AddRange(new float[] { x, Height / 2, z, u, 1.0f });

                // Bund punkt
                verts.AddRange(new float[] { x, -Height / 2, z, u, 0.0f });
            }

            // Sideflader (trekanter)
            for (uint i = 2; i < (Segments + 1) * 2; i += 2)
            {
                inds.Add(i);
                inds.Add(i + 1);
                inds.Add(i + 2);

                inds.Add(i + 1);
                inds.Add(i + 3);
                inds.Add(i + 2);
            }

            // Top
            for (uint i = 2; i < Segments * 2; i += 2)
            {
                inds.Add(topCenterIndex);
                inds.Add(i);
                inds.Add(i + 2);
            }

            //  Bund
            for (uint i = 3; i < Segments * 2; i += 2)
            {
                inds.Add(bottomCenterIndex);
                inds.Add(i + 2);
                inds.Add(i);
            }

            vertices = verts.ToArray();
            indices = inds.ToArray();
        }
    }
}

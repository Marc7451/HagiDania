using OpenTK.Mathematics;

namespace OwnRendere
{

    class CylinderMesh : Mesh
    {
        private const int Segments = 36; // Antal segmenter rundt om cylinderen
        private const float Radius = 0.5f;
        private const float Height = 1.0f;

        public CylinderMesh()
        {
            GenerateCylinder(Segments, Height, Radius);  // Opret vertices & indices
            GenerateBuffers();   // Brug Mesh-metoden til at loade bufferne
            
        }

        private void GenerateCylinder(int segments, float height, float radius)
        {
            List<float> verts = new List<float>();
            List<uint> inds = new List<uint>();

            float angleStep = 2.0f * MathF.PI / segments;

            int centerBottom = 0;
            int centerTop = segments + 1;

            // 🌟 Bund og top centerpunkt
            verts.Add(0.0f); verts.Add(-height / 2); verts.Add(0.0f); verts.Add(0.5f); verts.Add(0.5f); // Bund midt
            verts.Add(0.0f); verts.Add(height / 2); verts.Add(0.0f); verts.Add(0.5f); verts.Add(0.5f); // Top midt

            // 🟠 Generer bund- og toppunkter
            for (int i = 0; i < segments; i++)
            {
                float angle = i * angleStep;
                float x = MathF.Cos(angle) * radius;
                float z = MathF.Sin(angle) * radius;
                float u = (x / radius + 1) * 0.5f;
                float v = (z / radius + 1) * 0.5f;

                // Bund cirkel (y = -height / 2)
                verts.Add(x); verts.Add(-height / 2); verts.Add(z); verts.Add(u); verts.Add(v);

                // Top cirkel (y = height / 2)
                verts.Add(x); verts.Add(height / 2); verts.Add(z); verts.Add(u); verts.Add(v);
            }

            // 🔺 Indices til bund
            for (uint i = 0; i < segments; i++)
            {
                uint next = (i + 1) % (uint)segments;
                inds.Add((uint)centerBottom);
                inds.Add((uint)(i + 2));
                inds.Add((uint)(next + 2));
            }

            // 🔺 Indices til top
            for (uint i = 0; i < segments; i++)
            {
                uint next = (i + 1) % (uint)segments;
                inds.Add((uint)centerTop);
                inds.Add((uint)(next + 2 + segments));
                inds.Add((uint)(i + 2 + segments));
            }

            // 🔷 Indices til siderne (trekanter)
            for (uint i = 0; i < segments; i++)
            {
                uint next = (i + 1) % (uint)segments;
                uint bot1 = i + 2;
                uint bot2 = next + 2;
                uint top1 = bot1 + (uint)segments;
                uint top2 = bot2 + (uint)segments;

                inds.Add(bot1); inds.Add(top1); inds.Add(bot2);
                inds.Add(bot2); inds.Add(top1); inds.Add(top2);
            }

            vertices = verts.ToArray();
            indices = inds.ToArray();
        }
    }
}


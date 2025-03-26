using OpenTK.Mathematics;

namespace OwnRendere
{

    class CylinderMesh : Mesh
    {
        public CylinderMesh(int segments, float height, float radius)
        {
            GenerateCylinder(segments, height, radius);
            GenerateBuffers();
        }

        private void GenerateCylinder(int segments, float height, float radius)
        {
            List<float> verts = new List<float>();
            List<uint> inds = new List<uint>();

            float angleStep = 2.0f * MathF.PI / segments;

            int vertexOffset = 0;

            // 🟠 Bund og top cirkler
            for (int i = 0; i < 2; i++)
            {
                float y = (i == 0) ? -height / 2 : height / 2; // Bund eller top

                // Center vertex for cirklen
                verts.Add(0.0f); verts.Add(y); verts.Add(0.0f);
                verts.Add(0.5f); verts.Add(0.5f); // UV

                for (int j = 0; j < segments; j++)
                {
                    float angle = j * angleStep;
                    float x = MathF.Cos(angle) * radius;
                    float z = MathF.Sin(angle) * radius;
                    float u = (x / radius + 1) * 0.5f;
                    float v = (z / radius + 1) * 0.5f;

                    verts.Add(x); verts.Add(y); verts.Add(z);
                    verts.Add(u); verts.Add(v);
                }
            }

            // 🔺 Indices til bund og top
            for (uint i = 0; i < segments; i++)
            {
                uint next = (i + 1) % (uint)segments;
                inds.Add(0);
                inds.Add(i + 1);
                inds.Add(next + 1);

                uint topCenter = (uint)(segments + 1);
                inds.Add(topCenter);
                inds.Add(topCenter + next + 1);
                inds.Add(topCenter + i + 1);
            }

            vertexOffset = (segments + 1) * 2;

            // 🔷 Sideflader (To trekanter per segment)
            for (uint i = 0; i < segments; i++)
            {
                float angle = i * angleStep;
                float x = MathF.Cos(angle) * radius;
                float z = MathF.Sin(angle) * radius;
                float u = i / (float)segments;

                float nextAngle = (i + 1) * angleStep;
                float nextX = MathF.Cos(nextAngle) * radius;
                float nextZ = MathF.Sin(nextAngle) * radius;
                float nextU = (i + 1) / (float)segments;

                verts.Add(x); verts.Add(-height / 2); verts.Add(z);
                verts.Add(u); verts.Add(0.0f);
                verts.Add(x); verts.Add(height / 2); verts.Add(z);
                verts.Add(u); verts.Add(1.0f);
                verts.Add(nextX); verts.Add(-height / 2); verts.Add(nextZ);
                verts.Add(nextU); verts.Add(0.0f);
                verts.Add(nextX); verts.Add(height / 2); verts.Add(nextZ);
                verts.Add(nextU); verts.Add(1.0f);

                uint baseIdx = (uint)vertexOffset + i * 4;
                inds.Add(baseIdx);
                inds.Add(baseIdx + 1);
                inds.Add(baseIdx + 2);
                inds.Add(baseIdx + 2);
                inds.Add(baseIdx + 1);
                inds.Add(baseIdx + 3);
            }

            vertices = verts.ToArray();
            indices = inds.ToArray();
        }
    }
}


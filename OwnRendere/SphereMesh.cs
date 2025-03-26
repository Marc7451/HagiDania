using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnRendere
{
    internal class SphereMesh : Mesh
    {
        public SphereMesh(int latitudeBands, int longitudeBands, float radius)
        {
            GenerateSphere(latitudeBands, longitudeBands, radius);
            GenerateBuffers();
        }

        private void GenerateSphere(int latitudeBands, int longitudeBands, float radius)
        {
            List<float> verts = new List<float>();
            List<uint> inds = new List<uint>();

            // 🔵 Generér vertices
            for (int lat = 0; lat <= latitudeBands; lat++)
            {
                float theta = lat * MathF.PI / latitudeBands;
                float sinTheta = MathF.Sin(theta);
                float cosTheta = MathF.Cos(theta);

                for (int lon = 0; lon <= longitudeBands; lon++)
                {
                    float phi = lon * 2.0f * MathF.PI / longitudeBands;
                    float sinPhi = MathF.Sin(phi);
                    float cosPhi = MathF.Cos(phi);

                    float x = cosPhi * sinTheta;
                    float y = cosTheta;
                    float z = sinPhi * sinTheta;
                    float u = 1.0f - (float)lon / longitudeBands;
                    float v = 1.0f - (float)lat / latitudeBands;

                    verts.Add(x * radius);
                    verts.Add(y * radius);
                    verts.Add(z * radius);
                    verts.Add(u);
                    verts.Add(v);
                }
            }

            // 🔺 Generér indices
            for (int lat = 0; lat < latitudeBands; lat++)
            {
                for (int lon = 0; lon < longitudeBands; lon++)
                {
                    uint first = (uint)(lat * (longitudeBands + 1) + lon);
                    uint second = first + (uint)longitudeBands + 1;

                    inds.Add(first);
                    inds.Add(second);
                    inds.Add(first + 1);

                    inds.Add(second);
                    inds.Add(second + 1);
                    inds.Add(first + 1);
                }
            }

            vertices = verts.ToArray();
            indices = inds.ToArray();
        }
    }
}

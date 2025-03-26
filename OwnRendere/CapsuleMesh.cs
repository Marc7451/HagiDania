using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnRendere
{
    internal class CapsuleMesh : Mesh
    {
        public CapsuleMesh(int segments, int latitudeBands, int longitudeBands, float height, float radius)
        {
            GenerateCapsule(segments, latitudeBands, longitudeBands, height, radius);
            GenerateBuffers();
        }

        private void GenerateCapsule(int segments, int latitudeBands, int longitudeBands, float height, float radius)
        {
            List<float> verts = new List<float>();
            List<uint> inds = new List<uint>();

            CylinderMesh cylinder = new CylinderMesh(segments, height, radius);
            SphereMesh topSphere = new SphereMesh(latitudeBands, longitudeBands, radius);
            SphereMesh bottomSphere = new SphereMesh(latitudeBands, longitudeBands, radius);

            // Flyt den øvre halvkugle opad, så dens bund starter ved cylinderens top
            ShiftVertices(topSphere.vertices, 0, height / 2, 0);

            //  Flyt den nedre halvkugle nedad, så dens top starter præcis ved cylinderens bund
            ShiftVertices(bottomSphere.vertices, 0, -height / 2, 0, true);

            //  Kombiner alle vertices
            int cylinderOffset = verts.Count / 5;
            verts.AddRange(cylinder.vertices);

            int topSphereOffset = verts.Count / 5;
            verts.AddRange(topSphere.vertices);

            int bottomSphereOffset = verts.Count / 5;
            verts.AddRange(bottomSphere.vertices);

            // Kombiner alle indices
            foreach (var index in cylinder.indices)
                inds.Add(index + (uint)cylinderOffset);

            foreach (var index in topSphere.indices)
                inds.Add(index + (uint)topSphereOffset);

            foreach (var index in bottomSphere.indices)
                inds.Add(index + (uint)bottomSphereOffset);

            vertices = verts.ToArray();
            indices = inds.ToArray();
        }

        private void ShiftVertices(float[] verts, float x, float y, float z, bool invertY = false)
        {
            for (int i = 0; i < verts.Length; i += 5)
            {
                verts[i] += x;
                verts[i + 1] = invertY ? -verts[i + 1] : verts[i + 1] + y;
                verts[i + 2] += z;
            }
        }
    }
    }

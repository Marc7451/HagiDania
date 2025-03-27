using OpenTK.Graphics.OpenGL4;

namespace OwnRendere
{
    public class Mesh
    {
        protected int vertexArrayObject;
        protected int elementBufferObject;
        protected int vertexBufferObject;
        public virtual float[] vertices { get; set; }
        public virtual uint[] indices { get; set; }

        public Mesh()
        {
            //GenerateBuffers(); //setup based on vertices, indices, etc
        }
        
        protected virtual void GenerateBuffers()
        {
            // 1. Opret og bind VAO
            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            // 2. Opret og bind VBO
            vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // 3. Opret og bind EBO
            elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            // 4. Definer vertex-attributterne (Position og Texture coordinates)
            int stride = 5 * sizeof(float); // 5 floats per vertex (3 pos + 2 tex)

            // Position (3 floats)
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);
            GL.EnableVertexAttribArray(0);

            // Texture coordinates (2 floats)
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // Unbind for at undgå utilsigtede ændringer
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }


        public virtual void Draw()
        {
            GL.BindVertexArray(vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        }
    }
}

namespace OwnRendere.Shapes
{
    public class PlaneMesh : Mesh
    {
        protected override float[] vertices { get; set; } =
        {
            // Positions         // Texture Coords
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
        };

        protected override uint[] indices { get; set; } =
        {
            0, 1, 2,  2, 3, 0,
        };

        public PlaneMesh()
        {
            GenerateBuffers();
        }
    }
}

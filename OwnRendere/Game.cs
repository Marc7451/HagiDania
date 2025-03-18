using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OwnRendere
{
    public class Game : GameWindow
    {
        private Color4 backgroundColor = new Color4(0.2f, 0.1f, 0.1f, 1.0f);
        private bool isFullscreen = false;

        // Change color
        float greenValue = 1;
        bool increaseGreen = false;

        // Change position
        Vector2 position = new Vector2(0, 0);
        float speed = .01f;

        private int vertexArrayObject;
        private int vertexBufferObject;
        private Shader shader;

        private readonly float[] verticesTriangleWithCol =
        {
            // positions         // colors
             0.5f, -0.5f, 0.0f,  1.0f, 0.0f, 0.0f, // bottom right
            -0.5f, -0.5f, 0.0f,  0.0f, 1.0f, 0.0f, // bottom left
             0.0f,  0.5f, 0.0f,  0.0f, 0.0f, 1.0f  // top center
        };

        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            GL.ClearColor(backgroundColor);
            Size = new Vector2i(800, 800);
        }


        protected override void OnLoad()
        {
            base.OnLoad();

            // Send data til OpenGL
            vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, verticesTriangleWithCol.Length * sizeof(float), verticesTriangleWithCol, BufferUsageHint.StaticDraw);

            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            // Position attribute (3 floats per vertex)
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Color attribute (3 floats per vertex)
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            shader.Use();

            // Send uniform color til shader
            int vertexColorLocation = GL.GetUniformLocation(shader.Handle, "aColor");
            GL.Uniform4(vertexColorLocation, 0.0f, greenValue, 0.0f, 1.0f);

            GL.BindVertexArray(vertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, verticesTriangleWithCol.Length / 6);

            GL.ClearColor(backgroundColor);
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            KeyboardState input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            if (input.IsKeyDown(Keys.W))
            {
                if (backgroundColor.R < 1)
                    backgroundColor.R += .01f;
            }

            if (input.IsKeyDown(Keys.F))
            {
                ToggleFullscreen();
            }

            if (input.IsKeyDown(Keys.E))
            {
                if (increaseGreen)
                {
                    if (greenValue < 1)
                        greenValue += .1f;
                    else
                        increaseGreen = false;
                }
                else
                {
                    if (greenValue > 0)
                        greenValue -= .1f;
                    else
                        increaseGreen = true;
                }
            }

            if (input.IsKeyDown(Keys.D))
                position.X += speed;
            if (input.IsKeyDown(Keys.A))
                position.X -= speed;

            shader.Use();
            int positionLocation = GL.GetUniformLocation(shader.Handle, "offset");
            GL.Uniform2(positionLocation, position);
        }

        private void ToggleFullscreen()
        {
            if (isFullscreen)
            {
                WindowState = WindowState.Normal;
                Size = new Vector2i(800, 800);
            }
            else
            {
                WindowState = WindowState.Fullscreen;
            }

            isFullscreen = !isFullscreen;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }
    }
}

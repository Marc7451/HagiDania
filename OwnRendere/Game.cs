using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;

namespace OwnRendere
{
    public class Game : GameWindow
    {
        private Color4 backgroundColor = new Color4(0.2f, 0.1f, 0.1f, 1.0f);
        private bool isFullscreen = false;

        // Position og hastighed
        Vector2 position = new Vector2(0, 0);
        float speed = .01f;

        private int vertexArrayObject;
        private int vertexBufferObject;
        private int elementBufferObject;
        private Shader shader;
        private Texture texture0;
        private Texture texture1;

        //moveing texture
        Stopwatch timer = new Stopwatch();

        float[] vertices =
        {
            // Position        // Texture coords
             0.5f,  0.5f, 0.0f,  1.0f, 1.0f, // Top Right
             0.5f, -0.5f, 0.0f,  1.0f, 0.0f, // Bottom Right
            -0.5f, -0.5f, 0.0f,  0.0f, 0.0f, // Bottom Left
            -0.5f,  0.5f, 0.0f,  0.0f, 1.0f  // Top Left
        };

        uint[] indices =
        {
            0, 1, 3, // Første trekant
            1, 2, 3  // Anden trekant
        };

        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            GL.ClearColor(backgroundColor);
            Size = new Vector2i(800, 800);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            timer.Start();

            // Indlæs tekstur
            texture0 = new Texture("Textures/wall.jpg");
            texture1 = new Texture("Textures/AragonTexUdenBaggrund.png");

            // Opret VBO (Vertex Buffer Object)
            vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // Opret VAO (Vertex Array Object)
            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            // Opret EBO (Element Buffer Object) for at bruge `indices`
            elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            // Position (layout = 0)
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Texture koordinater (layout = 1)
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);



            // Indlæs shaders
            shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
            shader.Use();
            shader.SetInt("texture0", 0);
            shader.SetInt("texture1", 1);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.BindVertexArray(vertexArrayObject);
            texture0.Use(TextureUnit.Texture0);
            texture1.Use(TextureUnit.Texture1);
            shader.Use();
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            float time = (float)timer.Elapsed.TotalSeconds;
            int timeLocation = GL.GetUniformLocation(shader.handle, "time");
            GL.Uniform1(timeLocation, time);

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

            if (input.IsKeyDown(Keys.F))
            {
                ToggleFullscreen();
            }

            if (input.IsKeyDown(Keys.D))
                position.X += speed;
            if (input.IsKeyDown(Keys.A))
                position.X -= speed;

            shader.Use();
            int positionLocation = GL.GetUniformLocation(shader.handle, "offset");
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

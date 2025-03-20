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

        private int vertexArrayObject;
        private int vertexBufferObject;
        private int elementBufferObject;
        private Shader shader;
        private Texture texture0;
        private Texture texture1;

        //Cam
        private Vector3 cameraPosition = new Vector3(0.0f, 0.0f, 3.0f);
        private Vector3 cameraFront = new Vector3(0.0f, 0.0f, -1.0f);
        private Vector3 cameraUp = new Vector3(0.0f, 1.0f, 0.0f);
        private float cameraSpeed = 0.1f;
        //Tilføj en yaw-værdi til at holde styr på rotationen
        private float yaw = -90.0f; // Startværdien sikrer at vi kigger langs -Z-aksen
        private float rotationSpeed = 2.0f; // Justér rotationshastigheden

        //moveing texture
        Stopwatch timer = new Stopwatch();

        float[] vertices =
        {
            -0.5f, -0.5f, -0.5f, 0.0f, 0.0f,
            0.5f, -0.5f, -0.5f, 1.0f, 0.0f,
            0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
            0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
            -0.5f, 0.5f, -0.5f, 0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f, 0.0f, 0.0f,
            -0.5f, -0.5f, 0.5f, 0.0f, 0.0f,
            0.5f, -0.5f, 0.5f, 1.0f, 0.0f,
            0.5f, 0.5f, 0.5f, 1.0f, 1.0f,
            0.5f, 0.5f, 0.5f, 1.0f, 1.0f,
            -0.5f, 0.5f, 0.5f, 0.0f, 1.0f,
            -0.5f, -0.5f, 0.5f, 0.0f, 0.0f,
            -0.5f, 0.5f, 0.5f, 1.0f, 0.0f,
            -0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
            -0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
            -0.5f, -0.5f, 0.5f, 0.0f, 0.0f,
            -0.5f, 0.5f, 0.5f, 1.0f, 0.0f,
            0.5f, 0.5f, 0.5f, 1.0f, 0.0f,
            0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
            0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
            0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
            0.5f, -0.5f, 0.5f, 0.0f, 0.0f,
            0.5f, 0.5f, 0.5f, 1.0f, 0.0f,
            -0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
            0.5f, -0.5f, -0.5f, 1.0f, 1.0f,
            0.5f, -0.5f, 0.5f, 1.0f, 0.0f,
            0.5f, -0.5f, 0.5f, 1.0f, 0.0f,
            -0.5f, -0.5f, 0.5f, 0.0f, 0.0f,
            -0.5f, -0.5f, -0.5f, 0.0f, 1.0f,
            -0.5f, 0.5f, -0.5f, 0.0f, 1.0f,
            0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
            0.5f, 0.5f, 0.5f, 1.0f, 0.0f,
            0.5f, 0.5f, 0.5f, 1.0f, 0.0f,
            -0.5f, 0.5f, 0.5f, 0.0f, 0.0f,
            -0.5f, 0.5f, -0.5f, 0.0f, 1.0f
        };

        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            GL.ClearColor(backgroundColor);
            Size = new Vector2i(800, 800);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.Enable(EnableCap.DepthTest);

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
            GL.BufferData(BufferTarget.ElementArrayBuffer, vertices.Length * sizeof(uint), vertices, BufferUsageHint.StaticDraw);

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
            //CalculateAndSetTransform();

            Matrix4 model = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-55.0f));
            Matrix4 cameraModel = Matrix4.CreateTranslation(0.0f, 0.0f, 3);
            Matrix4 view = Matrix4.LookAt(cameraPosition, cameraPosition + cameraFront, cameraUp);
            shader.SetMatrix("view", view);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)Size.X /
            (float)Size.Y, 0.1f, 100.0f);
            shader.SetMatrix("mvp", model * view * projection);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.BindVertexArray(vertexArrayObject);
            texture0.Use(TextureUnit.Texture0);
            texture1.Use(TextureUnit.Texture1);
            shader.Use();
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);

            float time = (float)timer.Elapsed.TotalSeconds;
            int timeLocation = GL.GetUniformLocation(shader.handle, "time");
            GL.Uniform1(timeLocation, time);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)Size.X / (float)Size.Y, 0.1f, 100.0f);
            Matrix4 model = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-55.0f));
            Matrix4 view = Matrix4.LookAt(cameraPosition, cameraPosition + cameraFront, cameraUp);
            shader.SetMatrix("mvp", model * view * projection);

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

            // Kamera bevægelse
            if (input.IsKeyDown(Keys.W))
                cameraPosition += cameraFront * cameraSpeed;  // Fremad
            if (input.IsKeyDown(Keys.S))
                cameraPosition -= cameraFront * cameraSpeed;  // Bagud
            if (input.IsKeyDown(Keys.A))
                cameraPosition -= Vector3.Normalize(Vector3.Cross(cameraFront, cameraUp)) * cameraSpeed;  // Venstre
            if (input.IsKeyDown(Keys.D))
                cameraPosition += Vector3.Normalize(Vector3.Cross(cameraFront, cameraUp)) * cameraSpeed;  // Højre

            // Rotation om Y-aksen
            if (input.IsKeyDown(Keys.Q))
                yaw -= rotationSpeed;  // Roter mod venstre
            if (input.IsKeyDown(Keys.E))
                yaw += rotationSpeed;  // Roter mod højre

            // Opdater cameraFront baseret på yaw-værdien
            Vector3 newFront;
            newFront.X = MathF.Cos(MathHelper.DegreesToRadians(yaw));
            newFront.Y = 0.0f;
            newFront.Z = MathF.Sin(MathHelper.DegreesToRadians(yaw));
            cameraFront = Vector3.Normalize(newFront);

            // Opdater view matrix
            Matrix4 view = Matrix4.LookAt(cameraPosition, cameraPosition + cameraFront, cameraUp);
            shader.Use();
            shader.SetMatrix("view", view);
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

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

        //Change color
        float greenValue = 1;
        bool increaseGreen = false;

        //Change position
        Vector2 position = new Vector2(0, 0);
        float speed = .01f;

        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            GL.ClearColor(backgroundColor);
            Size = new Vector2i(800, 800);
        }



        float[] vertices;
        uint[] indices;
        private int vertexArrayObject;
        int vertexBufferObject;
        Shader shader;

        protected override void OnLoad()
        {
            base.OnLoad();

            // Start med en trekant og divider den rekursivt
            DivideTriangle(new Vector3(0, .5f, 0), new Vector3(-.5f, -.5f, 0), new Vector3(.5f, -.5f, 0), 0);
            vertices = queue.ToArray();
            //HeartRendering();

            // Send data til OpenGL
            vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            int elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, vertices.Length * sizeof(uint), vertices, BufferUsageHint.StaticDraw);

            shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            shader.Use();
            int vertexColorLocation = GL.GetUniformLocation(shader.Handle, "ourColor");
            GL.Uniform4(vertexColorLocation, 0.0f, greenValue, 0.0f, 1.0f);

            GL.BindVertexArray(vertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length / 3);
            //GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.ClearColor(backgroundColor);

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);




            //Input
            KeyboardState input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            if (input.IsKeyDown(Keys.W))
            {
                if (backgroundColor.R < 1)
                    backgroundColor.R += .01f;
                Console.WriteLine(backgroundColor.R);
            }

            // Skift mellem fullscreen og windowed mode med 'F'
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

            //Shader update
            shader.Use();
            int positionLocation = GL.GetUniformLocation(shader.Handle, "offset");
            GL.Uniform2(positionLocation, position);
        }

        void CirckleRendering()
        {
            float radius = .2f;
            int grader = 5;
            int polygons = 360 / grader;
            List<float> calVertices = new List<float>();
            List<uint> calIndices = new List<uint>();

            for (int i = 0; i < polygons; i++)
            {
                double radians = (i * grader) * Math.PI / 180.0; // Konverter til radianer

                double x = Math.Cos(radians);
                double y = Math.Sin(radians);

                calVertices.Add((float)x * radius);
                calVertices.Add((float)y * radius);
                calVertices.Add(0);

                if (i > 0)
                {
                    calIndices.Add((uint)i);
                    calIndices.Add((uint)i - 1);
                    calIndices.Add((uint)0);
                }
            }

            vertices = calVertices.ToArray();
            indices = calIndices.ToArray();
        }

        void HeartRendering()
        {
            float radius = .2f;
            int grader = 5;
            int polygons = 360 / grader;
            List<float> calVertices = new List<float>();
            List<uint> calIndices = new List<uint>();

            for (int i = 0; i < polygons; i++)
            {
                double radians = (i * grader) * Math.PI / 180.0; // Konverter til radianer

                double x = 16 * MathF.Pow(MathF.Sin((float)radians), 3) * radius;
                double y = (13 * Math.Cos(radians) - 5 * MathF.Cos(2 * (float)radians) - MathF.Cos(3 * (float)radians) - MathF.Cos(4 * (float)radians)) * radius;

                calVertices.Add((float)x * radius);
                calVertices.Add((float)y * radius);
                calVertices.Add(0);

                if (i > 0)
                {
                    calIndices.Add((uint)i);
                    calIndices.Add((uint)i - 1);
                    calIndices.Add((uint)0);
                }
            }

            vertices = calVertices.ToArray();
            indices = calIndices.ToArray();
        }


        //List<float> divideCalVertices = new List<float>();
        //List<uint> divideCalIndices = new List<uint>();
        //Dictionary<Vector3, uint> vertexIndexMap = new Dictionary<Vector3, uint>();

        //uint AddVertex(Vector3 v)
        //{
        //    if (vertexIndexMap.TryGetValue(v, out uint index))
        //    {
        //        return index; // Brug eksisterende indeks
        //    }
        //    else
        //    {
        //        index = (uint)(divideCalVertices.Count / 3);
        //        divideCalVertices.Add(v.X);
        //        divideCalVertices.Add(v.Y);
        //        divideCalVertices.Add(v.Z);
        //        vertexIndexMap[v] = index;
        //        return index;
        //    }
        //}

        //void DivideTriangle(Vector3 a, Vector3 b, Vector3 c, int count)
        //{
        //    if (count == 0)
        //    {
        //        uint ia = AddVertex(a);
        //        uint ib = AddVertex(b);
        //        uint ic = AddVertex(c);

        //        divideCalIndices.Add(ia);
        //        divideCalIndices.Add(ib);
        //        divideCalIndices.Add(ic);
        //    }
        //    else
        //    {
        //        // Beregn midtpunkterne af trekanten
        //        Vector3 ab = Vector3.Lerp(a, b, 0.5f);
        //        Vector3 ac = Vector3.Lerp(a, c, 0.5f);
        //        Vector3 bc = Vector3.Lerp(b, c, 0.5f);

        //        // Rekursivt opdel trekanterne
        //        DivideTriangle(a, ab, ac, count - 1);
        //        DivideTriangle(b, bc, ab, count - 1);
        //        DivideTriangle(c, ac, bc, count - 1);
        //        DivideTriangle(ab, bc, ac, count - 1);
        //    }
        //}

        Queue<float> queue = new Queue<float>();
        void Triangle(Vector3 a, Vector3 b, Vector3 c)
        {
            queue.Enqueue(a.X);
            queue.Enqueue(a.Y);
            queue.Enqueue(a.Z);
            queue.Enqueue(b.X);
            queue.Enqueue(b.Y);
            queue.Enqueue(b.Z);
            queue.Enqueue(c.X);
            queue.Enqueue(c.Y);
            queue.Enqueue(c.Z);
        }

        void DivideTriangle(Vector3 a, Vector3 b, Vector3 c, int count)
        {
            //	check	for	end	of	recursion		 				
            if (count == 0)
            {
                Triangle(a, b, c);
            }
            else
            {
                //bisect	the	sides		 				
                Vector3 ab = Vector3.Lerp(a, b, 0.5f);
                Vector3 ac = Vector3.Lerp(a, c, 0.5f);
                Vector3 bc = Vector3.Lerp(b, c, 0.5f);
                //	three	new	triangles		 				
                DivideTriangle(a, ab, ac, count - 1);
                DivideTriangle(c, ac, bc, count - 1);
                DivideTriangle(b, bc, ab, count - 1);
            }
        }



        private void ToggleFullscreen()
        {
            if (isFullscreen)
            {
                WindowState = WindowState.Normal; // Gå tilbage til windowed
                Size = new Vector2i(800, 800);  // Sæt en standard opløsning
            }
            else
            {
                WindowState = WindowState.Fullscreen; // Gå i fullscreen
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

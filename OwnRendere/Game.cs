using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OwnRendere.Shaders;
using System.Diagnostics;

namespace OwnRendere
{
    public class Game : GameWindow
    {
        private Color4 backgroundColor = new Color4(0.2f, 0.1f, 0.1f, 1.0f);
        private bool isFullscreen = false;

        private Texture texture0;
        private Texture texture1;


        public List<GameObject> gameObjects = new List<GameObject>();

        Camera camera;

        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            GL.ClearColor(backgroundColor);
            Size = new Vector2i(800, 800);
        }

        protected override void OnLoad()
        {            
            base.OnLoad();

            GL.Enable(EnableCap.DepthTest);

            texture0 = new Texture("Textures/wall.jpg");
            texture1 = new Texture("Textures/AragonTexUdenBaggrund.png");
            Dictionary<string, object> uniforms = new Dictionary<string, object>();
            uniforms.Add("texture0", texture0);
            uniforms.Add("texture1", texture1);
            Material mat = new Material("Shaders/shader.vert",
            "Shaders/shader.frag", uniforms);
            Renderer rend = new Renderer(mat, new TriangleMesh());
            Renderer rend2 = new Renderer(mat, new CubeMesh());

            //Camera
            GameObject cam = new GameObject(null, this);
            cam.AddComponent<Camera>(60.0f, (float)Size.X, (float)Size.Y, 0.3f, 1000.0f);
            cam.AddComponent<CameraMoveBehaviour>();
            camera = cam.GetComponent<Camera>();
            gameObjects.Add(cam);

            GameObject triangle = new GameObject(rend, this);
            gameObjects.Add(triangle);

            GameObject cube = new GameObject(rend2, this);
            cube.AddComponent<MoveUpDownBehaviour>();
            cube.transform.Position = new Vector3(1, 0, 0);
            gameObjects.Add(cube);

            GameObject inport = LoadeObject("moddels/Suzanne.obj", this);
            gameObjects.Add(inport);
        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            gameObjects.ForEach(x => x.Update(args));
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            gameObjects.ForEach(x => x.Draw(camera.GetViewProjection()));
            SwapBuffers();

            //Input
            KeyboardState input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            if (input.IsKeyDown(Keys.F))
            {
                ToggleFullscreen();
            }
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

        GameObject LoadeObject(string filename,GameWindow gameWindow)
        {
            ObjVolume obj = ObjVolume.LoadFromfile(filename);

            float[] vertices = new float[(obj.faces.Count * 3 * 3) + (obj.faces.Count * 2 * 3)];
            int verticesIndex = 0;
            int textrureIndex = 0;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = obj.faces[verticesIndex].Item1.Position.X;
                i++;
                vertices[i] = obj.faces[verticesIndex].Item1.Position.Y;
                i++;
                vertices[i] = obj.faces[verticesIndex].Item1.Position.Z;
                i++;
                vertices[i] = obj.faces[textrureIndex].Item1.TextureCoord.X;
                i++;
                vertices[i] = obj.faces[textrureIndex].Item1.TextureCoord.Y;
                i++;
                vertices[i] = obj.faces[verticesIndex].Item2.Position.X;
                i++;
                vertices[i] = obj.faces[verticesIndex].Item2.Position.Y;
                i++;
                vertices[i] = obj.faces[verticesIndex].Item2.Position.Z;
                i++;
                vertices[i] = obj.faces[textrureIndex].Item2.TextureCoord.X;
                i++;
                vertices[i] = obj.faces[textrureIndex].Item2.TextureCoord.Y;
                i++;
                vertices[i] = obj.faces[verticesIndex].Item3.Position.X;
                i++;
                vertices[i] = obj.faces[verticesIndex].Item3.Position.Y;
                i++;
                vertices[i] = obj.faces[verticesIndex].Item3.Position.Z;
                i++;
                vertices[i] = obj.faces[textrureIndex].Item3.TextureCoord.X;
                i++;
                vertices[i] = obj.faces[textrureIndex].Item3.TextureCoord.Y;
                verticesIndex++;
                textrureIndex++;
            }
            int[] temp = obj.GetIndices();
            uint[] indices = new uint[temp.Length];
            for (int i = 0; i < indices.Length; i++)
            {
                indices[i] = (uint)temp[i];
            }

            CustomMesh customMesh = new CustomMesh(vertices, indices);
            Texture texture0 = new Texture("Textures/wall.jpg");
            Texture texture1 = new Texture("Textures/AragonTexUdenBaggrund.png");
            Dictionary<string, object> uniforms = new Dictionary<string, object>();
            uniforms.Add("texture0", texture0);
            uniforms.Add("texture1", texture1);
            Material mat = new Material("Shaders/shader.vert",
            "Shaders/shader.frag", uniforms);
            Renderer renderer = new(mat, customMesh);

            GameObject gameObject = new(renderer, gameWindow);
            return gameObject;
        }
    }
}

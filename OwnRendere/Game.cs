using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OwnRendere.Shapes;

namespace OwnRendere
{
    public class Game : GameWindow
    {
        private Color4 backgroundColor = new Color4(0.2f, 0.1f, 0.1f, 1.0f);
        private bool isFullscreen = false;

        private Texture texture0;
        private Texture texture1;

        private Texture texture2;
        private Texture texture3;

        public List<GameObject> gameObjects = new List<GameObject>();
        //UI
        public List<GameObject> UI = new List<GameObject>();
        private Matrix4 uiProjection;
        private double uiUpdateTimer = 0.0;
        private const double uiUpdateInterval = 0.1; 

        Camera camera;

        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            GL.ClearColor(backgroundColor);
            Size = new Vector2i(800, 800);
        }

        protected override void OnLoad()
        {            
            base.OnLoad();            

            texture0 = new Texture("Textures/wall.jpg", .2f);
            texture1 = new Texture("Textures/AragonTexUdenBaggrund.png", .2f);
            Dictionary<string, object> uniforms = new Dictionary<string, object>();
            uniforms.Add("texture0", texture0);
            uniforms.Add("texture1", texture1);
            Material mat_3D = new Material("Shaders/shader.vert", "Shaders/shader.frag", uniforms);
            //UI
            //Map
            texture2 = new Texture("Sprites/round_brown.png", 100);
            Dictionary<string, object> tImages = new Dictionary<string, object>();
            tImages.Add("uiTexture", texture2);
            Material uiMaterial = new Material("Shaders/ui_shader.vert", "Shaders/ui_shader.frag", tImages, true);
            Renderer ui = new Renderer(uiMaterial, new PlaneMesh());
            //Dot
            Dictionary<string, object> dotImages = new Dictionary<string, object>();
            texture3 = new Texture("Sprites/progress_red_small_border.png", 100);
            dotImages.Add("uiTexture", texture3);
            Material dotMaterial = new Material("Shaders/ui_shader.vert", "Shaders/ui_shader.frag", dotImages, true);
            Renderer dot = new Renderer(dotMaterial, new PlaneMesh());
            Material mat = new Material("Shaders/shader.vert",
            "Shaders/shader.frag", uniforms);
            Renderer rend = new Renderer(mat, new TriangleMesh());
            Renderer rend2 = new Renderer(mat, new CubeMesh());
            Renderer rend3 = new Renderer(mat, new CylinderMesh(36, 1.0f, 0.5f));
            Renderer rend4 = new Renderer(mat, new SphereMesh(36, 36, 0.5f));
            

            //Camera
            GameObject cam = new GameObject(null, this);
            cam.AddComponent<Camera>(60.0f, (float)Size.X, (float)Size.Y, 0.3f, 1000.0f);
            cam.AddComponent<CameraMoveBehaviour>();
            camera = cam.GetComponent<Camera>();
            gameObjects.Add(cam);

            //Triangle
            GameObject triangle = new GameObject(rend, this);
            gameObjects.Add(triangle);

            //Cube
            GameObject cube = new GameObject(rend2, this);
            cube.AddComponent<MoveUpDownBehaviour>();
            cube.transform.Position = new Vector3(1, 0, 0);
            gameObjects.Add(cube);

            GameObject inport = LoadeObject("moddels/Suzanne.obj", this);
            inport.transform.Position = new Vector3(-3.5f,0,0);
            gameObjects.Add(inport);

            GameObject cylinder = new GameObject(rend3, this);
            cylinder.transform.Position = new Vector3(-2, 0, 0);
            gameObjects.Add(cylinder);

            GameObject sphere = new GameObject(rend4, this);
            sphere.AddComponent<RotateBehaviour>();
            sphere.transform.Position = new Vector3(-1, 0, 0);
            gameObjects.Add(sphere);

            //UI
            //Plane
            GameObject plane = new GameObject(ui, this);
            plane.transform.Position = new Vector3(Size.X / 2 - 200, Size.Y / 2 + 200, 0);
            plane.transform.Scale = new Vector3(1.5f, 1.5f, 1);
            UI.Add(plane);
            //Dot
            GameObject dotObj = new GameObject(dot, this);
            dotObj.transform.Position = new Vector3(Size.X / 2 - 200, Size.Y / 2 + 200, -1);
            dotObj.transform.Scale = new Vector3(1.2f, 1.2f, 1);
            dotObj.AddComponent<LoopMovementRightToLeft>(25, 50);
            UI.Add(dotObj);
        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            
            gameObjects.ForEach(x => x.Update(args));

            //UI
            uiUpdateTimer += args.Time;
            if (uiUpdateTimer >= uiUpdateInterval)
            {
                // Sort UI-listen, så elementer med den største Z-værdi tegnes først
                UI.Sort((a, b) => b.transform.Position.Z.CompareTo(a.transform.Position.Z));
                UI.ForEach(x => x.Update(args)); //Draw
                uiUpdateTimer = 0.0; //Reset time
            }
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            gameObjects.ForEach(x => x.Draw(camera.GetViewProjection()));

            // UI rendering
            GL.Disable(EnableCap.DepthTest);
            uiProjection = Matrix4.CreateOrthographicOffCenter(0, Size.X, 0, Size.Y, -1, 1);
            UI.ForEach(x => x.Draw(uiProjection));


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
            Texture texture0 = new Texture("Textures/wall.jpg", 0.2f);
            Texture texture1 = new Texture("Textures/AragonTexUdenBaggrund.png", 0.2f);
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

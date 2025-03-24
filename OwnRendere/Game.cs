using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OwnRendere.Shaders;
using OwnRendere.Shapes;
using System.Diagnostics;

namespace OwnRendere
{
    public class Game : GameWindow
    {
        private Color4 backgroundColor = new Color4(0.2f, 0.1f, 0.1f, 1.0f);
        private bool isFullscreen = false;

        private Texture texture0;
        private Texture texture1;

        private Texture texture2;

        public List<GameObject> gameObjects = new List<GameObject>();
        //UI
        public List<GameObject> UI = new List<GameObject>();
        private Matrix4 uiProjection;

        Camera camera;

        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            GL.ClearColor(backgroundColor);
            Size = new Vector2i(800, 800);
        }

        protected override void OnLoad()
        {            
            base.OnLoad();            

            texture0 = new Texture("Textures/wall.jpg");
            texture1 = new Texture("Textures/AragonTexUdenBaggrund.png");
            Dictionary<string, object> uniforms = new Dictionary<string, object>();
            uniforms.Add("texture0", texture0);
            uniforms.Add("texture1", texture1);
            Material mat_3D = new Material("Shaders/shader.vert", "Shaders/shader.frag", uniforms);
            Renderer rend = new Renderer(mat_3D, new TriangleMesh());
            Renderer rend2 = new Renderer(mat_3D, new CubeMesh());

            Dictionary<string, object> UI_Images = new Dictionary<string, object>();
            texture2 = new Texture("Sprites/round_brown.png");
            UI_Images.Add("texture2", texture2);
            Material uiMaterial = new Material("Shaders/ui_shader.vert", "Shaders/ui_shader.frag", UI_Images, true);
            Renderer ui = new Renderer(uiMaterial, new PlaneMesh());

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

            //Place
            GameObject plane = new GameObject(ui, this);
            plane.transform.Position = new Vector3(200, 400, 0);
            plane.transform.Scale = new Vector3(50, 50, 1);
            UI.Add(plane);
        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            
            gameObjects.ForEach(x => x.Update(args));
            UI.ForEach(x => x.Update(args));

        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            gameObjects.ForEach(x => x.Draw(camera.GetViewProjection()));

            //UI
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
    }
}

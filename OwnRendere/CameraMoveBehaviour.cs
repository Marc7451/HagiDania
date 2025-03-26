using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OwnRendere
{
    public class CameraMoveBehaviour : Behaviour
    {
        float movementSpeed = 1;
        float sensitivity = 0.1f;
        private Vector2 lastMousePosition;
        private bool firstMouse = true;
        public CameraMoveBehaviour(GameObject gameObject, Game window) : base(gameObject, window) { }
        public override void Update(FrameEventArgs args)
        {
            KeyboardState input = window.KeyboardState;
            if (input.IsKeyDown(Keys.W))
            {
                gameObject.transform.Position -= gameObject.transform.Front * movementSpeed * (float)args.Time;
            }
            if (input.IsKeyDown(Keys.S))
            {
                gameObject.transform.Position += gameObject.transform.Front * movementSpeed * (float)args.Time;
            }
            if (input.IsKeyDown(Keys.A))
            {
                gameObject.transform.Position -= gameObject.transform.Right * movementSpeed * (float)args.Time;
            }
            if (input.IsKeyDown(Keys.D))
            {
                gameObject.transform.Position += gameObject.transform.Right * movementSpeed * (float)args.Time;
            }
        }
    }
}

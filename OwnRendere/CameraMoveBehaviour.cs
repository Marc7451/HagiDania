using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;

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

        private float yaw = 0f; 
        private float pitch = 0f; 
        private float rotationSpeed = 2f; 

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

            if (input.IsKeyDown(Keys.Q)) // Rotate Right (Yaw)
            {
                yaw += rotationSpeed * (float)args.Time;
            }
            if (input.IsKeyDown(Keys.E)) // Rotate Left (Yaw)
            {
                yaw -= rotationSpeed * (float)args.Time;
            }
            if (input.IsKeyDown(Keys.R)) // Look Up (Pitch)
            {
                pitch -= rotationSpeed * (float)args.Time;
            }
            if (input.IsKeyDown(Keys.T)) // Look Down (Pitch)
            {
                pitch += rotationSpeed * (float)args.Time;
            }

            // Clamp pitch so the camera doesn't flip upside down
            pitch = MathHelper.Clamp(pitch, -89f, 89f);

            // Create a new rotation vector
            Vector3 newRotation = new Vector3(pitch, yaw, 0f);

            // Apply it to your game object’s transform
            gameObject.transform.Rotation = newRotation;
        }
    }
}

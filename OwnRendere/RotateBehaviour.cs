using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OwnRendere.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnRendere
{
    class RotateBehaviour : Behaviour
    {
        float movementSpeed = 1;
        public RotateBehaviour(GameObject gameObject, Game window) :
        base(gameObject, window)
        {
        }
        public override void Update(FrameEventArgs args)
        {
            KeyboardState input = window.KeyboardState;
            if (input.IsKeyDown(Keys.Right))
            {
                gameObject.transform.Rotation.Y += movementSpeed
                * (float)args.Time;
            }
            if (input.IsKeyDown(Keys.Left))
            {
                gameObject.transform.Rotation.Y -= movementSpeed
                * (float)args.Time;
            }
        }
    
    }
}

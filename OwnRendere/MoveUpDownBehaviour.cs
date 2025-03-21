﻿using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OwnRendere.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnRendere
{
    internal class MoveUpDownBehaviour : Behaviour
    {
        float movementSpeed = 1;
        public MoveUpDownBehaviour(GameObject gameObject, Game window) :
        base(gameObject, window)
        {
        }
        public override void Update(FrameEventArgs args)
        {
            KeyboardState input = window.KeyboardState;
            if (input.IsKeyDown(Keys.Up))
            {
                gameObject.transform.Position.Y += movementSpeed
                * (float)args.Time;
            }
            if (input.IsKeyDown(Keys.Down))
            {
                gameObject.transform.Position.Y -= movementSpeed
                * (float)args.Time;
            }
        }
    }
}

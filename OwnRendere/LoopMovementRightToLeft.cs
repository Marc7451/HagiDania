using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnRendere
{
    internal class LoopMovementRightToLeft : Behaviour
    {
        float movementSpeed;
        private float rightPoint;
        private bool moveRight = true;
        private float startX;

        public LoopMovementRightToLeft(GameObject gameObject, Game window, float rightPoint = 2, float speed = 1) : base(gameObject, window)
        {
            this.rightPoint = rightPoint;
            movementSpeed = speed;

            startX = gameObject.transform.Position.X;
        }

        public override void Update(FrameEventArgs args)
        {
            if(moveRight)
            {
                gameObject.transform.Position.X += movementSpeed * .1f; //.1f is update rate for ui.
                if(gameObject.transform.Position.X > startX + rightPoint)
                    moveRight = false;
            }
            else
            {
                gameObject.transform.Position.X -= movementSpeed * .1f;
                if (gameObject.transform.Position.X < startX)
                    moveRight = true;
            }

        }
    }
}

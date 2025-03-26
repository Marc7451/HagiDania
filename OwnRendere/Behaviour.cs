using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnRendere
{
    public abstract class Behaviour
    {
        protected GameObject gameObject;
        protected Game window;
        public Behaviour(GameObject gameObject, Game window)
        {
            this.gameObject = gameObject;
            this.window = window;
        }
        public abstract void Update(FrameEventArgs args);
    }
}

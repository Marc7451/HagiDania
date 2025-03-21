using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnRendere.Shaders
{
    public class GameObject
    {
        public Transform transform;
        public Renderer renderer;
        private GameWindow gameWindow;

        List<Behaviour> behaviours = new List<Behaviour>();

        public GameObject(Renderer renderer, GameWindow gameWindow)
        {
            this.renderer = renderer;
            this.gameWindow = gameWindow;
            transform = new Transform();
        }
        
        public T GetComponent<T>() where T : Behaviour
        {
            foreach (Behaviour component in behaviours)
            {
                T componentAsT = component as T;
                if (componentAsT != null) return componentAsT;
            }
            return null;
        }
        public void AddComponent<T>(params object?[]? args) where T : Behaviour
        {
            if (args == null)
            {
                behaviours.Add(Activator.CreateInstance(typeof(T), this, gameWindow) as T);
            }
            else
            {
                int initialParameters = 2;
                int totalParams = args.Length + initialParameters;
                object?[]? objects = new object[totalParams];
                objects[0] = this;
                objects[1] = gameWindow;
                for (int i = initialParameters; i < totalParams; i++)
                {
                    objects[i] = args[i - 2];
                }
                behaviours.Add(Activator.CreateInstance(typeof(T), objects) as T);
            }
        }
        public void Update(FrameEventArgs args)
        {
            foreach (Behaviour item in behaviours)
            {
                item.Update(args);
            }
        }
        public void Draw(Matrix4 vp)
        {
            if(renderer != null)
                renderer.Draw(transform.CalculateModel() * vp);
        }
    }
}

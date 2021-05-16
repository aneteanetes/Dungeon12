using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.GameObjects
{
    public class EmptyGameComponent : IGameComponent
    {
        public ISceneObject SceneObject { get; set; }

        public void SetView(ISceneObject sceneObject)
        {
            SceneObject = sceneObject;
        }

        public void Destroy()
        {
            SceneObject.Destroy?.Invoke();
        }

        public void Init() { }

        public static EmptyGameComponent Empty { get; } = new EmptyGameComponent();

        public string Name { get; set; }
    }
}

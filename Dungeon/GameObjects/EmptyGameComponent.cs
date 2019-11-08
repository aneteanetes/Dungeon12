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

        public static EmptyGameComponent Empty { get; } = new EmptyGameComponent();
    }
}

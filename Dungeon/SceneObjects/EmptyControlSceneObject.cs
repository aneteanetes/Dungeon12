using Dungeon.GameObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.SceneObjects
{
    public class EmptyControlSceneObject : ControlSceneObject<EmptyGameComponent>
    {
        public EmptyControlSceneObject() : base(EmptyGameComponent.Empty)
        {
        }
    }
}

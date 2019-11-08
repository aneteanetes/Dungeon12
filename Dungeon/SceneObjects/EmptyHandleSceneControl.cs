using Dungeon.GameObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.SceneObjects
{
    public class EmptyHandleSceneControl : HandleSceneControl<EmptyGameComponent>
    {
        public EmptyHandleSceneControl() : base(EmptyGameComponent.Empty)
        {
        }
    }
}

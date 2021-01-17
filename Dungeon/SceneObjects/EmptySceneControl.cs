using Dungeon.GameObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.SceneObjects
{
    public class EmptySceneControl : SceneControl<EmptyGameComponent>
    {
        public EmptySceneControl() : base(EmptyGameComponent.Empty)
        {
        }
    }
}

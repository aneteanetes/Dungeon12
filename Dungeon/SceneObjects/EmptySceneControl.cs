using Dungeon.GameObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.SceneObjects
{
    public abstract class EmptySceneControl : SceneControl<GameComponentEmpty>
    {
        public EmptySceneControl() : base(GameComponentEmpty.Empty)
        {
        }
    }
}

using Dungeon.GameObjects;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.SceneObjects
{
    public abstract class EmptySceneControl : SceneControl<GameComponentEmpty>
    {
        public EmptySceneControl(ISceneLayer layer) : base(GameComponentEmpty.Empty)
        {
        }
    }
}

using Dungeon.Transactions;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.GameObjects
{
    public abstract class GameComponent : Applicable, IGameComponent
    {
        public ISceneObject SceneObject { get; set; }

        public virtual void SetView(ISceneObject sceneObject)
        {
            SceneObject = sceneObject;
        }
    }
}

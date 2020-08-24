using Dungeon.Transactions;
using Dungeon.Utils;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.GameObjects
{
    public abstract class GameComponent : Applicable, IGameComponent
    {
        [Newtonsoft.Json.JsonIgnore]
        [Hidden]
        public ISceneObject SceneObject { get; set; }

        public virtual void SetView(ISceneObject sceneObject)
        {
            SceneObject = sceneObject;
        }

        protected override void CallApply(dynamic obj) { }

        protected override void CallDiscard(dynamic obj) { }
    }
}

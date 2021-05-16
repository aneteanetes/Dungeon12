using Dungeon.Data;
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

        public string Name { get; set; }

        public void Destroy()
        {
            SceneObject?.Destroy?.Invoke();
            SceneObject = default;
        }

        public virtual void SetView(ISceneObject sceneObject)
        {
            SceneObject = sceneObject;
        }

        protected override void CallApply(dynamic obj) { }

        protected override void CallDiscard(dynamic obj) { }

        private bool _inited = false;
        public void Init()
        {
            if (!_inited)
            {
                Initialization();
                _inited = true;
            }
        }

        public virtual void Initialization() { }
    }

    public abstract class StoredGameComponent<T> : GameComponent
        where T: Persist
    {
        public T Data { get; protected set; }

        public StoredGameComponent(int id)
        {
            Data = Persist.LoadOne<T>(x => x.ObjectId == id);
        }

        public StoredGameComponent(string name)
        {
            Data = Persist.LoadOne<T>(x => x.IdentifyName == name);
        }
    }
}

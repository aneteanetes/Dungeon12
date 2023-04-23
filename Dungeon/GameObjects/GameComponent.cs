﻿using Dungeon.Transactions;
using Dungeon.Utils;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon.GameObjects
{
    public abstract class GameComponent : Applicable, IGameComponent
    {
        public Dictionary<Type, GameComponentBehavior> ComponentsMap = new Dictionary<Type, GameComponentBehavior>();

        public TComponent Component<TComponent>() where TComponent : GameComponentBehavior
            => (TComponent)ComponentsMap[typeof(TComponent)];

        public GameComponent()
        {
            if (ComponentsMap == null)
            {
                GetType()
                    .GetCustomAttributes(true)
                    .Where(x => x is GameComponentBehavior)
                    .Select(x => x as GameComponentBehavior)
                    .ForEach(component =>
                    {
                        ComponentsMap[component.GetType()] = component;
                    });
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        [Hidden]
        public ISceneObject SceneObject { get; set; }

        public string Name { get; set; }

        public virtual void Destroy()
        {
            SceneObject.Destroy();
            SceneObject = default;
            OnDestroyGameComponent?.Invoke();
            IsDestroyed=true;
        }

        public bool IsDestroyed { get; private set; }


        public Action OnDestroyGameComponent;

        public virtual void SetView(ISceneObject sceneObject)
        {
            SceneObject = sceneObject;
            IsDestroyed=false;
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

        public void Refresh() => SceneObject.Refresh();
    }
}

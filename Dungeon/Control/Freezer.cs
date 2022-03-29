using Dungeon.SceneObjects;
using Dungeon.Scenes.Manager;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Control
{
    public class Freezer
    {
        private SceneManager sceneManager;

        public Freezer(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
        }

        private object freezeWorldObject;

        public object World
        {
            get => freezeWorldObject;
            set
            {
                if (value == null)
                {
                    DungeonGlobal.Time.Resume();
                    sceneManager.Current.Freezer = null;
                }
                else
                {
                    sceneManager.Current.Freezer = value;
                    DungeonGlobal.Time.Pause();
                }

                freezeWorldObject = value;
            }
        }

        private Stack<object> freezeStack = new Stack<object>();

        public void Freeze(object @object)
        {
            if (World == @object)
                return;

            if (freezeWorldObject != null)
                freezeStack.Push(freezeWorldObject);
            World = @object;
        }

        public void Freeze(IAutoUnfreeze destroyable)
        {
            if (World == destroyable)
                return;

            if (freezeWorldObject != null)
                freezeStack.Push(freezeWorldObject);

            World = destroyable;

            destroyable.Destroy += () => Unfreeze();
        }

        public void Unfreeze()
        {
            if (freezeStack.Count == 0)
                World = null;
            else
                World = freezeStack.Pop();
        }

        public Dictionary<ControlEventType, object> HandleFreezes = new Dictionary<ControlEventType, object>();

        public void FreezeHandle(ControlEventType controlEventType, object freezer)
        {
            if (HandleFreezes.ContainsKey(controlEventType))
            {
                HandleFreezes.Remove(controlEventType);
            }

            HandleFreezes.Add(controlEventType, freezer);
        }

        public void UnfreezeHandle(ControlEventType controlEventType, object freezer)
        {
            if (!HandleFreezes.ContainsKey(controlEventType))
            {
                return;
            }

            HandleFreezes.Remove(controlEventType);
        }
    }
}

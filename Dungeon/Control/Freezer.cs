using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Control
{
    public class Freezer
    {
        private object freezeWorldObject;

        public object World
        {
            get => freezeWorldObject;
            set
            {
                if (value == null)
                {
                    Global.Time.Resume();
                }
                else
                {
                    Global.Time.Pause();
                }

                freezeWorldObject = value;
            }
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

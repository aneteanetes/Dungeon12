using System;

namespace Dungeon.Types
{
    public class Once : Singleton<string, Once>
    {
        public Once(string name) : base(name) { }

        public bool Failed { get; private set; }

        public static Once Call(Action action, string name = default)
        {
            var once = new Once(name ?? Guid.NewGuid().ToString());
            once.InternalRun(action);
            return once;
        }

        private void InternalRun(Action action)
        {
            if (This.Failed)
                return;

            try
            {
                action?.Invoke();
            }
            catch (Exception e)
            {
                This.Failed = true;
                DungeonGlobal.Logger.Log(e.ToString());
            }
        }
    }
}
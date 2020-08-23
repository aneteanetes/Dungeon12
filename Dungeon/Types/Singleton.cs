using System.Collections.Generic;

namespace Dungeon.Types
{
    public abstract class Singleton<Tkey, TValue>
        where TValue : class
    {
        private static Dictionary<Tkey, TValue> Instances = new Dictionary<Tkey, TValue>();

        protected TValue This { get; private set; }

        public Singleton(Tkey key)
        {
            if (Instances.ContainsKey(key))
            {
                This = Instances[key];
            }
            else
            {
                This = (this as TValue);
                Instances.Add(key, This);
            }
        }
    }
}
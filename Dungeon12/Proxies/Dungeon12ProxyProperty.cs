using Dungeon;
using Dungeon12.Classes;
using Dungeon.Drawing;
using Dungeon12.Map;
using Dungeon12.SceneObjects; using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using System;

namespace Dungeon12.Proxies
{
    public abstract class Dungeon12ProxyProperty<TClass> : ProxyProperty where TClass : Character
    {
        protected long Now => __Get<long>();

        public override T Get<T>(T v, string proxyId)
        {
            return Get(v.As<long>(), owner.As<TClass>(), proxyId).As<T>();
        }

        public override T Set<T>(T v, string proxyId)
        {
            return Set(v.As<long>(), owner.As<TClass>(), proxyId).As<T>();
        }

        protected abstract long Get(long v, TClass noone, string proxyId);

        protected abstract long Set(long v, TClass noone, string proxyId);
    }
}
using FastMember;
using System;

namespace Dungeon
{
    public abstract class ProxyProperty
    {
        public string Name;
        public object owner;
        public TypeAccessor ownerAccessor;

        private Func<object> _get;
        private Action<object> _set;

        public void BindAccessors(Func<object> get, Action<object> set)
        {
            _get = get;
            _set = set;
        }

        protected void __Set<T>(T value) => _set?.Invoke(value);

        protected T __Get<T>() => (T)_get?.Invoke();

        public abstract T Get<T>(T v, string proxyId);

        public abstract T Set<T>(T v, string proxyId);
    }
}
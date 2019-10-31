using FastMember;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ProxiedAttribute : Attribute
    {
        private IEnumerable<ProxyProperty> Proxies { get; }

        public ProxiedAttribute(params Type[] types)
        {
            if (types == default)
            {
                Proxies = Enumerable.Empty<ProxyProperty>();
            }

            Proxies = types
                .Select(t => t.NewAs<ProxyProperty>())
                .Reverse();
        }

        public T Get<T>(T value, string proxyId, Func<object> get, Action<object> set, object owner, TypeAccessor ownerAccessor, string propName)
        {
            return Proxies.Reduce(value, (p, v) =>
            {
                p.Name = propName;
                p.owner = owner;
                p.ownerAccessor = ownerAccessor;
                p.BindAccessors(get, set);
                return p.Get(v, proxyId);
            });
        }

        public T Set<T>(T value, string proxyId, Func<object> get, Action<object> set, object owner, TypeAccessor ownerAccessor, string propName)
        {
            return Proxies.Reduce(value, (p, v) =>
            {
                p.Name = propName;
                p.owner = owner;
                p.ownerAccessor = ownerAccessor;
                p.BindAccessors(get, set);
                return p.Set(v, proxyId);
            });
        }
    }
}
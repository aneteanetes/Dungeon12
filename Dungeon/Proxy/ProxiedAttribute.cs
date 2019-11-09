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
                .Reverse()
                .ToList();
        }

        public T Get<T>(T value, string proxyId, Func<object> get, Action<object> set, object owner, TypeAccessor ownerAccessor, string propName, List<ProxyProperty> additional)
        {
            T reduce(ProxyProperty p, T v)
            {
                p.Name = propName;
                p.owner = owner;
                p.ownerAccessor = ownerAccessor;
                p.BindAccessors(get, set);
                return p.Get(v, proxyId);
            }

            var result = Proxies.Reduce(value, reduce);

            if (additional != default)
            {
                result = additional.Reduce(result, reduce);
            }

            return result;
        }

        public T Set<T>(T value, string proxyId, Func<object> get, Action<object> set, object owner, TypeAccessor ownerAccessor, string propName, List<ProxyProperty> additional)
        {
            T reduce(ProxyProperty p, T v)
            {
                p.Name = propName;
                p.owner = owner;
                p.ownerAccessor = ownerAccessor;
                p.BindAccessors(get, set);
                return p.Set(v, proxyId);
            }

            var result = Proxies.Reduce(value, reduce);

            if (additional != default)
            {
                result = additional.Reduce(result, reduce);
            }

            return result;
        }
    }
}
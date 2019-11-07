using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Entities.Alive.Proxies
{
    public class Limit : ProxyProperty
    {
        public override T Get<T>(T v, string proxyId) => v;

        public override T Set<T>(T v, string proxyId)
        {
            var maxProp = $"Max{Name}";
            var maxValue = ownerAccessor[owner, maxProp];

            if (maxValue.IsDefault())
            {
                return v;
            }

            if (v is long l)
            {
                var m = (long)maxValue;
                if (l > m)
                {
                    v = (l = m).As<T>();
                }

                if (l < 0)
                {
                    v = 0L.As<T>();
                }
            }

            if (v is int i)
            {
                var m = (int)maxValue;
                if (i > m)
                {
                    v = (i = m).As<T>();
                }

                if (i < 0)
                {
                    v = 0.As<T>();
                }
            }

            return v;
        }
    }
}
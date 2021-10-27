using Dungeon;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Noone.Proxies
{
    public class StaminaProxyProperty : NooneProxyProperty
    {
        protected override long Get(long v, Noone noone, string proxyId) => v + noone.Stamina * 2;

        protected override long Set(long v, Noone noone, string proxyId) => v;
    }
}

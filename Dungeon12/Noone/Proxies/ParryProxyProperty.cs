using Dungeon;
using Dungeon.Drawing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Noone.Proxies
{
    public class ParryProxyProperty : NooneProxyProperty
    {
        protected override long Get(long v, Noone noone, string proxyId) => v;

        protected override long Set(long v, Noone noone, string proxyId)
        {
            if (v < Now && noone.InParry)
            {
                var dmg = Now - v;
                var i = RandomDungeon.Next(0, 101);
                if (i <= noone.Parry)
                {
                    var parried = (long)Math.Floor(dmg * (noone.Block / 100d));
                    dmg -= parried;
                    Message($"Паррировано: {parried}!", DrawColor.Red);
                }

                if (v < dmg)
                {
                    dmg = 0;
                }

                v = Now - dmg;

                return v;
            }

            return v;
        }
    }
}
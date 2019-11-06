using Dungeon;
using Dungeon.Drawing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon12.Noone.Proxies
{
    public class BlockProxyProperty : NooneProxyProperty
    {
        protected override long Get(long v, Noone noone, string proxyId) => v;

        protected override long Set(long v, Noone noone, string proxyId)
        {
            if (v < Now)
            {
                var dmg = Now - v;
                var i = RandomDungeon.Next(1, 101);
                if (i <= noone.Block)
                {
                    var block = (long)Math.Floor(dmg * (noone.Block / 100d));
                    dmg -= block;
                    if (block > 0)
                    {
                        Message($"Блок: {block}!", DrawColor.Red);
                    }
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
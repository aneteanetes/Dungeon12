using Dungeon;
using Dungeon.Abilities;
using Dungeon.Drawing;
using Dungeon12.Noone.Abilities;
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

                var parried = dmg / 2;
                dmg -= parried;
                if (parried > 0)
                {
                    Cooldown.Done(Attack.AttackCooldown);
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
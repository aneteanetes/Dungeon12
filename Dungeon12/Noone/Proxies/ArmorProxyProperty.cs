namespace Dungeon12.Noone.Proxies
{
    public class ArmorProxyProperty : NooneProxyProperty
    {
        protected override long Get(long v, Noone noone, string proxyId) => v;

        protected override long Set(long v, Noone noone, string proxyId)
        {
            if (v < Now)
            {
                var dmg = Now - v;

                var less = noone.Armor / 2;
                dmg -= less;

                if (dmg < 0)
                {
                    dmg = 0;
                }

                return Now - dmg;
            }

            return v;
        }
    }
}
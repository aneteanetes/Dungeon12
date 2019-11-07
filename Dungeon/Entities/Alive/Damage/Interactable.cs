using Dungeon.Drawing;
using Dungeon.Map;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using System;

namespace Dungeon.Entities.Alive
{
    public class Interactable : Modified
    {
        public virtual void Damage(Damage dmg)
        {
            var amount = this.Call<long>(dmg.Type.Name, 999999999, dmg);
            if (amount == 999999999)
            {
                amount = this.Call<long, Interactable>(dmg.Type.Name, 999999999, dmg);
                if (amount == 999999999)
                {
                    amount = dmg.Amount;
                }
            }

            amount -= Resist(dmg, dmg.Type.PhysicRate, dmg.Type.MagicRate);

            if (amount < 0)
            {
                amount = 0;
            }

            this.HitPoints -= amount;

            var map = this.Map;
            if(map==default)
            {
                var parent = this.GetParentFlow();
                if(parent.Is<MapObject>())
                {
                    map = parent.As<MapObject>();
                }
            }

            if (map != default)
            {
                this.Flow(x => x.ShowEffect(true), new
                {
                    Effects = new PopupString(amount.ToString(), dmg.Type.Color.GetColor(), map.Location).InList<ISceneObject>()
                });
            }
        }

        protected virtual long Resist(Damage dmg, double defence, double barrier)
        {
            double thisDefence = this.Defence;
            double thisBarrier = this.Barrier;

            if (defence == 0 && barrier == 0)
            {
                switch (dmg.Type.Element)
                {
                    case Elements.Physical:
                        return dmg.Amount + dmg.Amount * (dmg.ArmorPenetration / 100);
                    case Elements.Magical:
                        return dmg.Amount + dmg.Amount * (dmg.MagicPenetration / 100);
                    default:
                        return 0;
                }
            }

            if (defence > 0)
            {
                thisDefence -= thisDefence * (dmg.ArmorPenetration / 100d);
            }

            if (barrier > 0)
            {
                thisBarrier -= thisBarrier * (dmg.MagicPenetration / 100d);
            }

            return (long)Math.Ceiling(thisBarrier * barrier + thisDefence * defence);
        }

        private long MultipleLong(long @long, double multiple) => (long)Math.Ceiling(@long * multiple);

        protected virtual long DarkMagic(Damage dmg)
        {
            var hours = Global.Time.Hours;
            if (hours > 8 && hours < 19)
            {
                return dmg.Amount - MultipleLong(dmg.Amount, .4);
            }
            else
            {
                return dmg.Amount + MultipleLong(dmg.Amount, .2);
            }
        }

        protected virtual long FrostMagic(Damage dmg)
        {
            if (dmg.MagicPenetration < 15)
            {
                dmg.MagicPenetration = 15;
            }
            return dmg.Amount;
        }

        protected virtual long Kenetic(Damage dmg)
        {
            if (dmg.ArmorPenetration < 5)
            {
                dmg.ArmorPenetration = 50;
            }
            return dmg.Amount;
        }

        protected virtual long Magical(Damage dmg)
        {
            return dmg.Amount / 4;
        }
    }
}
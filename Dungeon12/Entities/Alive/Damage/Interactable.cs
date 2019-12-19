using Dungeon.Drawing;
using Dungeon12.Map;
using Dungeon12.SceneObjects; using Dungeon.SceneObjects;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using System;
using Dungeon;

namespace Dungeon12.Entities.Alive
{
    public class Interactable : Modified
    {
        public bool Invulnerable { get; set; }

        public virtual void Damage(Interactable attacker, Damage dmg)
        {
            if (Invulnerable)
                return;

            var damageName = $"Damage{dmg.Type.Name}";
            var amount = this.Call<long>(damageName, 999999999, dmg);
            if (amount == 999999999)
            {
                amount = this.Call<long, Interactable>(damageName, 999999999, dmg);
                if (amount == 999999999)
                {
                    amount = dmg.Amount;
                }
            }

            amount -= Resist(dmg, dmg.Type.PhysicRate, dmg.Type.MagicRate);

            amount = DamageProcess(dmg, amount);

            if (amount < 0)
            {
                amount = 0;
            }

            this.HitPoints -= amount;

            var popup = new PopupString(amount.ToString(), dmg.Type.Color.GetColor(), this.MapObject.Location).InList<ISceneObject>();
            this.MapObject?.SceneObject.ShowInScene(popup);

            if (this.HitPoints == 0)
            {
                attacker.Exp(this.ExpGain);
                this.Die();
            }
        }

        /// <summary>
        /// Обработка нанесения урона перед его применением
        /// </summary>
        /// <param name="dmg"></param>
        /// <param name="amount"></param>
        protected virtual long DamageProcess(Damage dmg, long amount) => amount;

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

        protected virtual long DamageDarkMagic(Damage dmg)
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

        protected virtual long DamageFrostMagic(Damage dmg)
        {
            if (dmg.MagicPenetration < 15)
            {
                dmg.MagicPenetration = 15;
            }
            return dmg.Amount;
        }

        protected virtual long DamageKenetic(Damage dmg)
        {
            if (dmg.ArmorPenetration < 5)
            {
                dmg.ArmorPenetration = 50;
            }
            return dmg.Amount;
        }

        protected virtual long DamageMagical(Damage dmg)
        {
            return dmg.Amount / 4;
        }
    }
}
using Rogue.Abilities;
using Rogue.Abilities.Enums;
using Rogue.Abilities.Scaling;
using Rogue.Map;
using Rogue.Map.Objects;
using Rogue.Transactions;
using System;

namespace Rogue.Classes.Servant.Abilities
{
    public class Prayer : Ability<Servant>
    {
        public override AbilityPosition AbilityPosition => AbilityPosition.Q;

        public override AbilityActionAttribute ActionType => AbilityActionAttribute.Hold;

        public override AbilityTargetType TargetType => AbilityTargetType.SelfTarget;

        public override string Name => "Молитва";
        
        public override ScaleRate Scale => ScaleRate.Build(Entites.Enums.Scale.AttackDamage);

        protected override bool CanUse(Servant @class) => true;

        private PrayerBuff holdedBuf = null;

        protected override void Use(GameMap gameMap, Avatar avatar, Servant @class)
        {
            holdedBuf = new PrayerBuff(@class);
            avatar.AddState(holdedBuf);
        }

        protected override void Dispose(GameMap gameMap, Avatar avatar, Servant @class)
        {
            if (holdedBuf != null)
            {
                avatar.RemoveState(holdedBuf);
                holdedBuf = null;
            }
        }

        /// <summary>
        /// так то бафы должны действовать и на аватар тоже
        /// </summary>
        private class PrayerBuff : Applicable
        {
            private Servant _servant;
            public PrayerBuff(Servant servant) => _servant = servant;

            public override string Image => "Abilities/Prayer/buf.png".PathAsmImg();

            IDisposable bufTick;

            public void Apply(Avatar avatar)
            {
                bufTick = Global.Time.Timer(nameof(PrayerBuff) + avatar.Name)
                    .After(3000)
                    .Repeat()
                    .Do(() => _servant.FaithPower.Value++)
                    .Auto();
            }

            public void Discard(Avatar avatar)
            {
                bufTick.Dispose();
            }

            protected override void CallApply(dynamic obj)
            {
                this.Apply(obj);
            }

            protected override void CallDiscard(dynamic obj)
            {
                this.Discard(obj);
            }
        }

        public override AbilityCastType CastType => AbilityCastType.Active;

        public override Location CastLocation => Location.Combat;

        public override string Description => $"Накапливает силу веры раз в секунду";
    }
}

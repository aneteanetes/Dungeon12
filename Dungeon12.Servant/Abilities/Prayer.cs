using Dungeon;
using Dungeon.Abilities;
using Dungeon.Abilities.Enums;
using Dungeon.Abilities.Scaling;
using Dungeon.Drawing.Impl;
using Dungeon.Map;
using Dungeon.Map.Objects;
using Dungeon.SceneObjects;
using Dungeon.Transactions;
using Dungeon.View.Interfaces;
using System;

namespace Dungeon12.Servant.Abilities
{
    public class Prayer : Ability<Servant>
    {
        public override AbilityPosition AbilityPosition => AbilityPosition.Right;

        public override AbilityActionAttribute ActionType => AbilityActionAttribute.Hold;

        public override AbilityTargetType TargetType => AbilityTargetType.SelfTarget;

        public override string Name => "Молитва";
        
        public override ScaleRate Scale => ScaleRate.Build(Dungeon.Entities.Enums.Scale.AttackDamage);

        protected override bool CanUse(Servant @class) => true;

        private PrayerBuff holdedBuf = null;

        protected override void Use(GameMap gameMap, Avatar avatar, Servant @class)
        {
            @class.Serve = true;
            holdedBuf = new PrayerBuff(@class);
            avatar.AddState(holdedBuf);
        }

        protected override void Dispose(GameMap gameMap, Avatar avatar, Servant @class)
        {
            if (holdedBuf != null)
            {
                @class.Serve = false;
                avatar.RemoveState(holdedBuf);
                holdedBuf = null;
            }
        }

        private class PrayerEffect : SceneObject
        {
            public PrayerEffect()
            {
                this.Width = 1;
                this.Height = 1;

                this.Top = 0.5;
                this.Left = 0.4;

                this.Effects.Add(new ParticleEffect()
                {
                    Name="Prayer",
                    Scale=0.2
                });
            }
        }

        /// <summary>
        /// так то бафы должны действовать и на аватар тоже
        /// </summary>
        private class PrayerBuff : Applicable
        {
            private Servant _servant;
            public PrayerBuff(Servant servant) => _servant = servant;

            public override string Image => "Abilities/Prayer/buf.png".AsmImgRes();

            IDisposable bufTick;

            private PrayerEffect effect;

            public void Apply(Avatar avatar)
            {
                effect = new PrayerEffect();

                avatar.Flow(a => a.AddEffect(true), new { Effects = effect.InList<ISceneObject>() });

                bufTick = Dungeon.Global.Time.Timer(nameof(PrayerBuff) + avatar.Name)
                    .After(3000)
                    .Repeat()
                    .Do(() => _servant.FaithPower.Value++)
                    .Auto();
            }

            public void Discard(Avatar avatar)
            {
                effect.Destroy?.Invoke();
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

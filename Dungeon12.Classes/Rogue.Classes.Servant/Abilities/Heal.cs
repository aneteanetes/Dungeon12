using Rogue.Abilities;
using Rogue.Abilities.Enums;
using Rogue.Abilities.Scaling;
using Rogue.Drawing.Impl;
using Rogue.Drawing.SceneObjects;
using Rogue.Map;
using Rogue.Map.Objects;
using Rogue.Transactions;
using Rogue.View.Interfaces;
using System;

namespace Dungeon12.Classes.Servant.Abilities
{
    public class Heal : Ability<Servant>
    {
        public override Cooldown Cooldown { get; } = new Cooldown(1500, nameof(Heal));

        public override AbilityPosition AbilityPosition => AbilityPosition.Q;

        public override AbilityActionAttribute ActionType => AbilityActionAttribute.DmgHealInstant;

        public override AbilityTargetType TargetType => AbilityTargetType.TargetFrendly;

        public override string Name => "Исцеление";

        public override ScaleRate Scale => ScaleRate.Build(Entites.Enums.Scale.AttackDamage);

        protected override bool CanUse(Servant @class) => !@class.Serve && @class.FaithPower.Value >= 1;

        protected override void Use(GameMap gameMap, Avatar avatar, Servant @class)
        {
            @class.FaithPower.Value--;

            long val = 10;
            Target.HitPoints += val;
            Global.AudioPlayer.Effect(@"\heal".AsmName());
            Target.Flow(t => t.AddEffect(true), new { Effects = new HealEffect().InList<ISceneObject>() });
        }

        private class HealEffect : SceneObject
        {
            public HealEffect()
            {
                this.Width = 1;
                this.Height = 1.5;

                this.Left = 0.4;
                this.Top = 0.3;

                this.Effects.Add(new ParticleEffect()
                {
                    Name="Heal",
                    Scale=0.2,
                    Assembly="".AsmName()
                });

                Global.Time.Timer(Guid.NewGuid().ToString())
                    .After(1700)
                    .Do(() => this.Destroy?.Invoke())
                    .Auto();
            }
        }

        protected override void Dispose(GameMap gameMap, Avatar avatar, Servant @class)
        {
        }

        public override AbilityCastType CastType => AbilityCastType.Active;

        public override Location CastLocation => Location.Combat;
    }
}

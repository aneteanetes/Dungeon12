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
    public class Heal : Ability<Servant>
    {
        public override Cooldown Cooldown { get; } = new Cooldown(1500, nameof(Heal));

        public override AbilityPosition AbilityPosition => AbilityPosition.Q;

        public override AbilityActionAttribute ActionType => AbilityActionAttribute.DmgHealInstant;

        public override AbilityTargetType TargetType => AbilityTargetType.TargetFrendly;

        public override string Name => "Исцеление";

        public override ScaleRate Scale => ScaleRate.Build(Dungeon.Entites.Enums.Scale.AttackDamage);

        protected override bool CanUse(Servant @class) => !@class.Serve && @class.FaithPower.Value >= 1;

        protected override void Use(GameMap gameMap, Avatar avatar, Servant @class)
        {
            @class.FaithPower.Value--;

            long val = 10;
            Target.HitPoints += val;
            Global.AudioPlayer.Effect(@"Audio\Sound\heal.wav".AsmNameRes());
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
                    Scale=0.2
                });

                Dungeon.Global.Time.Timer(Guid.NewGuid().ToString())
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

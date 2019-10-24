using Rogue.Abilities;
using Rogue.Abilities.Enums;
using Rogue.Abilities.Scaling;
using Rogue.Map;
using Rogue.Map.Objects;
using Rogue.Transactions;
using System;

namespace Rogue.Classes.Servant.Abilities
{
    public class Heal : Ability<Servant>
    {
        public override Cooldown Cooldown { get; } = new Cooldown(1500, nameof(Heal));

        public override AbilityPosition AbilityPosition => AbilityPosition.Right;

        public override AbilityActionAttribute ActionType => AbilityActionAttribute.DmgHealInstant;

        public override AbilityTargetType TargetType => AbilityTargetType.TargetAndNonTarget;

        public override string Name => "Исцеление";

        public override ScaleRate Scale => ScaleRate.Build(Entites.Enums.Scale.AttackDamage);

        protected override bool CanUse(Servant @class) => @class.FaithPower.Value >= 1;

        protected override void Use(GameMap gameMap, Avatar avatar, Servant @class)
        {
            @class.FaithPower.Value--;

            long val = 10;
            var res = @class.HitPoints + val;

            if (res > @class.MaxHitPoints)
            {
                @class.HitPoints = @class.MaxHitPoints;
            }
            else
            {
                @class.HitPoints += val;
            }
        }

        protected override void Dispose(GameMap gameMap, Avatar avatar, Servant @class)
        {
        }

        public override AbilityCastType CastType => AbilityCastType.Active;

        public override Location CastLocation => Location.Combat;
    }
}

using Rogue.Abilities;
using Rogue.Abilities.Enums;
using Rogue.Abilities.Scaling;
using Rogue.Map;
using Rogue.Map.Objects;
using Rogue.View.Interfaces;

namespace Rogue.Classes.Servant.Abilities
{
    public class Сonsecration : BaseCooldownAbility<Servant>
    {
        public override Cooldown Cooldown { get; } = BaseCooldown.Chain(2000, nameof(Servant)).Build();

        public override AbilityPosition AbilityPosition => AbilityPosition.E;

        public override string Name => "Ливень стрел";

        public override ScaleRate Scale => ScaleRate.Build(Entites.Enums.Scale.AbilityPower);

        public override AbilityActionAttribute ActionType => AbilityActionAttribute.DmgHealInstant;

        public override AbilityTargetType TargetType => AbilityTargetType.NonTarget;

        protected override bool CanUse(Servant @class) => @class.FaithPower.Value >= 3;

        protected override void Dispose(GameMap gameMap, Avatar avatar, Servant @class)
        {
        }

        protected override void Use(GameMap gameMap, Avatar avatar, Servant @class)
        {
            @class.FaithPower.Value -= 3;
        }
    }
}

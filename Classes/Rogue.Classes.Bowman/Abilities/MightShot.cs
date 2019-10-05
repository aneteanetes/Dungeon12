using Rogue.Abilities;
using Rogue.Abilities.Enums;
using Rogue.Abilities.Scaling;
using Rogue.Map;
using Rogue.Map.Objects;

namespace Rogue.Classes.Bowman.Abilities
{
    public class MightShot : BaseCooldownAbility
    {
        public override AbilityPosition AbilityPosition => AbilityPosition.Right;

        public override string Name => "Сильный выстрел";

        public override ScaleRate Scale => ScaleRate.Build(Entites.Enums.Scale.AttackDamage);

        public override AbilityActionAttribute ActionType => AbilityActionAttribute.DmgHealInstant;
        
        public override AbilityTargetType TargetType => AbilityTargetType.NonTarget;

        protected override bool CanUse(Bowman @class)
        {
            return @class.Energy.RightHand >= 15;
        }

        protected override void Dispose(GameMap gameMap, Avatar avatar, Bowman @class)
        {
        }

        protected override void Use(GameMap gameMap, Avatar avatar, Bowman @class)
        {
            @class.Energy.RightHand -= 15;
        }
    }
}

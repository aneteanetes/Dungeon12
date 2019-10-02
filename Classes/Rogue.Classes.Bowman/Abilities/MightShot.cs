using Rogue.Abilities;
using Rogue.Abilities.Enums;
using Rogue.Abilities.Scaling;
using Rogue.Map;
using Rogue.Map.Objects;

namespace Rogue.Classes.Bowman.Abilities
{
    public class MightShot : Ability<Bowman>
    {
        public override AbilityPosition AbilityPosition => AbilityPosition.Right;

        public override string Name => "Сильный выстрел";

        public override ScaleRate Scale => ScaleRate.Build(Entites.Enums.Scale.AttackDamage);

        protected override bool CanUse(Bowman @class)
        {
            return false;
        }

        protected override void Dispose(GameMap gameMap, Avatar avatar, Bowman @class)
        {
        }

        protected override void Use(GameMap gameMap, Avatar avatar, Bowman @class)
        {
        }
    }
}

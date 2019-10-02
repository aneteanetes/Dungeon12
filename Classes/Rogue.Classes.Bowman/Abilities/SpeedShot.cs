using Rogue.Abilities;
using Rogue.Abilities.Enums;
using Rogue.Abilities.Scaling;
using Rogue.Map;
using Rogue.Map.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Classes.Bowman.Abilities
{
    public class SpeedShot : Ability<Bowman>
    {
        public override AbilityPosition AbilityPosition => AbilityPosition.Left;

        public override string Name => "Быстрый выстрел";

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

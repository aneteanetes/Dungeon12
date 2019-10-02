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
    public class RainOfArrows : Ability<Bowman>
    {
        public override AbilityPosition AbilityPosition => AbilityPosition.Q;

        public override string Name => "Ливень стрел";

        public override ScaleRate Scale => ScaleRate.Build(Entites.Enums.Scale.AbilityPower);

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

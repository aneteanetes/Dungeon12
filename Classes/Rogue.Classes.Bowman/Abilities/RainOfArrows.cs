using Rogue.Abilities;
using Rogue.Abilities.Enums;
using Rogue.Abilities.Scaling;
using Rogue.Classes.Bowman.Effects;
using Rogue.Map;
using Rogue.Map.Objects;
using Rogue.View.Interfaces;
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

        public override AbilityActionAttribute ActionType => AbilityActionAttribute.DmgHealInstant;
        public override AbilityTargetType TargetType => AbilityTargetType.NonTarget;

        protected override bool CanUse(Bowman @class)
        {
            return true;
        }

        protected override void Dispose(GameMap gameMap, Avatar avatar, Bowman @class)
        {
        }

        protected override void Use(GameMap gameMap, Avatar avatar, Bowman @class)
        {
            @class.Energy.LeftHand -= 15;
            @class.Energy.RightHand -= 15;
            
            this.UseEffects(new ArrowRain(avatar.Position).InList<ISceneObject>());
        }
    }
}

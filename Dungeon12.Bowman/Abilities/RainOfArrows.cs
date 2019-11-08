using Dungeon.Abilities;
using Dungeon.Abilities.Enums;
using Dungeon.Abilities.Scaling;
using Dungeon12.Bowman.Effects;
using Dungeon.Map;
using Dungeon.Map.Objects;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Dungeon;

namespace Dungeon12.Bowman.Abilities
{
    public class RainOfArrows : BaseCooldownAbility<Bowman>
    {
        public override Cooldown Cooldown { get; } = BaseCooldown.Chain(6500, nameof(RainOfArrows)).Build();

        public override AbilityPosition AbilityPosition => AbilityPosition.Q;

        public override string Name => "Ливень стрел";

        public override ScaleRate Scale => ScaleRate.Build(Dungeon.Entities.Enums.Scale.AbilityPower);

        public override AbilityActionAttribute ActionType => AbilityActionAttribute.DmgHealInstant;

        public override AbilityTargetType TargetType => AbilityTargetType.NonTarget;

        private Bowman rangeclass;
        protected override bool CanUse(Bowman @class)
        {
            rangeclass = @class;
            return @class.Energy.RightHand >= 35 && @class.Energy.LeftHand >= 35;
        }

        protected override void Dispose(GameMap gameMap, Avatar avatar, Bowman @class)
        {
        }

        protected override void Use(GameMap gameMap, Avatar avatar, Bowman @class)
        {
            @class.Energy.LeftHand -= 35;
            @class.Energy.RightHand -= 35;
            
            this.UseEffects(new ArrowRain(@class,3000, PointerLocation.GameCoordinates, gameMap).InList<ISceneObject>());
        }
    }
}

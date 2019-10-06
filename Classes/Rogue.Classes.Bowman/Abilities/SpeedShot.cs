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
    public class SpeedShot : BaseCooldownAbility
    {
        public override AbilityPosition AbilityPosition => AbilityPosition.Left;

        public override AbilityActionAttribute ActionType => AbilityActionAttribute.DmgHealInstant;

        public override AbilityTargetType TargetType => AbilityTargetType.TargetAndNonTarget;

        public override string Name => "Быстрый выстрел";

        public override ScaleRate Scale => ScaleRate.Build(Entites.Enums.Scale.AttackDamage);

        private Bowman rangeclass;
        protected override bool CanUse(Bowman @class)
        {
            rangeclass = @class;
            return @class.Energy.LeftHand >= 15;
        }

        protected override double RangeMultipler => (4 + rangeclass?.Range ?? 0)*2.5;

        protected override void Dispose(GameMap gameMap, Avatar avatar, Bowman @class)
        {
        }

        protected override void Use(GameMap gameMap, Avatar avatar, Bowman @class)
        {
            @class.Energy.LeftHand -= 15;

            var arrow = new ArrowObject(avatar.VisionDirection, 4 + @class.Range, 15, 0.05);

            this.UseEffects(new Arrow(gameMap, arrow, avatar.VisionDirection,new Types.Point(avatar.Position.X / 32, avatar.Position.Y / 32)).InList<ISceneObject>());
        }
    }
}
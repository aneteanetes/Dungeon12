using Dungeon;
using Dungeon.View.Interfaces;
using Dungeon12.Abilities;
using Dungeon12.Abilities.Enums;
using Dungeon12.Abilities.Scaling;
using Dungeon12.Bowman.Effects;
using Dungeon12.Map;
using Dungeon12.Map.Objects;

namespace Dungeon12.Bowman.Abilities
{
    public class SpeedShot : BaseCooldownAbility<Bowman>
    {
        public override AbilityPosition AbilityPosition => AbilityPosition.Left;

        public override AbilityActionAttribute ActionType => AbilityActionAttribute.DmgHealInstant;

        public override AbilityTargetType TargetType => AbilityTargetType.TargetAndNonTarget;

        public override string Name => "Быстрый выстрел";

        public override ScaleRate Scale => ScaleRate.Build(Dungeon12.Entities.Enums.Scale.AttackDamage);

        private Bowman rangeclass;
        protected override bool CanUse(Bowman @class)
        {
            rangeclass = @class;
            return @class.Energy.LeftHand >= 15;
        }

        protected override double RangeMultipler => (4 + (rangeclass?.Range ?? 0))*2.5;

        protected override void Dispose(GameMap gameMap, Avatar avatar, Bowman @class)
        {
        }

        protected override void Use(GameMap gameMap, Avatar avatar, Bowman @class)
        {
            @class.Energy.LeftHand -= 15;

            var baseSpeed = 0.045;
            var speed = baseSpeed;

            if (@class.AttackSpeed > 0)
            {
                speed+=@class.AttackSpeed / 1000d;
            }

            var range = @class.Range / 15;

            var arrow = new ArrowObject(avatar.VisionDirection, 4 + range, 15, speed);

            this.UseEffects(new Arrow(@class,gameMap, arrow, avatar.VisionDirection,new Dungeon.Types.Point(avatar.Position.X / 32, avatar.Position.Y / 32)).InList<ISceneObject>());
        }
    }
}
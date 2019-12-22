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

        public override string Description => "Выпускает стрелу в направлении врага. У навыка небольшой срок восстановления, но маленький урон. На навык влияет скорость атаки и радиус. Использует натяжение левой руки.";

        public override string Name => "Быстрый выстрел";

        public override ScaleRate<Bowman> Scale => new ScaleRate<Bowman>(x => x.AttackDamage * .6);

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

        public override long Value => 5;

        protected override void Use(GameMap gameMap, Avatar avatar, Bowman @class)
        {
            Global.AudioPlayer.Effect("arrow.wav".AsmSoundRes());
            @class.Energy.LeftHand -= 15;

            var baseSpeed = 0.045;
            var speed = baseSpeed;

            if (@class.AttackSpeed > 0)
            {
                speed+=@class.AttackSpeed / 1000d;
            }

            var range = @class.Range / 15;

            var dmg = ScaledValue(@class);

            var arrow = new ArrowObject(avatar.VisionDirection, 4 + range, dmg, speed);

            this.UseEffects(new Arrow(@class,gameMap, arrow, avatar.VisionDirection,new Dungeon.Types.Point(avatar.Position.X / 32, avatar.Position.Y / 32)).InList<ISceneObject>());
        }
    }
}
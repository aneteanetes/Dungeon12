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
    public class MightShot : BaseCooldownAbility<Bowman>
    {
        public override Cooldown Cooldown { get; } = BaseCooldown.Chain(3000, nameof(MightShot)).Build();

        public override AbilityPosition AbilityPosition => AbilityPosition.Right;

        public override AbilityActionAttribute ActionType => AbilityActionAttribute.DmgHealInstant;

        public override AbilityTargetType TargetType => AbilityTargetType.TargetAndNonTarget;

        public override string Description => "Выпускает стрелу с большим натяжением в направлении врага. У навыка большой срок восстановления но сильный урон. На навык влияет скорость атаки и радиус. Использует натяжение правой руки.";

        public override string Name => "Сильный выстрел";

        public override ScaleRate<Bowman> Scale => new ScaleRate<Bowman>(x => x.AttackDamage * 1.7);

        private Bowman rangeclass;
        protected override bool CanUse(Bowman @class)
        {
            rangeclass = @class;
            return @class.Energy.RightHand >= 15;
        }

        protected override double RangeMultipler => (4 + (rangeclass?.Range ?? 0)) * 2.5;

        protected override void Dispose(GameMap gameMap, Avatar avatar, Bowman @class)
        {
        }

        public override long Value => 10;

        protected override void Use(GameMap gameMap, Avatar avatar, Bowman @class)
        {
            @class.Energy.RightHand -= 15;

            var baseSpeed = 0.045;
            var speed = baseSpeed;

            if (@class.AttackSpeed > 0)
            {
                speed += @class.AttackSpeed / 1000d;
            }
            var range = @class.Range / 15;

            var dmg = ScaledValue(@class);

            var arrow = new ArrowObject(avatar.VisionDirection, 4 + range, dmg, speed);

            this.UseEffects(new Arrow(@class, gameMap, arrow, avatar.VisionDirection, new Dungeon.Types.Point(avatar.Position.X / 32, avatar.Position.Y / 32), true).InList<ISceneObject>());
        }
    }
}
using Rogue.Abilities;
using Rogue.Abilities.Enums;
using Rogue.Abilities.Scaling;
using Rogue.Classes.Bowman.Effects;
using Rogue.Map;
using Rogue.Map.Objects;
using Rogue.View.Interfaces;

namespace Rogue.Classes.Bowman.Abilities
{
    public class MightShot : BaseCooldownAbility<Bowman>
    {
        public override Cooldown Cooldown { get; } = BaseCooldown.Chain(3000, nameof(MightShot)).Build();

        public override AbilityPosition AbilityPosition => AbilityPosition.Right;

        public override AbilityActionAttribute ActionType => AbilityActionAttribute.DmgHealInstant;

        public override AbilityTargetType TargetType => AbilityTargetType.TargetAndNonTarget;

        public override string Name => "Сильный выстрел";

        public override ScaleRate Scale => ScaleRate.Build(Entites.Enums.Scale.AttackDamage);

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

        protected override void Use(GameMap gameMap, Avatar avatar, Bowman @class)
        {
            @class.Energy.RightHand -= 15;

            var arrow = new ArrowObject(avatar.VisionDirection, 4 + @class.Range, 27, 0.045);

            this.UseEffects(new Arrow(gameMap, arrow, avatar.VisionDirection, new Types.Point(avatar.Position.X / 32, avatar.Position.Y / 32),true).InList<ISceneObject>());
        }
    }
}

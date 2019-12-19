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
    public class RainOfArrows : BaseCooldownAbility<Bowman>
    {
        public override Cooldown Cooldown { get; } = BaseCooldown.Chain(6500, nameof(RainOfArrows)).Build();

        public override AbilityPosition AbilityPosition => AbilityPosition.Q;

        public override string Name => "Ливень стрел";

        public override ScaleRate<Bowman> Scale => new ScaleRate<Bowman>(x => x.AttackDamage * 0.6, x => x.AttackSpeed * 0.8);

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

        public override long Value => Global.GameState.Character.Level * 2;

        protected override void Use(GameMap gameMap, Avatar avatar, Bowman @class)
        {
            @class.Energy.LeftHand -= 35;
            @class.Energy.RightHand -= 35;

            var range = (@class.Range / 15) + 5;

            var destination = avatar.Location.CalculatePath(PointerLocation.GameCoordinates, range, 0.5);

            var minDamage = ScaledValue(@class);
            var maxDamage = minDamage * 5;

            this.UseEffects(new ArrowRain(@class, 3000, destination, gameMap)
            {
                MinDmg=minDamage,
                MaxDmg=maxDamage
            }.InList<ISceneObject>());
        }
    }
}
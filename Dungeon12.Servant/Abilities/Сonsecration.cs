using Dungeon;
using Dungeon.View.Interfaces;
using Dungeon12.Abilities;
using Dungeon12.Abilities.Enums;
using Dungeon12.Abilities.Scaling;
using Dungeon12.Map;
using Dungeon12.Map.Objects;
using Dungeon12.Servant.Effects.Сonsecration;

namespace Dungeon12.Servant.Abilities
{
    public class Сonsecration : BaseCooldownAbility<Servant>
    {
        public override Cooldown Cooldown { get; } = BaseCooldown.Chain(2000, nameof(Servant)).Build();

        public override AbilityPosition AbilityPosition => AbilityPosition.E;

        public override string Name => "Освящение";

        //public override ScaleRate Scale => ScaleRate.Build(Dungeon12.Entities.Enums.Scale.AbilityPower);

        public override AbilityActionAttribute ActionType => AbilityActionAttribute.DmgHealInstant;

        public override AbilityTargetType TargetType => AbilityTargetType.NonTarget;

        protected override bool CanUse(Servant @class) => !@class.Serve && @class.FaithPower.Value >= 3;

        protected override void Dispose(GameMap gameMap, Avatar avatar, Servant @class)
        {
        }

        protected override void Use(GameMap gameMap, Avatar avatar, Servant @class)
        {
            @class.FaithPower.Value -= 3;

            this.UseEffects(new СonsecrationCircle(gameMap, avatar)
            {
                Left = (avatar.Position.X / 32) - 1.25,
                Top = (avatar.Position.Y / 32)-0.25
            }.Init(5000).InList<ISceneObject>());
        }
    }
}

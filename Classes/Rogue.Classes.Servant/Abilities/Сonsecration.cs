using Rogue.Abilities;
using Rogue.Abilities.Enums;
using Rogue.Abilities.Scaling;
using Rogue.Classes.Servant.Effects.Сonsecration;
using Rogue.Map;
using Rogue.Map.Objects;
using Rogue.View.Interfaces;

namespace Rogue.Classes.Servant.Abilities
{
    public class Сonsecration : BaseCooldownAbility<Servant>
    {
        public override Cooldown Cooldown { get; } = BaseCooldown.Chain(2000, nameof(Servant)).Build();

        public override AbilityPosition AbilityPosition => AbilityPosition.E;

        public override string Name => "Освящение";

        public override ScaleRate Scale => ScaleRate.Build(Entites.Enums.Scale.AbilityPower);

        public override AbilityActionAttribute ActionType => AbilityActionAttribute.DmgHealInstant;

        public override AbilityTargetType TargetType => AbilityTargetType.NonTarget;

        protected override bool CanUse(Servant @class) => !@class.Serve && @class.FaithPower.Value >= 3;

        protected override void Dispose(GameMap gameMap, Avatar avatar, Servant @class)
        {
        }

        protected override void Use(GameMap gameMap, Avatar avatar, Servant @class)
        {
            @class.FaithPower.Value -= 3;

            this.UseEffects(new СonsecrationCircle(gameMap, avatar, 5000)
            {
                Left = (avatar.Position.X / 32) - 1.5,
                Top = (avatar.Position.Y / 32) - 0.75
            }.InList<ISceneObject>());
        }
    }
}

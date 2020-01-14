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

        public override string Description => "Освещает область под персонажем. Тратит 3 Печати. Освященная область существует 5 секунд и увеличивает физическую и магическую защиту внутри области.";

        public override ScaleRate<Servant> Scale => new ScaleRate<Servant>(x => x.Defence * 3.17, x => x.Barrier * 3.12);

        public override AbilityActionAttribute ActionType => AbilityActionAttribute.DmgHealInstant;

        public override AbilityTargetType TargetType => AbilityTargetType.NonTarget;

        protected override bool CanUse(Servant @class) => !@class.Serve && @class.FaithPower.Value >= 3;

        protected override void Dispose(GameMap gameMap, Avatar avatar, Servant @class)
        {
        }

        public override long Value => 1;

        public override string Spend => "Использует: 3 Печати";

        protected override void Use(GameMap gameMap, Avatar avatar, Servant @class)
        {
            @class.FaithPower.Value -= 3;

            this.UseEffects(new СonsecrationCircle(gameMap, avatar, ScaledValue(@class))
            {
                Left = (avatar.Position.X / 32) - 1.25,
                Top = (avatar.Position.Y / 32) - 0.25
            }.Init(5000).InList<ISceneObject>());
            Global.AudioPlayer.Effect("concentration.wav".AsmSoundRes());
        }
    }
}

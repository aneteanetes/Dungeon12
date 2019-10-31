using Rogue.Abilities;
using Rogue.Abilities.Enums;
using Rogue.Abilities.Scaling;
using Rogue.Classes.Servant.Effects.FaithShot;
using Rogue.Map;
using Rogue.Map.Objects;
using Rogue.Physics;
using Rogue.View.Interfaces;

namespace Dungeon12.Classes.Servant.Abilities
{
    public class FaithShot : BaseCooldownAbility<Servant>
    {
        public override Cooldown Cooldown { get; } = BaseCooldown.Chain(2500, nameof(FaithShot)).Build();

        public override AbilityPosition AbilityPosition => AbilityPosition.Left;

        public override AbilityActionAttribute ActionType => AbilityActionAttribute.DmgHealInstant;

        public override AbilityTargetType TargetType => AbilityTargetType.Target;

        public override string Name => "Удар веры";

        public override ScaleRate Scale => ScaleRate.Build(Entites.Enums.Scale.AttackDamage);

        protected override bool CanUse(Servant @class) => !@class.Serve;
        
        protected override void Dispose(GameMap gameMap, Avatar avatar, Servant @class)
        {
        }

        protected override double RangeMultipler => 4;

        protected override void Use(GameMap gameMap, Avatar avatar, Servant @class)
        {            
            var rangeObject = new MapObject
            {
                Position = new PhysicalPosition
                {
                    X = avatar.Position.X - 32,
                    Y = avatar.Position.Y - 32
                },
                Size = new PhysicalSize()
                {
                    Height = 128,
                    Width = 128
                }
            };

            var enemy = gameMap.One<Mob>(rangeObject);
            if (enemy != default)
            {
                @class.FaithPower.Value++;
                this.UseEffects(new Smash(avatar).InList<ISceneObject>());
                enemy.Flow(t => t.Damage(true), new { Damage = 20l });
            }
        }
    }
}

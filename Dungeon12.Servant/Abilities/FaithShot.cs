using Dungeon.Abilities;
using Dungeon.Abilities.Enums;
using Dungeon.Abilities.Scaling;
using Dungeon12.Servant.Effects.FaithShot;
using Dungeon.Map;
using Dungeon.Map.Objects;
using Dungeon.Physics;
using Dungeon.View.Interfaces;
using Dungeon;

namespace Dungeon12.Servant.Abilities
{
    public class FaithShot : BaseCooldownAbility<Servant>
    {
        public override Cooldown Cooldown { get; } = BaseCooldown.Chain(2500, nameof(FaithShot)).Build();

        public override AbilityPosition AbilityPosition => AbilityPosition.Left;

        public override AbilityActionAttribute ActionType => AbilityActionAttribute.DmgHealInstant;

        public override AbilityTargetType TargetType => AbilityTargetType.Target;

        public override string Name => "Удар веры";

        public override ScaleRate Scale => ScaleRate.Build(Dungeon.Entities.Enums.Scale.AttackDamage);

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
                enemy.Flow(t => t.Damage(true), new { Damage = 20L });
            }
        }
    }
}

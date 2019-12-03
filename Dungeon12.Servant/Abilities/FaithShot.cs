using Dungeon.Abilities;
using Dungeon.Abilities.Enums;
using Dungeon.Abilities.Scaling;
using Dungeon12.Servant.Effects.FaithShot;
using Dungeon.Map;
using Dungeon.Map.Objects;
using Dungeon.Physics;
using Dungeon.View.Interfaces;
using Dungeon;
using Dungeon.Entities.Alive;
using Dungeon12.Map.Objects;

namespace Dungeon12.Servant.Abilities
{
    public class FaithShot : BaseCooldownAbility<Servant>
    {
        public override Cooldown Cooldown { get; } = BaseCooldown.Chain(2500, nameof(FaithShot)).Build();

        public override AbilityPosition AbilityPosition => AbilityPosition.Left;

        public override AbilityActionAttribute ActionType => AbilityActionAttribute.DmgHealInstant;

        public override AbilityTargetType TargetType => AbilityTargetType.TargetAndNonTarget;

        public override string Name => "Удар веры";

        public override ScaleRate Scale => ScaleRate.Build(Dungeon.Entities.Enums.Scale.AttackDamage);

        protected override bool CanUse(Servant @class) => !@class.Serve;
        
        protected override void Dispose(GameMap gameMap, Avatar avatar, Servant @class)
        {
        }

        protected override double RangeMultipler => 4;

        protected override void Use(GameMap gameMap, Avatar avatar, Servant @class)
        {
            var rangeObject = avatar.Grow(4);

            var enemy = gameMap.One<NPCMap>(rangeObject,x=>x.IsEnemy);
            if (enemy != default)
            {
                @class.FaithPower.Value++;
                this.UseEffects(new Smash(avatar).InList<ISceneObject>());
                enemy.Entity.Damage(@class,new Dungeon.Entities.Alive.Damage()
                {
                    Amount=20,
                    Type= DamageType.HolyMagic
                });
            }
        }
    }
}

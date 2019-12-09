namespace Dungeon12.Noone.Abilities
{
    using Dungeon12.Abilities;
    using Dungeon12.Abilities.Enums;
    using Dungeon12.Abilities.Scaling;
    using Dungeon12.Noone.Talants;
    using Dungeon12.SceneObjects; using Dungeon.SceneObjects;
    using Dungeon12.Map;
    using Dungeon12.Map.Objects;
    using Dungeon.Physics;
    using Dungeon.View.Interfaces;
    using System;using Dungeon;using Dungeon.Drawing.SceneObjects;
    using System.Collections.Generic;
    using System.Linq;
    using Dungeon12.Entities.Alive;
    using Dungeon12.Map.Objects;

    public class Attack : Ability<Noone,AbsorbingTalants>
    {
        public const string AttackCooldown = nameof(Noone) + nameof(Attack);

        public override int Position => 0;

        public override double Spend => 1;

        public override string Name => "Атака";

        public override AbilityTargetType TargetType => AbilityTargetType.TargetAndNonTarget;

        public override Cooldown Cooldown { get; } = new Cooldown(500, AttackCooldown);

        public override ScaleRate Scale => ScaleRate.Build(Dungeon12.Entities.Enums.Scale.AbilityPower, 0.1);

        public override AbilityPosition AbilityPosition => AbilityPosition.Left;

        protected override bool CanUse(Noone @class)=> @class.Actions > 0;

        protected override double RangeMultipler => 2.5;

        public NPCMap AttackedEnemy { get; set; }

        protected override void Use(GameMap gameMap, Avatar avatar, Noone @class)
        {
            @class.InParry = true;
            Global.AudioPlayer.Effect("attack".NooneSoundPath());

            var rangeObject = avatar.Grow(2.5);

            var enemy = gameMap.Enemies(rangeObject).FirstOrDefault();

            if (enemy != null)
            {
                @class.Actions -= 1;
                var value = (long)this.Value;

                enemy.Entity.Damage(@class, new Dungeon12.Entities.Alive.Damage()
                {
                    Amount=value,
                    Type = DamageType.Physical
                });
                
                AttackedEnemy = enemy;
            }
            @class.InParry = false;
        }

        protected override void Dispose(GameMap gameMap, Avatar avatar, Noone @class) { }

        public override double Value => Dungeon.RandomDungeon.Next(10,30);

        public override AbilityActionAttribute ActionType => AbilityActionAttribute.EffectInstant;

        public override AbilityCastType CastType => AbilityCastType.Active;

        public override Location CastLocation => Location.Combat;

        public override string Description => $"Атакует врага нанося двойной урон {Environment.NewLine}оружием в правой руке. {Environment.NewLine}Может наносить критический урон.";
    }
}
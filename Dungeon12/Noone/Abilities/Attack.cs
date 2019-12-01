namespace Dungeon12.Noone.Abilities
{
    using Dungeon.Abilities;
    using Dungeon.Abilities.Enums;
    using Dungeon.Abilities.Scaling;
    using Dungeon12.Noone.Talants;
    using Dungeon.SceneObjects;
    using Dungeon.Map;
    using Dungeon.Map.Objects;
    using Dungeon.Physics;
    using Dungeon.View.Interfaces;
    using System;using Dungeon;using Dungeon.Drawing.SceneObjects;
    using System.Collections.Generic;
    using System.Linq;
    using Dungeon.Entities.Alive;
    using Dungeon12.Map.Objects;

    public class Attack : Ability<Noone,AbsorbingTalants>
    {
        public const string AttackCooldown = nameof(Noone) + nameof(Attack);

        public override int Position => 0;

        public override double Spend => 1;

        public override string Name => "Атака";

        public override AbilityTargetType TargetType => AbilityTargetType.TargetAndNonTarget;

        public override Cooldown Cooldown { get; } = new Cooldown(500, AttackCooldown);

        public override ScaleRate Scale => ScaleRate.Build(Dungeon.Entities.Enums.Scale.AbilityPower, 0.1);

        public override AbilityPosition AbilityPosition => AbilityPosition.Left;

        protected override bool CanUse(Noone @class)=> @class.Actions > 0;

        protected override double RangeMultipler => 2.5;

        public NPCMap AttackedEnemy { get; set; }

        protected override void Use(GameMap gameMap, Avatar avatar, Noone @class)
        {
            @class.InParry = true;
            Global.AudioPlayer.Effect("attack".NooneSoundPath());

            var rangeObject = new MapObject
            {
                Position = new PhysicalPosition
                {
                    X = avatar.Position.X - ((avatar.Size.Width * 2.5) / 2),
                    Y = avatar.Position.Y - ((avatar.Size.Height * 2.5) / 2)
                },
                Size = avatar.Size
            };

            rangeObject.Size.Height *= 2.5;
            rangeObject.Size.Width *= 2.5;

            var enemy = gameMap.Enemies(rangeObject).FirstOrDefault();

            if (enemy != null)
            {
                @class.Actions -= 1;
                var value = (long)this.Value;

                enemy.Entity.Damage(@class, new Dungeon.Entities.Alive.Damage()
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
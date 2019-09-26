namespace Rogue.Classes.Noone.Abilities
{
    using Rogue.Abilities;
    using Rogue.Abilities.Enums;
    using Rogue.Abilities.Scaling;
    using Rogue.Drawing.GUI;
    using Rogue.Map;
    using Rogue.Map.Objects;
    using Rogue.Physics;
    using Rogue.View.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Attack : Ability<Noone>
    {
        public override int Position => 0;

        public override double Spend => 1;

        public override string Name => "Атака";

        public override AbilityTargetType TargetType => AbilityTargetType.TargetAndNonTarget;

        public override ScaleRate Scale => ScaleRate.Build(Entites.Enums.Scale.AbilityPower, 0.1);

        public override AbilityPosition AbilityPosition => AbilityPosition.Left;

        protected override bool CanUse(Noone @class)=> @class.Actions > 0;

        protected override double RangeMultipler => 2.5;

        protected override void Use(GameMap gameMap, Avatar avatar, Noone @class)
        {
            Global.AudioPlayer.Effect("attack");

            var rangeObject = new MapObject
            {
                Position = new Physics.PhysicalPosition
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

                enemy.Enemy.HitPoints -= value;

                Global.AudioPlayer.Effect(enemy.DamageSound ?? "bat");

                if (enemy.Enemy.HitPoints <= 0)
                {
                    enemy.Die?.Invoke();

                    var expr = RandomRogue.Next(4, 16);
                    avatar.Character.EXP += expr;
                    this.UseEffects(new List<ISceneObject>()
                    {
                        new PopupString($"Вы получаете {expr} опыта!", ConsoleColor.DarkMagenta,avatar.Location,25, 19,0.06)
                    });
                }

                var critical = value > 10;

                this.UseEffects(new List<ISceneObject>()
                {
                    new PopupString(value.ToString()+(critical ? "!" : ""), critical ? ConsoleColor.Red : ConsoleColor.White,enemy.Location,25,critical ? 19 : 17,0.06)
                });
            }
        }

        protected override void Dispose(GameMap gameMap, Avatar avatar, Noone @class) { }

        public override double Value => Rogue.RandomRogue.Next(30,170);

        public override AbilityActionAttribute ActionType => AbilityActionAttribute.EffectInstant;

        public override AbilityCastType CastType => AbilityCastType.Active;

        public override Location CastLocation => Location.Combat;

        public override string Description => $"Атакует врага нанося двойной урон {Environment.NewLine}оружием в правой руке. {Environment.NewLine}Может наносить критический урон.";
    }
}
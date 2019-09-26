using Rogue.Abilities;
using Rogue.Abilities.Talants;
using Rogue.Drawing.GUI;
using Rogue.Entites.Enemy;
using Rogue.Map;
using Rogue.Map.Objects;
using Rogue.Transactions;
using Rogue.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Classes.Noone.Talants.ElementalShield
{
    public class PoisonShieldTalant : Talant<Noone>
    {
        public override string Name => "Щит яда";

        public override string Description => "Увеличивает магическую защиту и отравляет всех в радиусе 1 клетки";

        public override bool CanUse(Noone @class, Ability ability) => @class.Actions >= 4;

        public override bool Dispose(GameMap gameMap, Avatar avatar, Noone @class, Action<GameMap, Avatar, Noone> @base, Ability ability) => false;

        public override bool Use(GameMap gameMap, Avatar avatar, Noone @class, Action<GameMap, Avatar, Noone> @base, Ability ability)
        {
            @class.Actions -= 1;

            //возможно нужны сборные аппликативные эффекты

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

            var enemies = gameMap.Enemies(rangeObject);

            foreach (var enemy in enemies)
            {
                var value = (long)ability.Value;

                var debuff = new PoisonDebuffPoisonShield(value * 2, ability, avatar);
                enemy.AddState(debuff);

                Global.Time
                    .Timer(nameof(PoisonDebuffPoisonShield))
                    .Each(value * 1000)
                    .Do(() => enemy.RemoveState(debuff))
                    .Auto();
            }

            return false;
        }

        /// <summary>
        /// так то бафы должны действовать и на аватар тоже
        /// </summary>
        private class PoisonDebuffPoisonShield : Applicable
        {
            private readonly long value;
            private readonly Avatar avatar;
            private readonly Ability ability;

            public PoisonDebuffPoisonShield(long value, Ability ability, Avatar avatar)
            {
                this.ability = ability;
                this.value = value;
                this.avatar = avatar;
            }

            public override string Image => "Rogue.Classes.Noone.Images.Abilities.Defstand.buf.png";

            private TimerTrigger timer;

            public void Apply(Mob enemy)
            {
                avatar.Character.Barrier += value;
                timer = Global.Time
                    .Timer(nameof(PoisonDebuffPoisonShield) + enemy.Uid)
                    .Each(1000)
                    .Do(()=> DOT(enemy))
                    .Repeat()
                    .Auto();
            }

            public void Discard(Mob enemy)
            {
                timer.StopDestroy();
            }

            private void DOT(Mob enemy)
            {
                if (!enemy.Enemy.Dead)
                {
                    enemy.Enemy.HitPoints -= value;

                    if (enemy.Enemy.HitPoints <= 0)
                    {
                        enemy.Die?.Invoke();

                        var expr = RandomRogue.Next(4, 16);
                        avatar.Character.EXP += expr;

                        ability.UseEffects?.Invoke(new PopupString($"Вы получаете {expr} опыта!", ConsoleColor.DarkMagenta, avatar.Location, 25, 19, 0.06).InList<ISceneObject>());
                    }

                    ability.UseEffects?.Invoke(new PopupString(value.ToString(), ConsoleColor.Green, enemy.Location, 25, 19, 0.06).InList<ISceneObject>());
                }
            }

            protected override void CallApply(dynamic obj)
            {
                this.Apply(obj);
            }

            protected override void CallDiscard(dynamic obj)
            {
                this.Discard(obj);
            }
        }
    }
}

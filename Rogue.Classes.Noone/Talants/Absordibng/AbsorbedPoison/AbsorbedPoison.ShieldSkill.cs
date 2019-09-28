using Rogue.Abilities;
using Rogue.Abilities.Talants;
using Rogue.Classes.Noone.Abilities;
using Rogue.Drawing.GUI;
using Rogue.Entites.Enemy;
using Rogue.Map;
using Rogue.Map.Objects;
using Rogue.Transactions;
using Rogue.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Classes.Noone.Talants.Absordibng
{
    public partial class AbsorbedPoison : Talant<Noone>
    {
        public void Discard(ShieldSkill elementalShield) { }

        public bool CanUse(ShieldSkill elementalShield) => Class.Actions >= 4;
        
        public void Apply(ShieldSkill elementalShield)
        {
            Class.Actions -= 1;

            //возможно нужны сборные аппликативные эффекты

            var rangeObject = new MapObject
            {
                Position = new Physics.PhysicalPosition
                {
                    X = Avatar.Position.X - ((Avatar.Size.Width * 2.5) / 2),
                    Y = Avatar.Position.Y - ((Avatar.Size.Height * 2.5) / 2)
                },
                Size = Avatar.Size
            };

            rangeObject.Size.Height *= 2.5;
            rangeObject.Size.Width *= 2.5;

            var enemies = GameMap.Enemies(rangeObject);

            foreach (var enemy in enemies)
            {
                var value = (long)elementalShield.Value;

                var debuff = new PoisonDebuffPoisonShield(value * 2, elementalShield, Avatar);
                enemy.AddState(debuff);

                Global.Time
                    .Timer(nameof(PoisonDebuffPoisonShield))
                    .Each(value * 1000)
                    .Do(() => enemy.RemoveState(debuff))
                    .Auto();
            }
        }

        public TalantInfo TalantInfo(ShieldSkill elementalShield)
        {
            return new TalantInfo()
            {
                Description = ""
            };
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
                    .Do(() => DOT(enemy))
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

                        ability.UseEffects?.Invoke(new PopupString($"Вы получаете {expr} опыта!", ConsoleColor.DarkMagenta, avatar.Location, 25, 12, 0.06).InList<ISceneObject>());
                    }

                    ability.UseEffects?.Invoke(new PopupString(value.ToString(), ConsoleColor.Green, enemy.Location, 25, 12, 0.06).InList<ISceneObject>());
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

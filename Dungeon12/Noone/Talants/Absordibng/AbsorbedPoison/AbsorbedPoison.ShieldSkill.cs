using Dungeon;
using Dungeon.Abilities;
using Dungeon.Abilities.Talants;
using Dungeon.Map;
using Dungeon.Map.Objects;
using Dungeon.SceneObjects;
using Dungeon.Transactions;
using Dungeon.View.Interfaces;
using Dungeon12.Map.Objects;
using Dungeon12.Noone.Abilities;
using System;

namespace Dungeon12.Noone.Talants.Absordibng
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
                Position = new Dungeon.Physics.PhysicalPosition
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

                Dungeon.Global.Time
                    .Timer(nameof(PoisonDebuffPoisonShield))
                    .After(value * 1000)
                    .Do(() => enemy.RemoveState(debuff))
                    .Auto();
            }
        }

        public TalantInfo TalantInfo(ShieldSkill elementalShield)
        {
            return new TalantInfo()
            {
                Name = "Облако яда",
                Description = $"При активации заражает всех врагов{Environment.NewLine} по близости, нанося Ур*2 ед. урона{Environment.NewLine} на Ур*1 секунд"
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

            public override string Image => "Dungeon12.Noone.Images.Abilities.Defstand.buf.png";

            private TimerTrigger timer;

            public void Apply(NPCMap enemy)
            {
                avatar.Character.Barrier += value;
                timer = Dungeon.Global.Time
                    .Timer(nameof(PoisonDebuffPoisonShield) + enemy.Uid)
                    .After(1000)
                    .Do(() => DOT(enemy))
                    .Repeat()
                    .Auto();
            }

            public void Discard(NPCMap enemy)
            {
                timer.StopDestroy();
            }

            private void DOT(NPCMap enemy)
            {
                if (!enemy.Entity.Dead)
                {
                    enemy.Entity.HitPoints -= value;

                    if (enemy.Entity.HitPoints <= 0)
                    {
                        enemy.Die?.Invoke();

                        var expr = RandomDungeon.Next(4, 16);
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

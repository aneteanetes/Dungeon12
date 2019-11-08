using Dungeon;
using Dungeon.Abilities.Talants;
using Dungeon.SceneObjects;
using Dungeon.Types;
using Dungeon.View.Interfaces;
using Dungeon12.Noone.Abilities;
using System;
using System.Collections.Generic;

namespace Dungeon12.Noone.Talants.Absordibng
{
    public partial class AbsorbedPoison : Talant<Noone>
    {
        public void Apply(Attack attack)
        {
            var enemy = attack.AttackedEnemy;

            if (enemy != default)
            {
                if (enemy.Entity.HitPoints <= 0)
                {
                    enemy.Die?.Invoke();

                    var expr = RandomDungeon.Next(4, 16);
                    Avatar.Character.EXP += expr;
                    attack.UseEffects(new List<ISceneObject>()
                    {
                        new PopupString($"Вы получаете {expr} опыта!", ConsoleColor.DarkMagenta,Avatar.Location,25, 14,0.06)
                    });
                }

                var dmg = this.Level;

                var pos = new Point(enemy.Location.X, enemy.Location.Y - 0.5);

                attack.UseEffects(new List<ISceneObject>()
                {
                    new PopupString(dmg.ToString(), ConsoleColor.Green,pos,25,12,0.06)
                });
            }

            attack.AttackedEnemy = default;
        }

        public TalantInfo TalantInfo(Attack attack)
        {
            return new TalantInfo()
            {
                Name="Ядовитая атака",
                Description = "Добавляет к атаке Ур*1 ед. урона ядом"
            };
        }

        public bool CanUse(Attack attack) => true;

        public void Discard(Attack attack) { }
    }
}

using Rogue.Abilities.Talants;
using Rogue.Classes.Noone.Abilities;
using Rogue.Drawing.GUI;
using Rogue.Types;
using Rogue.View.Interfaces;
using System;
using System.Collections.Generic;

namespace Rogue.Classes.Noone.Talants.Absordibng
{
    public partial class AbsorbedPoison : Talant<Noone>
    {
        public void Apply(Attack attack)
        {
            var enemy = attack.AttackedEnemy;

            if (enemy != default)
            {
                if (enemy.Enemy.HitPoints <= 0)
                {
                    enemy.Die?.Invoke();

                    var expr = RandomRogue.Next(4, 16);
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
                Description = "Добавляет яд к урону"
            };
        }

        public bool CanUse(Attack attack) => true;

        public void Discard(Attack attack) { }
    }
}

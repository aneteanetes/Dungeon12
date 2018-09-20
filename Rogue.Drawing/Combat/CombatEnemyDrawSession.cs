namespace Rogue.Drawing.Combat
{
    using System;
    using Rogue.Drawing.GUIInfo;
    using Rogue.Drawing.Impl;
    using Rogue.Entites.Enemy;

    public class CombatEnemyDrawSession : RightInfoDrawSession
    {
        public Enemy Enemy { get; set; }

        protected override void Draw()
        {
            string stringBuffer = string.Empty;

            //name
            var color = Enemy.ForegroundColor;
            if (Enemy.Name == "Валоран")
            {
                color = new DrawColor(ConsoleColor.Magenta);
            }
            this.WriteStatFull(Enemy.Name, 1, color);


            if (Enemy.Name != "Валоран")
            {
                this.WriteStatFull(" " + Enemy.ToString(), 3, ConsoleColor.DarkCyan);
            }
            else
            {
                this.WriteStatFull("  Хранитель мира  ", 2, ConsoleColor.Black, ConsoleColor.DarkCyan);
                this.WriteStatFull("  [Предатель]  ", 3, ConsoleColor.Black, ConsoleColor.Cyan);
            }

            this.WriteStatFull(" Уровень: " + Enemy.Level.ToString(), 3, ConsoleColor.DarkGray);
            this.WriteStatFull(" Жизнь: " + Enemy.HitPoints.ToString(), 7, ConsoleColor.Red);
            this.WriteStatFull("Урон: " + Enemy.MinDMG.ToString() + "-" + Enemy.MaxDMG.ToString(), 9, ConsoleColor.DarkYellow);
            this.WriteStatFull("Сила атаки: " + Enemy.AttackPower.ToString(), 11, ConsoleColor.DarkRed);
            this.WriteStatFull("Сила магии: " + Enemy.AbilityPower.ToString(), 12, ConsoleColor.DarkCyan);
            this.WriteStatFull("Защита Ф : " + Enemy.Defence.ToString(), 14, ConsoleColor.DarkGreen);
            this.WriteStatFull("Защита М : " + Enemy.Barrier.ToString(), 15, ConsoleColor.DarkMagenta);
            this.WriteStatFull("Способности:", 17, ConsoleColor.Yellow);
            
            //int i = 19;
            //foreach (MonsterAbility a in Enemy.Cast)
            //{
            //    if (a != null)
            //    {
            //        this.DrawStat(a.Name, i, ConsoleColor.Yellow);
            //        i++;
            //    }
            //}

            //if (i == 19)
            //{
            //    this.WriteStatFull(" Нет", i, ConsoleColor.Yellow);
            //}

            string clear = DrawHelp.FullLine(75, " ", 2);
            this.Write(26, 2, clear, ConsoleColor.DarkCyan);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Rogue.Drawing.Impl;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.GUIInfo
{
    public class EnemyInfoDrawSession : RightInfoDrawSession
    {
        public IDrawable Enemy { get; set; }

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
                this.DrawStat(" " + Enemy.GetRace(), 3, ConsoleColor.DarkCyan);
            }
            else
            {
                this.WriteStatFull("  Хранитель мира  ", 2, ConsoleColor.Black, ConsoleColor.DarkCyan);
                this.WriteStatFull("  [Предатель]  ", 3, ConsoleColor.Black, ConsoleColor.Cyan);
            }

            this.DrawStat(" Уровень: " + Enemy.LVL.ToString(), 3, ConsoleColor.DarkGray);
            this.DrawStat(" Жизнь: " + Enemy.CHP.ToString(), 7, ConsoleColor.Red);
            this.DrawStat("Урон: " + Enemy.MIDMG.ToString() + "-" + Enemy.MADMG.ToString(), 9, ConsoleColor.DarkYellow);
            this.DrawStat("Сила атаки: " + Enemy.AD.ToString(), 11, ConsoleColor.DarkRed);
            this.DrawStat("Сила магии: " + Enemy.AP.ToString(), 12, ConsoleColor.DarkCyan);
            this.DrawStat("Защита Ф : " + Enemy.ARM.ToString(), 14, ConsoleColor.DarkGreen);
            this.DrawStat("Защита М : " + Enemy.MRS.ToString(), 15, ConsoleColor.DarkMagenta);
            this.WriteStatFull("Способности:", 17, ConsoleColor.Yellow);
            
            int i = 19;
            foreach (MonsterAbility a in Enemy.Cast)
            {
                if (a != null)
                {
                    this.DrawStat(a.Name, i, ConsoleColor.Yellow);
                    i++;
                }
            }

            if (i == 19)
            {
                this.WriteStatFull(" Нет", i, ConsoleColor.Yellow);
            }

            string clear = DrawHelp.FullLine(75, " ", 2);
            this.Write(26, 2, clear, ConsoleColor.DarkCyan);
        }
    }
}

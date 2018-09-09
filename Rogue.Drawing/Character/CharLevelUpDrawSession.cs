using System;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Character
{
    public class CharLevelUpDrawSession : CharPerksInfoDrawSession
    {
        public int Gold { get; set; }

        public int Hp { get; set; }

        public int Mp { get; set; }

        public int Ldmg { get; set; }

        public int Mdmg { get; set; }

        public override IDrawSession Run()
        {
            //DrawMainInfoCharWindow();

            //Draw.Session<CharInfoItemsDrawSession>()
            //    .Publish();

            this.WriteHeader("Повышение уровня:");
            this.DrawLevelUp();

            return base.Run();
        }

        public void DrawLevelUp()
        {
            DrawStat(Rogue.RAM.Player.GetClassRace(2), 4, ConsoleColor.Cyan);
            DrawStat("Уровень: +1", 6, ConsoleColor.DarkCyan);
            DrawStat("Золото: +" + Gold, 6, ConsoleColor.Yellow);
            DrawStat("Жизнь: +" + Hp.ToString(), 10, ConsoleColor.Red);
            DrawStat(Rogue.RAM.Player.ManaName + ": +" + Mp.ToString(), 11, ConsoleColor.Blue);
            DrawStat("DMG↓: +" + Ldmg.ToString(), 13, ConsoleColor.DarkYellow);
            DrawStat("DMG↑: +" + Mdmg.ToString(), 13, ConsoleColor.DarkYellow);
            DrawStat("Очки навыков: +1", 16, ConsoleColor.Magenta);
            DrawStat("Очки крафта: +1 ", 17, ConsoleColor.DarkMagenta);
        }

        private void DrawStat(string statName, int height, ConsoleColor color)
        {
            int offset = 50 - (statName.Length / 2);
            this.Write(height, offset + 1, statName, color);
        }
    }
}

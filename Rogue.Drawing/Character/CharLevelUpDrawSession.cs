using System;
using Rogue.Entites.Alive.Character;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Character
{
    public class CharLevelUpDrawSession : CharPerksInfoDrawSession
    {
        public Player Player { get; set; }

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
            DrawStat(Player.Race.ToDisplay(), 4, ConsoleColor.Cyan);
            DrawStat("Уровень: +1", 6, ConsoleColor.DarkCyan);
            DrawStat("Золото: +" + Player.Gold, 6, ConsoleColor.Yellow);
            DrawStat("Жизнь: +" + Player.MaxHitPoints.ToString(), 10, ConsoleColor.Red);
            DrawStat(Player.Resource() + ": +" + Player.Resource().ToString(), 11, ConsoleColor.Blue);
            DrawStat("DMG↓: +" + Player.MinDMG.ToString(), 13, ConsoleColor.DarkYellow);
            DrawStat("DMG↑: +" + Player.MaxDMG.ToString(), 13, ConsoleColor.DarkYellow);
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

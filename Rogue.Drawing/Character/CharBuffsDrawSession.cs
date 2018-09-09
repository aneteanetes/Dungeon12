using System;
using System.Diagnostics;
using Rogue.Drawing.Impl;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Character
{
    public class CharBuffsDrawSession : CharPerksInfoDrawSession
    {
        public override IDrawSession Run()
        {
            this.WriteHeader("Эффекты персонажа");
            this.WriteTab();

            int ctab = (Tab * 5) - 5;
            int i = 0;
            for (int q = ctab; q < Tab * 5; q++)
            {
                try
                {
                    MechEngine.Ability p = Rogue.RAM.Player.Buffs[q];

                    this.Write(3 + i, 28, new DrawText("┌───┐ " + p.Name, ConsoleColor.Gray));
                    this.Write(4 + i, 28, new DrawText("│   │ Уровень навыка:" + p.Level.ToString(), ConsoleColor.Gray));
                    this.Write(4 + i, 30, new DrawText(p.Icon, p.Color));
                    this.Write(5 + i, 28, new DrawText("└───┘ " + "Время действия: " + p.Duration.ToString(), ConsoleColor.Gray));

                    //
                    string stringBuffer = string.Empty;
                    stringBuffer = DrawHelp.FullLine(74, DrawHelp.Border(true, 4), 27);
                    stringBuffer = " " + DrawHelp.Border(true, 8) + stringBuffer.Remove(stringBuffer.Length - 1) + DrawHelp.Border(true, 7);

                    this.Write(6 + i, 25, new DrawText(stringBuffer, ConsoleColor.DarkGreen));

                    i = i + 4;
                }
                catch (ArgumentOutOfRangeException)
                {
                    //ниуя не пофигу
                    Debugger.Break();
                    //пофигу
                }
            }

            return base.Run();
        }
    }
}

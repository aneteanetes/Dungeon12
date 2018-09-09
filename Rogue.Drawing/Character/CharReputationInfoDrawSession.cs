using System;
using System.Diagnostics;
using Rogue.Drawing.Impl;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Character
{
    public class CharReputationInfoDrawSession :CharPerksInfoDrawSession
    {
        public override IDrawSession Run()
        {
            this.WriteHeader("Дипломатическая репутация");
            this.WriteTab();


            int ctab = (Tab * 5) - 5;
            int i = 0;

            for (int q = ctab; q < Tab * 5; q++)
            {
                try
                {
                    MechEngine.Reputation p = Rogue.RAM.Player.Repute[q];
                    this.Write(3 + i, 28, new DrawText("Фракция: " + p.name, ConsoleColor.Gray));
                    this.Write(4 + i, 28, new DrawText("Прогресс: " + p.min.ToString() + "/" + p.max.ToString(), ConsoleColor.Gray));
                    this.Write(5 + i, 28, new DrawText("Цель: " + SystemEngine.Helper.String.ToString(p.race)));

                    //
                    string stringBuffer = string.Empty;
                    stringBuffer = DrawHelp.FullLine(74, DrawHelp.Border(true, 4), 27);
                    stringBuffer = " " + DrawHelp.Border(true, 8) + stringBuffer.Remove(stringBuffer.Length - 1) + DrawHelp.Border(true, 7);

                    this.Write(6 + i, 25, new DrawText(stringBuffer, ConsoleColor.Gray));

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

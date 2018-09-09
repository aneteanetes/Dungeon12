using System;
using Rogue.Drawing.Impl;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Character
{
    public class CharQuestInfoDrawSession : CharPerksInfoDrawSession
    {
        public override IDrawSession Run()
        {
            this.WriteHeader("Задания персонажа");
            this.WriteTab();

            int ctab = (Tab * 5) - 5;
            int i = 0;

            this.WriteTab();

            for (int q = ctab; q < Tab * 5; q++)
            {
                try
                {
                    MechEngine.Quest Q = Rogue.RAM.Player.QuestBook[q];

                    this.Write(3 + i, 28, new DrawText("┌───┐", ConsoleColor.Gray));

                    this.Write(3 + i, 28+6, new DrawText(Q.Name, Q.Difficult));
                    
                    this.Write(4 + i, 28, new DrawText("│   │", ConsoleColor.Gray));

                    ConsoleColor color = 0;

                    if (Q.Progress >= Math.Floor(Q.TargetCount * 0.2) || Q.Progress == 0) { color = ConsoleColor.Red; }
                    if (Q.Progress >= Math.Floor(Q.TargetCount * 0.4)) { color = ConsoleColor.Yellow; }
                    if (Q.Progress >= Math.Floor(Q.TargetCount * 0.8)) { color = ConsoleColor.Green; }

                    this.Write(4 + i, 28 + 6, new DrawText("Прогресс: <<" + Q.Progress + "/" + Q.TargetCount + ">>", color));

                    this.Write(4 + i, 30, new DrawText(Q.Icon, Q.Color));

                    this.Write(5 + i, 28, new DrawText("└───┘ " + Q.Target, ConsoleColor.Gray));

                    //
                    string stringBuffer = string.Empty;
                    stringBuffer = DrawHelp.FullLine(74, DrawHelp.Border(true, 4), 27);
                    stringBuffer = " " + DrawHelp.Border(true, 8) + stringBuffer.Remove(stringBuffer.Length - 1) + DrawHelp.Border(true, 7);

                    this.Write(6 + i, 25, new DrawText(stringBuffer, ConsoleColor.DarkGreen));

                    i = i + 4;
                }
                catch (ArgumentOutOfRangeException)
                {
                    //пофигу
                }
            }

            return base.Run();
        }
    }
}

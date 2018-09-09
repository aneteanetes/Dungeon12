using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Drawing
{
    public static class QuestgiverDraw
    {
        /// <summary>
        /// Draw quest giver window
        /// </summary>
        public static void DrawGiverWindow()
        {
            FightDraw.DrawEnemyGUI();

            MechEngine.Questgiver e = Rogue.RAM.qGiver;

            Console.ForegroundColor = e.SpeachColor;
            int Count = (23 / 2) - (e.Name.Length / 2);
            Console.SetCursorPosition(Count + 1, 2);
            Console.WriteLine(DrawHelp.FullLine(" ".Length + e.Name.Length, " " + e.Name, " ".Length + e.Name.Length - 1));

            DrawEngine.DrawHelp.DrawAvatar = new ColorChar() { Char = e.SpeachIcon, Color = e.SpeachColor };


            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Count = (23 / 2) - (" Задание:".Length / 2);
            Console.SetCursorPosition(Count + 1, 12);
            Console.WriteLine(" Задание:");
            //text of quest
            int forCount = 14;
            foreach (string s in DrawEngine.DrawHelp.TextBlock(e.Quest.Speach, 21))
            {
                Console.SetCursorPosition(3, forCount++);
                Console.WriteLine(s);
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Count = (23 / 2) - (" Награда:".Length / 2);
            Console.SetCursorPosition(Count + 1, forCount++);
            Console.WriteLine(" Награда:");

            int exemplar = 0;
            foreach (MechEngine.Item itm in e.Quest.Rewards.Items)
            {
                foreach (MechEngine.Item search in e.Quest.Rewards.Items)
                { if (itm.Name == search.Name) { exemplar++; } }
                Console.ForegroundColor = itm.Color;
                string nameofreward = string.Empty;
                if (itm.Name.Length > 14) { nameofreward = itm.Name.Substring(0, 14); } else { nameofreward = itm.Name; };
                Count = (23 / 2) - ((nameofreward + " (" + exemplar.ToString() + ")").Length / 2);
                Console.SetCursorPosition(Count + 1, forCount++);
                Console.WriteLine(nameofreward + " (" + exemplar + ")");
            }

            foreach (MechEngine.Perk prk in e.Quest.Rewards.Perks)
            {
                Console.ForegroundColor = prk.Color;
                string nameofreward = string.Empty;
                if (prk.Name.Length > 14) { nameofreward = prk.Name.Substring(0, 14); } else { nameofreward = prk.Name; };
                Count = (23 / 2) - (("Перк: " + nameofreward).Length / 2);
                Console.SetCursorPosition(Count + 1, forCount++);
                Console.WriteLine("Перк: " + nameofreward);
            }

            foreach (MechEngine.Ability abl in e.Quest.Rewards.Abilityes)
            {
                Console.ForegroundColor = abl.Color;
                string nameofreward = string.Empty;
                if (abl.Name.Length > 14) { nameofreward = abl.Name.Substring(0, 14); } else { nameofreward = abl.Name; };
                Count = (23 / 2) - (("Нав: <" + nameofreward + ">").Length / 2);
                Console.SetCursorPosition(Count + 1, forCount++);
                Console.WriteLine("Нав <" + nameofreward + ">");
            }

            foreach (int epx in e.Quest.Rewards.Exp)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                string nameofreward = string.Empty;
                if (epx.ToString().Length > 14) { nameofreward = epx.ToString().Substring(0, 12) + "~"; } else { nameofreward = epx.ToString(); };
                Count = (23 / 2) - (("EXP: " + nameofreward).Length / 2);
                Console.SetCursorPosition(Count + 1, forCount++);
                Console.WriteLine("EXP: " + nameofreward);
            }

            foreach (int epx in e.Quest.Rewards.Gold)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                string nameofreward = string.Empty;
                if (epx.ToString().Length > 14) { nameofreward = epx.ToString().Substring(0, 12) + "~"; } else { nameofreward = epx.ToString(); };
                Count = (23 / 2) - (("$: " + nameofreward).Length / 2);
                Console.SetCursorPosition(Count + 1, forCount++);
                Console.WriteLine("$: " + nameofreward);
            }
        }
    }
}

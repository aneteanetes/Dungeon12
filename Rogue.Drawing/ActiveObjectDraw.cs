using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Drawing
{


    public static class ActiveObjectDraw
    {
        public static void Draw(List<string> repl, MechEngine.CapitalDoor.GateKeeper obj)
        {
            FightDraw.DrawEnemyGUI();

            MechEngine.CapitalDoor.GateKeeper e = obj;

            Console.ForegroundColor = e.Color;
            int Count = (23 / 2) - (e.Name.Length / 2);
            Console.SetCursorPosition(Count + 1, 2);
            Console.WriteLine(DrawHelp.FullLine(" ".Length + e.Name.Length, " " + e.Name, " ".Length + e.Name.Length - 1));

            DrawEngine.DrawHelp.DrawAvatar = new ColorChar() { Char = e.Icon, Color = e.Color };

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Count = (23 / 2) - (" Куда переместиться:".Length / 2);
            Console.SetCursorPosition(Count + 1, 12);
            Console.WriteLine(" Куда переместиться:");

            int forCount = 14;

            foreach (string loc in repl)
            {
                Count = (23 / 2) - (loc.Length / 2);
                Console.SetCursorPosition(Count + 1, forCount++);
                Console.WriteLine(loc);
            }
        }

        public static void Draw(List<string> repl, MechEngine.ActiveObject obj, ConsoleColor TextColor)
        {
            FightDraw.DrawEnemyGUI();

            MechEngine.ActiveObject e = obj;

            Console.ForegroundColor = e.Color;
            int Count = (23 / 2) - (e.Name.Length / 2);
            Console.SetCursorPosition(Count + 1, 2);
            Console.WriteLine(DrawHelp.FullLine(" ".Length + e.Name.Length, " " + e.Name, " ".Length + e.Name.Length - 1));

            DrawEngine.DrawHelp.DrawAvatar = new ColorChar() { Char = e.Icon, Color = e.Color };

            Console.ForegroundColor = TextColor;
            Count = (23 / 2) - ("Действия:".Length / 2);
            Console.SetCursorPosition(Count + 1, 12);
            Console.WriteLine("Действия:");

            int forCount = 14;

            foreach (string loc in repl)
            {
                Count = (23 / 2) - (loc.Length / 2);
                Console.SetCursorPosition(Count + 1, forCount++);
                Console.WriteLine(loc);
            }
        }

        public static void Draw(Replica R, MechEngine.ActiveObject obj)
        {
            FightDraw.DrawEnemyGUI();

            MechEngine.ActiveObject e = obj;

            Console.ForegroundColor = e.Color;
            int Count = (23 / 2) - (e.Name.Length / 2);
            Console.SetCursorPosition(Count + 1, 2);
            Console.WriteLine(DrawHelp.FullLine(" ".Length + e.Name.Length, " " + e.Name, " ".Length + e.Name.Length - 1));

            DrawEngine.DrawHelp.DrawAvatar = new ColorChar() { Char = e.Icon, Color = e.Color };
            int forCount = 10;
            Console.ForegroundColor = R.TextColor;
            foreach (string s in DrawEngine.DrawHelp.TextBlock(R.Text, 21))
            {
                Count = (25 / 2) - (s.Replace('<', '\0').Replace('<', '\0').Length / 2);
                Count++;
                forCount++;
                if (s != "" && s != " ")
                {
                    foreach (char c in s)
                    {
                        if (c == '<') { Console.ForegroundColor = ConsoleColor.Green; continue; }
                        else if (c == '>') { Console.ForegroundColor = R.TextColor; continue; }

                        Console.SetCursorPosition(Count++, forCount);
                        Console.WriteLine(c);

                    }
                }
                // { Console.Write(s.Substring(1, s.Length - 1)); } else { Console.WriteLine(s); } }
            }
            forCount += 2;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Count = (23 / 2) - ("Действия:".Length / 2);
            Console.SetCursorPosition(Count + 1, forCount);
            Console.WriteLine("Действия:");
            forCount++;
            Console.ForegroundColor = R.OptionsColor;
            foreach (string repl in R.Options)
            {
                if (repl.Length > 20)
                {
                    Count = (23 / 2) - (repl.Substring(0, 20).Length / 2);
                    Console.SetCursorPosition(Count + 1, forCount++);
                    Console.WriteLine(repl.Substring(0, 20));
                }
                else
                {
                    Count = (23 / 2) - (repl.Length / 2);
                    Console.SetCursorPosition(Count + 1, forCount++);
                    Console.WriteLine(repl);
                }
            }
        }

        public static void DrawChest(MechEngine.Chest obj, int index)
        {
            //winAPIDraw.DrawRightWindow.Clear(); old method
            BatchDraw.DrawLeftWindow.Start = true;


            Console.ForegroundColor = obj.Color;
            int Count = (23 / 2) - (obj.Name.Length / 2);
            //Console.SetCursorPosition(Count + 1, 1);
            //Console.WriteLine(DrawHelp.FullLine(" ".Length + obj.Name.Length, " " + obj.Name, " ".Length + obj.Name.Length - 1));
            BatchDraw.DrawLeftWindow.AddLine(Count + 1, 1, obj.Name, obj.Color);

            int top = 2;
            int limit = "                  ".Length;

            for (int i = 0; i < 7; i++)
            {
                bool bold = false;
                ConsoleColor col = ConsoleColor.Gray;
                if (index == i) { bold = true; col = Rogue.RAM.CUIColor; }

                //Console.ForegroundColor = col;
                top++;
                //Console.SetCursorPosition(3, top);
                //Console.WriteLine(DrawHelp.Border(bold, 0, "TopCornLeft") + "──────────────────" + DrawHelp.Border(bold, 0, "TopCornRight"));
                BatchDraw.DrawLeftWindow.AddLine(1, top, DrawHelp.Border(bold, 0, "TopCornLeft") + "──────────────────" + DrawHelp.Border(bold, 0, "TopCornRight"), col);
                top++;
                //Console.SetCursorPosition(3, top);
                //Console.WriteLine("│                  │");
                BatchDraw.DrawLeftWindow.AddLine(1, top, "│                  │", col);

                try
                {
                    //itm
                    MechEngine.Item it = obj.Items[i];
                    string itm = string.Empty;
                    if (it.Name.Length > limit) { itm = it.Name.Substring(0, limit); } else { itm = it.Name; }
                    int count = (22 / 2) - (itm.Length / 2);
                    //Console.SetCursorPosition(count, top);
                    //Console.ForegroundColor = it.Color;
                    //Console.Write(itm);                
                    BatchDraw.DrawLeftWindow.AddLine(count, top, itm, it.Color);
                }
                catch (ArgumentOutOfRangeException) { }
                //Console.ForegroundColor = col;

                top++;
                //Console.SetCursorPosition(3, top);
                //Console.WriteLine(DrawHelp.Border(bold, 0, "BotCornLeft") + "──────────────────" + DrawHelp.Border(bold, 0, "BotCornRight"));
                BatchDraw.DrawLeftWindow.AddLine(1, top, DrawHelp.Border(bold, 0, "BotCornLeft") + "──────────────────" + DrawHelp.Border(bold, 0, "BotCornRight"), col);
                bold = false;
            }

            BatchDraw.DrawLeftWindow.Draw = true;
        }
    }
}

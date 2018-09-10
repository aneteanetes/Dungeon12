using System;
using Rogue.Drawing.Impl;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Abilities
{
    public class AbilityInfoDrawSession : DrawSession
    {
        public AbilityInfoDrawSession()
        {
            this.AutoClear = false;
        }

        public IDrawable Ability { get; set; }

        public IDrawable Player { get; set; }

        public override IDrawSession Run()
        {
            var color = ConsoleColor.Gray;

            string[] str = Ability.Description.Split('\n');
            
            int topPos = 0;
            int leftPos = 0;
            foreach (string s in str)
            {
                leftPos = (50 / 2) - (s.Length / 2);
                foreach (char ch in s.ToCharArray())
                {
                    if (ch == '&') { color = ConsoleColor.DarkCyan; continue; }
                    else if (ch == '^') { color = ConsoleColor.Green; continue; }
                    else if (ch == '#') { color = ConsoleColor.Yellow; continue; }
                    else if (ch == ';') { color = ConsoleColor.DarkGray; continue; }
                    else if (ch == '<') { color = ConsoleColor.DarkYellow; continue; }
                    else if (ch == '@') { color = ConsoleColor.DarkMagenta; continue; }
                    else if (ch == '?') { color = ConsoleColor.Cyan; continue; }
                    else if (ch == '$') { color = ConsoleColor.Cyan; continue; }
                    else if (ch == '№') { color = ConsoleColor.Cyan; continue; }
                    else if (ch == '~') { color = ConsoleColor.Magenta; continue; }
                    else if (ch == '†') { color = /*Player.ForegroundColor*/ ConsoleColor.Gray; continue; }

                    if (ch == ' ')
                        color = ConsoleColor.Gray;

                    this.Write(topPos + 4, leftPos + 25, ch.ToString(), color);
                }

                topPos++;
            }



            //Mode
            leftPos = (50 / 2) - (Ability.Mode.Length / 2);
            this.Write(15, leftPos + 25, Abilities.Mode, ConsoleColor.Red);

            //Cost
            leftPos = (50 / 2) - ((Player.Mana.Name.Length + Ability.Cost.Length + 1) / 2);
            this.Write(17, leftPos + 25, Rogue.RAM.Player.ManaName + Cost, SystemEngine.Helper.Information.ClassC);

            //Rate
            leftPos = (25 / 2) - ((Abilitiy.ApRate.Length) / 2);
            this.Write(19, leftPos + 25, Abilitiy.ApRate, ConsoleColor.DarkRed);
            
            //lvlRate
            leftPos = (75 / 2) - (("Уровень: " + Ability.LvlRate).Length / 2);
            this.Write(20, leftPos + 25, "Уровень: " + Abilitiy.LvlRate, ConsoleColor.DarkGray);

            //duration
            string duration = Ability.Duration;

            if (duration == "0")
            {
                duration = "Мгновенно";
            }

            if (duration == "999")
            {
                duration = "Постоянно";
            }

            duration = "Время действия: " + duration;

            leftPos = (50 / 2) - (duration.Length / 2);
            this.Write(20, leftPos + 25, duration, ConsoleColor.DarkYellow);

            //location
            string location = "Где используется: " + Ability.Location;
            leftPos = (50 / 2) - location.Length / 2;
            this.Write(22, leftPos + 25, location, ConsoleColor.DarkCyan);

            return base.Run();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Rogue.Drawing.Impl;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Combat
{
    public class CombatLogDrawSession : DrawSession
    {
        public CombatLogDrawSession()
        {
            this.DrawRegion = new Types.Rectangle
            {
                X = 24,
                Y = 4,
                Width = 47,
                Height = 22
            };
        }

        public IDrawable Enemy { get; set; }

        public IDrawable Player { get; set; }

        public List<IDrawable> Log { get; set; }

        public override IDrawSession Run()
        {
            var color = Enemy.ForegroundColor;
            if (Enemy.Name == "Валоран")
            {
                color = new DrawColor(ConsoleColor.Magenta);
            }

            int pos = (23 / 2) - (Enemy.Name.Length / 2);
            this.Write(1,pos+1, DrawHelp.FullLine(" ".Length + Enemy.Name.Length, " " + Enemy.Name, " ".Length + Enemy.Name.Length - 1),color);

            color = new DrawColor(ConsoleColor.DarkGray);
            pos = (100 / 2) - ((Enemy.Name.Length + " VS ".Length + Player.Name.Length) / 2);
            this.Write(1, pos + 1, DrawHelp.FullLine(Enemy.Name.Length + " VS ".Length + Player.Name.Length, Enemy.Name + " VS " + Player.Name, (Enemy.Name.Length + " VS ".Length + Player.Name.Length) - 1), color);
            
            color = new DrawColor(ConsoleColor.DarkCyan);

            int q = 3;
            if (Log.Count > 20)
            {
                for (int i = 0; i < Log.Count - 20; i++)
                {
                    Log.Remove(Log[i]);
                }
            }

            for (int i = 0; i < Log.Count; i++)
            {
                Console.SetCursorPosition(28, q);

                if (Log[i].Name.Length > 37)
                {
                    string first = Log[i].Name.Substring(37);
                    Log.Add(first);
                    Log[i] = Log[i].Name.Remove(37);
                    this.Run();
                }
                else
                {
                    this.Write(q, 28, DateTime.Now.ToShortTimeString() + " > " + Log[i], color);
                }
                q++;
            }

            return base.Run();
        }
    }
}

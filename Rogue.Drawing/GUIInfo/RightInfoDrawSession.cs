using System;
using System.Collections.Generic;
using System.Text;
using Rogue.Drawing.Impl;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.GUIInfo
{
    public abstract class RightInfoDrawSession : DrawSession
    {
        public RightInfoDrawSession()
        {
            this.DrawRegion = new Types.Rectangle
            {
                X = 1,
                Y = 1,
                Width = 24,
                Height = 23
            };
        }

        protected abstract void Draw();

        public override IDrawSession Run()
        {
            this.Draw();
            return base.Run();
        }

        protected void WriteAvatar(string icon, ConsoleColor color)
        {
            var GUIcolor = ConsoleColor.DarkGreen;

            var top = "╔═══════╗";
            int pos = (23 / 2) - (top.Length / 2);
            this.Write(4, pos + 1, top, GUIcolor);

            int count = 5;
            for (int i = 0; i < 5; i++)
            {
                this.Write(count++, pos + 1, "║       ║", GUIcolor);

                if (i == 2)
                {
                    this.Write(count-1, pos + 5, icon, color);
                }
            }

            this.Write(count, pos + 1, "╚═══════╝", GUIcolor);
        }
    }
}

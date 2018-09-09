using System;
using System.Collections.Generic;
using System.Text;
using Rogue.Drawing.GUIInfo;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Dialogue
{
    public class DialogueDrawSession : RightInfoDrawSession
    {
        public IDrawable Replier { get; set; }

        public List<string> Replics { get; set; }

        protected override void Draw()
        {
            this.WriteStatFull(Replier.Name, 2, Replier.ForegroundColor);
            this.WriteAvatar(Replier.Icon, ConsoleColor.Yellow);


            var stringBuffer = "Действия:";
            var pos = (23 / 2) - (stringBuffer.Length / 2);
            this.Write(12, pos + 1, stringBuffer, ConsoleColor.DarkGreen);

            int forCount = 14;

            foreach (string replic in Replics)
            {
                stringBuffer = replic;
                pos = (23 / 2) - (stringBuffer.Length / 2);
                this.Write(forCount++, pos + 1, stringBuffer, ConsoleColor.DarkGreen);
            }
        }
    }
}

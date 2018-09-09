using System;
using Rogue.Drawing.GUIInfo;
using Rogue.Drawing.Utils;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Dialogue
{
    public class ReplicaDrawSession : RightInfoDrawSession
    {
        public IDrawable Target { get; set; }

        public Replica Replica { get; set; }

        protected override void Draw()
        {
            this.WriteStatFull(Target.Name, 2, Target.ForegroundColor);
            this.WriteAvatar(Target.Icon, ConsoleColor.Yellow);

            int forCount = 10;

            var color = Replica.TextColor;
            foreach (string s in DrawHelp.TextBlock(Replica.Text, 21))
            {
                var pos1 = (25 / 2) - (s.Replace('<', '\0').Replace('<', '\0').Length / 2);
                pos1++;
                forCount++;
                if (s != "" && s != " ")
                {
                    foreach (char c in s)
                    {
                        if (c == '<') { color = ConsoleColor.Green; continue; }
                        else if (c == '>') { color = Replica.TextColor; continue; }

                        this.Write(forCount, pos1++, c.ToString(), color);

                    }
                }
            }

            forCount += 2;

            var stringBuffer = "Действия:";
            var pos = (23 / 2) - (stringBuffer.Length / 2);
            this.Write(forCount, pos + 1, stringBuffer, ConsoleColor.Yellow);

            forCount++;
            color = Replica.OptionsColor;
            foreach (string repl in Replica.Options)
            {
                if (repl.Length > 20)
                {
                    stringBuffer = repl.Substring(0, 20);
                    pos = (23 / 2) - (stringBuffer.Length / 2);
                    this.Write(forCount++, pos + 1, stringBuffer, color);
                }
                else
                {
                    pos = (23 / 2) - (repl.Length / 2);
                    this.Write(forCount++, pos + 1, repl, color);
                }
            }
        }
    }
}

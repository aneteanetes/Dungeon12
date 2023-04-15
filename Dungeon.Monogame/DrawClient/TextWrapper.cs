using Dungeon.View.Interfaces;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon.Monogame
{
    internal static class TextWrapper
    {
        internal static string WrapText(DynamicSpriteFont font, string text, double maxLineWidth, int counter = 0, string original = default, IDrawText dtext = default)
        {
            if (original == default)
            {
                original = text;
            }
            string[] words = text.Split(' ');
            StringBuilder sb = new StringBuilder();
            float lineWidth = 0f;
            float spaceWidth = font.MeasureString(" ").X;

            if (counter > 20)
                return text;

            if (maxLineWidth < spaceWidth)
                return text; //попытка избежать stackoverflowexception

            foreach (string wordinwords in words)
            {
                Wrap(wordinwords, sb, font, maxLineWidth, ref lineWidth, spaceWidth, ref counter, original);
            }

            return sb.ToString();
        }

        private static void Wrap(string wordinwords, StringBuilder sb, DynamicSpriteFont font, double maxLineWidth, ref float lineWidth, float spaceWidth, ref int counter, string original)
        {
            var word = wordinwords;

            if (wordinwords.Contains("\r\n"))
            {
                var rn = new List<string>();

                int idx = wordinwords.IndexOf("\r\n");
                while (idx != -1)
                {
                    var before = word.Substring(0, idx);
                    rn.Add(before);
                    rn.Add("\r\n");
                    word = word.Remove(0, before.Length);
                    word = word.Remove(0, 2);
                    idx = word.IndexOf("\r\n");
                }

                if (!string.IsNullOrWhiteSpace(word))
                    rn.Add(word);

                foreach (var p in rn)
                {
                    if (p == "\r\n")
                    {
                        sb.AppendLine();
                        lineWidth = 0;
                    }
                    else
                    {
                        Wrap(p, sb, font, maxLineWidth, ref lineWidth, spaceWidth, ref counter, original);
                    }
                }

                return;
            }

            Vector2 size = font.MeasureString(word);

            if (lineWidth + size.X < maxLineWidth)
            {
                sb.Append(word + " ");
                lineWidth += size.X + spaceWidth;
            }
            else
            {
                if (size.X > maxLineWidth)
                {
                    if (sb.ToString() == "")
                    {
                        sb.Append(WrapText(font, word.Insert(word.Length / 2, " ") + " ", maxLineWidth, ++counter, original));
                    }
                    else
                    {
                        sb.Append(Environment.NewLine + WrapText(font, word.Insert(word.Length / 2, " ") + " ", maxLineWidth, ++counter, original));
                    }
                }
                else
                {
                    sb.Append(Environment.NewLine + word + " ");
                    lineWidth = size.X + spaceWidth;
                }
            }
        }

    }
}

using Dungeon.Drawing;
using Dungeon.Drawing.Impl;
using Dungeon.View.Interfaces;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Dungeon
{
    public static class DrawTextExtensions
    {
        public static DrawText AsDrawText(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                value = ".";

            return new DrawText(value,fontAsm: Assembly.GetCallingAssembly().GetName().Name).DefaultFont();
        }

        /// <summary>
        /// 
        /// <para>
        /// [Кэшируемый]
        /// </para>
        /// </summary>
        public static DrawText Parse(this string valuestr)
        {
            if (!___ParsedTextCache.TryGetValue(valuestr, out var value))
            {
                value = ParseInternal(valuestr);
                value.OnDestroyGameComponent = () => ___ParsedTextCache.Remove(valuestr);
                ___ParsedTextCache.Add(valuestr, value);
            }

            return value;
        }
        private static readonly Dictionary<string, DrawText> ___ParsedTextCache = new Dictionary<string, DrawText>();

        private static DrawText ParseInternal(this string value)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(value);


            DrawText text = new DrawText("", fontAsm: Assembly.GetCallingAssembly().GetName().Name).DefaultFont();

            htmlDoc.DocumentNode.ChildNodes.ForEach(c =>
            {
                if (c.Name == "#text")
                {
                    text.Append(c.InnerText.AsDrawText());
                }
                else if (c.Name == "c")
                {
                    var color = DrawColor.GetByName(c.GetAttributeValue("value", "White").Capitalize());
                    text.Append(c.InnerText.AsDrawText().InColor(color));
                }
            });

            return text;
        }



        public static DrawText DefaultFont(this DrawText drawText)
        {
            drawText.FontName = DungeonGlobal.DefaultFontName;
            //drawText.FontAssembly = "Dungeon.Resources";
            //drawText.FontPath = "Dungeon.Resources.Fonts.Fledgling.ttf";

            return drawText;
        }

        public static DrawText InColor(this DrawText drawText, DrawColor drawColor)
        {
            drawText.ForegroundColor = drawColor;
            return drawText;
        }

        public static DrawText InColor(this DrawText drawText, IDrawColor drawColor)
        {
            drawText.ForegroundColor = drawColor;
            return drawText;
        }

        public static DrawText InColor(this DrawText drawText, ConsoleColor consoleColor)
        {
            drawText.ForegroundColor = new DrawColor(consoleColor);
            return drawText;
        }

        public static DrawText InSize(this DrawText drawText, long size=12)
        {
            drawText.Size = size;
            return drawText;
        }

        public static DrawText WithWordWrap(this DrawText drawText)
        {
            drawText.WordWrap = true;
            return drawText;
        }
    }
}

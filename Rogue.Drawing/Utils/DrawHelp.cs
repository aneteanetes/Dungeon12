using System;
using System.Collections.Generic;
using System.Text;
using Rogue.Drawing.Impl;
using Rogue.View.Interfaces;

namespace Rogue.Drawing
{

    public static class DrawHelp
    {
        /// <summary>
        /// Something about full line writing
        /// </summary>
        /// <param name="Width">i think result line width</param>
        /// <param name="Line">text</param>
        /// <param name="Spase">int for minus full line width</param>
        /// <returns></returns>
        public static string FullLine(int Width, string Line, int Spase = 1)
        {
            string r = string.Empty;
            for (int i = 0; i < Width - Spase; i++)
            {
                r += Line;
            }
            return r;
        }
        /// <summary>
        /// Get line
        /// </summary>
        /// <param name="Width">Line width</param>
        /// <param name="Char">Char for line</param>
        /// <returns>string.length==width and string.indexOf(Char)==width</returns>
        public static string GetLine(int Width, char Char)
        {
            string s = "";
            for (int i = 0; i < Width; i++) { s += Char; }
            return s;
        }
        /// <summary>
        /// return one char in string for console borders
        /// </summary>
        /// <param name="Bold">Bold = true</param>
        /// <param name="Wall">Int position of wall or=></param>
        /// <param name="NameOfWall">=> or string name of wall like 'TopConLeft', it's mean Upper Corner Left</param>
        /// <returns></returns>
        public static string Border(bool Bold, int Wall, string NameOfWall = "")
        {
            char r = new char();

            if (NameOfWall == "")
            {
                if (Bold == true)
                {
                    switch (Wall)
                    {
                        case 1: { r = '╔'; break; }
                        case 2: { r = '╗'; break; }
                        case 3: { r = '║'; break; }
                        case 4: { r = '═'; break; }
                        case 5: { r = '╚'; break; }
                        case 6: { r = '╝'; break; }
                        case 7: { r = '╣'; break; }
                        case 8: { r = '╠'; break; }
                        case 9: { r = '╩'; break; }
                        case 10: { r = '╦'; break; }
                        case 11: { r = '╬'; break; }
                    }
                }
                else
                {
                    switch (Wall)
                    {
                        case 1: { r = '┌'; break; }
                        case 2: { r = '┐'; break; }
                        case 3: { r = '│'; break; }
                        case 4: { r = '─'; break; }
                        case 5: { r = '└'; break; }
                        case 6: { r = '┘'; break; }
                        case 7: { r = '├'; break; }
                        case 8: { r = '┤'; break; }
                        case 9: { r = '┴'; break; }
                        case 10: { r = '┬'; break; }
                        case 11: { r = '┼'; break; }
                    }
                }
            }
            else
            {
                if (Bold == true)
                {
                    switch (NameOfWall)
                    {
                        case "TopCornLeft": { r = '╔'; break; }
                        case "TopCornRight": { r = '╗'; break; }
                        case "WallVert": { r = '║'; break; }
                        case "WallHor": { r = '═'; break; }
                        case "BotCornLeft": { r = '╚'; break; }
                        case "BotCornRight": { r = '╝'; break; }
                        case "ThreeLeft": { r = '╣'; break; }
                        case "ThreeRight": { r = '╠'; break; }
                        case "ThreeTop": { r = '╩'; break; }
                        case "ThreeBot": { r = '╦'; break; }
                        case "Cross": { r = '╬'; break; }
                    }
                }
                else
                {
                    switch (NameOfWall)
                    {
                        case "TopCornLeft": { r = '┌'; break; }
                        case "TopCornRight": { r = '┐'; break; }
                        case "WallVert": { r = '│'; break; }
                        case "WallHor": { r = '─'; break; }
                        case "BotCornLeft": { r = '└'; break; }
                        case "BotCornRight": { r = '┘'; break; }
                        case "ThreeLeft": { r = '┤'; break; }
                        case "ThreeRight": { r = '├'; break; }
                        case "ThreeTop": { r = '┴'; break; }
                        case "ThreeBot": { r = '┬'; break; }
                        case "Cross": { r = '┼'; break; }
                    }
                }
            }
            return r.ToString();
        }
        /// <summary>
        /// Return text block with lines whitch equal RowLimit
        /// </summary>
        /// <param name="Text">Text</param>
        /// <param name="RowLimit">Limit</param>
        /// <returns></returns>
        public static List<string> TextBlock(string Text, int RowLimit)
        {
            List<string> tb = new List<string>();
            int lines = Convert.ToInt32(Text.Length / RowLimit) + 1;
            for (int i = 0; i < lines; i++)
            {
                try
                { tb.Add(Text.Substring(i * RowLimit, RowLimit)); }
                catch (ArgumentOutOfRangeException) { tb.Add(Text.Substring(i * RowLimit)); }
            }
            return tb;
        }

        /// <summary>
        /// Return int value with sign ( - or + )
        /// </summary>
        /// <param name="Number">int value</param>
        /// <returns></returns>
        public static string Sign(int Number)
        {
            if (Number >= 0) { return "+" + Number.ToString(); }
            else { return Number.ToString(); }
        }
        /// <summary>
        /// Return double value with sign ( - or + )
        /// </summary>
        /// <param name="Number">double value</param>
        /// <returns></returns>
        public static string Sign(double Number)
        {
            if (Number >= 0) { return "+" + Number.ToString(); }
            else { return Number.ToString(); }
        }
    }
}

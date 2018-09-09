using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Drawing
{
    public class ColorChar
    { public char Char; public ConsoleColor Color; }
    public class ColoredWord
    { public string Word; public ConsoleColor Color; }
    public class Replica
    { public String Text; public List<String> Options = new List<string>(); public ConsoleColor TextColor; public ConsoleColor OptionsColor; }
    public class StringMenu
    {
        public ColoredWord Logo;
        public ColoredWord Additional;
        public ColoredWord Addon;
        public List<String> Options;
        public ConsoleColor OptionsColor;
        public ConsoleColor OptionsColorBackground;
        public ConsoleColor OptionsColorSelected;
        public ConsoleColor OptionsColorSelectedBackground;
        public Int32 Index;
    }

    /// <summary>
    /// Just more cose then winAPI char
    /// </summary>
    public class ColouredChar
    {
        public short Color;
        public short BackColor;
        public char Char;
    }
}

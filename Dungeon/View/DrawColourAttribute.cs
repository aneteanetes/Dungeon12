using Dungeon.Drawing;
using System;

namespace Dungeon
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class DrawColourAttribute : Attribute
    {
        public DrawColor Colour { get; set; }

        public DrawColourAttribute(byte r, byte g, byte b, byte a=255)
        {
            Colour=new DrawColor(r, g, b, a);
        }
    }
}
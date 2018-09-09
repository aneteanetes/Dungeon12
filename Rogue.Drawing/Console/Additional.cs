using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Drawing.Console
{
    /// <summary>
    /// Some helpful objects
    /// </summary>
    public static class Additional
    {
        public static Border BoldBorder
        {
            get
            {
                Border b = new Border();
                b.HorizontalLine = '═';
                b.LowerLeftCorner = '╚';
                b.LowerRightCorner = '╝';
                b.PerpendicularLeftward = '╣';
                b.PerpendicularRightward = '╠';
                b.UpperLeftCorner = '╔';
                b.UpperRightCorner = '╗';
                b.VerticalLine = '║';
                return b;
            }
        }
        public static Border LightBorder
        {
            get
            {
                Border b = new Border();
                b.HorizontalLine = '─';
                b.LowerLeftCorner = '└';
                b.LowerRightCorner = '┘';
                b.PerpendicularLeftward = '┤';
                b.PerpendicularRightward = '├';
                b.UpperLeftCorner = '┌';
                b.UpperRightCorner = '┐';
                b.VerticalLine = '│';
                return b;
            }
        }
        public static Animation StadartAnimation
        {
            get
            {
                return new Animation() { AnimationDirection = TextPosition.None, Frames = 1, Name = "Standart" };
            }
        }
        public static Animation HorizontalAnimation
        {
            get
            {
                return new Animation() { AnimationDirection = TextPosition.Right, Frames = 1, Name = "Horizontal" };
            }
        }
        public static Animation CenterAnimation
        {
            get
            {
                return new Animation() { AnimationDirection = TextPosition.Center, Frames = 1, Name = "Center" };
            }
        }
    }
}

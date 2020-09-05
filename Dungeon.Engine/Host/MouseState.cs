using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace Dungeon.Engine.Host
{
    internal class MouseState
    {
        public MouseState(double x, double y, int scrollWheel, MouseButtonState leftButton, MouseButtonState middleButton, MouseButtonState rightButton)
        {
            X = x;
            Y = y;
            LeftButton = leftButton;
            RightButton = rightButton;
            MiddleButton = middleButton;
            ScrollWheelValue = scrollWheel;
        }

        public double X { get; set; }

        public double Y { get; set; }

        //NOT REF
        public Point Position => new Point(X, Y);

        public MouseButtonState LeftButton { get; }
        //
        // Summary:
        //     Gets state of the middle mouse button.
        public MouseButtonState MiddleButton { get; }
        //
        // Summary:
        //     Gets state of the right mouse button.
        public MouseButtonState RightButton { get; }
        //
        // Summary:
        //     Returns cumulative scroll wheel value since the game start.
        public int ScrollWheelValue { get; }
        //
        // Summary:
        //     Returns the cumulative horizontal scroll wheel value since the game start
        public int HorizontalScrollWheelValue { get; }
    }
}

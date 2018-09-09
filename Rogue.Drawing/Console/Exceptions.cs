using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Drawing.Console
{
    public class WindowSizeException : Exception
    {
        public override string Message
        {
            get
            {
                return "One of size fields was zero or less!";
            }
        }
        public string Field
        {
            get
            {
                return "Width,Height,Top,Left";
            }
        }
    }

    public class WindowLowHeigthException : WindowSizeException
    {
        public override string Message
        {
            get
            {
                return "The window can not fit all the text!";
            }
        }
        public new string Field
        {
            get
            {
                return "Heigth";
            }
        }
    }

    public class WindowLowWidthException : WindowSizeException
    {
        public override string Message
        {
            get
            {
                return "The window width can not be less 2!";
            }
        }
        public new string Field
        {
            get
            {
                return "Width";
            }
        }
    }

    public class WindowTextWidthException : WindowSizeException
    {
        public override string Message
        {
            get
            {
                return "The window width can not be less text.lenght!";
            }
        }
        public new string Field
        {
            get
            {
                return "Width";
            }
        }
    }

    public class WindowHeaderWidthException : WindowSizeException
    {
        public override string Message
        {
            get
            {
                return "The window width can not be less header.lenght!";
            }
        }
        public new string Field
        {
            get
            {
                return "Width";
            }
        }
    }

    public class WindowOutOfDesplayException : WindowSizeException
    {
        public override string Message
        {
            get
            {
                return "The window size can not be more Console.BufferSize!";
            }
        }
        public new string Field
        {
            get
            {
                return "Width,Height,Top,Left";
            }
        }
    }

    public class WindowAnimationSizeException : WindowSizeException
    {
        public override string Message
        {
            get
            {
                return "Ony windows with equals Height and Width can use Center animation!";
            }
        }
        public new string Field
        {
            get
            {
                return "Window.Animation.AnimationDirection";
            }
        }
    }
}

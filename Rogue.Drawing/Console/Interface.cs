using System;
using System.Collections.Generic;
using System.Text;
using Rogue.Drawing.Impl;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Console
{
    /// <summary>
    /// Interface: Interface; Object;
    /// </summary>
    public class Interface : DrawSession
    {
        protected List<IDrawable> DrawableList { get; } = new List<IDrawable>();

        public override IEnumerable<IDrawable> Drawables { get => this.DrawableList; set { } }

        /// <summary>
        /// Event when control creating
        /// </summary>
        public Action OnCreate;

        /// <summary>
        /// Window which have this interface
        /// </summary>
        protected Window Window;

        /// <summary>
        /// Event when control on focus. this.OnFocus !=null ? this.OnFocus() : nothing;
        /// </summary>
        public Action OnFocus;

        /// <summary>
        /// Color of border / label when control active
        /// </summary>
        public DrawColor ActiveColor = ConsoleColor.Black;

        /// <summary>
        /// Color of border / label when control inactive
        /// </summary>
        public DrawColor InactiveColor = ConsoleColor.Black;

        /// <summary>
        /// Label, or default text
        /// </summary>
        public string Label;

        public virtual bool Activatable { get; } = false;

        public bool Active { get; set; }

        /// <summary>
        /// Result with params of string (abcdef):  (red)a,(red)b,(blue)c,(green)d,(green)e,Exception;
        /// </summary>
        /// <param name="Line">String line</param>
        /// <param name="ForeColors">List of colors, 0-default color: (Red,Blue,Green)</param>
        /// <param name="ForePositions">List of positions when need change color, 0-default color (2,3,5)</param>
        /// <returns></returns>
        /// 
        protected static List<ColouredChar> GetColouredLine(string Line, List<short> ForeColors, List<int> ForePositions, List<short> BackColors, List<int> BackPositions)
        {
            //return value
            List<ColouredChar> l = new List<ColouredChar>();

            //j - index of color list
            int j = 0;
            int jj = 0;
            int CurrentCharIndex = -1;
            short cl = ForeColors[0];
            short bcl = 0;
            try { bcl = BackColors[0]; }
            catch { }



            //foreach char in out string
            foreach (char c in Line.ToCharArray())
            {
                CurrentCharIndex++;
                //if need change color
                foreach (int i in ForePositions)
                {
                    if (CurrentCharIndex == i)
                    {
                        //j++ cuz j index of color
                        j++;
                        try { cl = ForeColors[j]; }
                        catch { }
                    }
                }
                //if need change background color
                foreach (int i in BackPositions)
                {
                    if (CurrentCharIndex == i)
                    {
                        //j++ cuz j index of color
                        jj++;
                        try { bcl = BackColors[jj]; }
                        catch { }
                    }
                }
                //add our color
                l.Add(new ColouredChar() { Color = cl, Char = c, BackColor = bcl });
            }

            //r
            return l;
        }

        /// <summary>
        /// Get line
        /// </summary>
        /// <param name="Width">Line width</param>
        /// <param name="Char">Char for line</param>
        /// <returns>string.length==width and string.indexOf(Char)==width</returns>
        /// 
        protected static string GetLine(int Width, char Char)
        {
            string s = "";
            for (int i = 0; i < Width; i++) { s += Char; }
            return s;
        }

        /// <summary>
        /// Return middle string
        /// </summary>
        /// <param name="String">Enter string</param>
        /// <returns></returns>
        protected string Middle(string String)
        {
            if (String.Length < this.Width - 2)
            {
                int side = ((this.Width - 2) - String.Length) / 2;
                for (int i = 0; i < side; i++)
                {
                    String = ' ' + String;
                    String += ' ';
                }
            }
            if (this.Width % 2 == 0 && String.Length % 2 != 0) { String = ' ' + String; }
            if (this.Width % 2 != 0 && String.Length % 2 == 0) { String += ' '; }
            return String;
        }
        
        public virtual IEnumerable<IDrawText> Construct(bool Active)
        { return null; }

        /// <summary>
        /// Width of window in char count
        /// </summary>
        public int Width;

        /// <summary>
        /// Height of window in char count
        /// </summary>
        public int Height;

        /// <summary>
        /// Left indent from console border in char count
        /// </summary>
        public int Left;

        /// <summary>
        /// Top indent from console border in char count
        /// </summary>
        public int Top;

        /// <summary>
        /// If you need close window after use interface, for example: Button 'Exit' which need exit window
        /// </summary>
        public bool CloseAfterUse = false;

        /// <summary>
        /// Return something from last Interface
        /// </summary>
        public object Return;
    }
}

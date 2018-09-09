using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Collections;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Globalization;
using Microsoft.Win32.SafeHandles;

namespace Rogue
{
    public static class ConsoleWindows
    {
        /// <summary>
        /// Window
        /// </summary>
        public class Window : Interface
        {
            /// <summary>
            /// Header of window
            /// </summary>
            protected List<ColouredChar> sHeader = new List<ColouredChar>();
            /// <summary>
            /// Text information in lines.
            /// </summary>
            protected List<List<ColouredChar>> sLines = new List<List<ColouredChar>>();
            /// <summary>
            /// Return converted header
            /// </summary>
            /// <returns></returns>
            public List<ColouredChar> GetHeader()
            {
                return this.sHeader;
            }
            /// <summary>
            /// Return converted text into Lines
            /// </summary>
            /// <returns></returns>
            public List<List<ColouredChar>> GetLines()
            {
                return this.sLines;
            }
            /// <summary>
            /// Enable animation. Be care, animation using Thread.Sleep, which mean animation drawning will stoped your programm
            /// </summary>
            public bool Animated;
            /// <summary>
            /// Animation
            /// </summary>
            public Animation Animation = Additional.StadartAnimation;
            /// <summary>
            /// Window need header? Default=false;
            /// </summary>
            public bool Header = false;
            /// <summary>
            /// Border bold
            /// </summary>
            public Border Border;
            /// <summary>
            /// Speed of animation in milliseconds between frames
            /// </summary>
            public int Speed = 15;
            /// <summary>
            /// Window-border color, if you have no-border, just set your background color or nothing(Black)
            /// </summary>
            public ConsoleColor BorderColor;
            /// <summary>
            /// Event when window after loading
            /// </summary>
            public Action OnLoad;
            /// <summary>
            /// All interfaces in window
            /// </summary>
            protected List<Interface> Controls = new List<Interface>();            
            /// <summary>
            /// Add interface object on window
            /// </summary>
            /// <param name="Control">Interface: Button, TextBox, CheckBox, RadioBox</param>
            public void AddControl(Interface Control)
            { this.Controls.Add(Control); }
            /// <summary>
            /// Current focused interface
            /// </summary>
            public Interface Sender = new Interface();
            /// <summary>
            /// Subj?
            /// </summary>
            protected int Focus = 0;
            /// <summary>
            /// Use active Sender
            /// </summary>
            protected void ActivateInterface()
            {
                if (this.Sender.GetType() == typeof(Button)) { if ((this.Sender as Button).OnClick != null) { (this.Sender as Button).OnClick(); } }
                if (this.Sender.GetType() == typeof(TextBox)) { (this.Sender as TextBox).Write(); }//{ if ((this.Sender as TextBox).OnKeyPress != null) { (this.Sender as TextBox).OnKeyPress(); } }
                if (this.Sender.GetType() == typeof(CheckBox)) { if ((this.Sender as CheckBox).OnCheckAdditional != null) { (this.Sender as CheckBox).OnCheckAdditional(); } }
                this.Return = Sender.Return;
                if (Sender.CloseAfterUse) { this.NeedClose = true; }
            }
            /// <summary>
            /// After window drawning, this main method
            /// </summary>
            protected void OnActivate()
            {
                bool Close = false;
                while (!Close)
                {
                    ConsoleKey k = Console.ReadKey(true).Key;
                    switch (k)
                    {
                        case ConsoleKey.LeftArrow:
                        case ConsoleKey.UpArrow: { up(); break; }
                        case ConsoleKey.DownArrow:
                        case ConsoleKey.RightArrow:
                        case ConsoleKey.Tab: { tab(); break; }
                        case ConsoleKey.Enter: { this.ActivateInterface(); if (this.NeedClose) { this.NeedClose = false; Close = true; } break; }
                        //case ConsoleKey.Escape: { Close = true; break; }
                    }
                }
            }
            protected bool tab()
            {
                if (Focus == this.Controls.Count - 1) { Focus = 0; this.Sender = Controls[0]; } else { Focus++; this.Sender = this.Controls[Focus]; } if (this.Sender.OnFocus != null) { this.Sender.OnFocus(); } ReconstructInterface(); if (this.Sender.GetType() == typeof(TextBox)) { this.ActivateInterface(); textboxendinput(); if (this.NeedClose) { this.NeedClose = false; return true; } }
                return false;
            }
            protected bool up()
            {
                if (Focus == 0)
                { Focus = this.Controls.Count - 1; this.Sender = Controls[this.Controls.Count - 1]; }
                else { Focus--; this.Sender = this.Controls[Focus]; } if (this.Sender.OnFocus != null)
                { this.Sender.OnFocus(); } ReconstructInterface(); //_Draw = this;
                if (this.Sender.GetType() == typeof(TextBox)) { this.ActivateInterface(); textboxendinput(); if (this.NeedClose) { this.NeedClose = false; return true; } }
                return false;
            }
            protected void textboxendinput()
            {
                if (Focus == 0) { up(); }
                else { tab();}
                //else if (Focus == this.Controls.Count - 1)
                //if (Focus == this.Controls.Count - 1) { tab(); }
                //else { up(); }
            }
            protected bool NeedClose = false;
            protected bool Constructed = false;
            /// <summary>
            /// First string is Header
            /// </summary>
            public Text Text
            {
                set
                {
                    //get data
                    var Lines = value.GetLines();
                    var Colors = value.GetForegroundColors();
                    var bColors = value.GetBackgroundColors();
                    var fPos = value.GetPositionsForegroundColors();
                    var bPos = value.GetPositionsBackgroundColors();

                    try
                    {
                        //convert header
                        this.sHeader = GetColouredLine(Lines[0], Colors[0], fPos[0], bColors[0], bPos[0]);

                        //convert text
                        for (int i = 1; i < Lines.Count; i++)
                        { this.sLines.Add(GetColouredLine(Lines[i], Colors[i], fPos[i], bColors[i], bPos[i])); }
                    }
                    catch { }
                }
            }
            /// <summary>
            /// Construct window, after construct you cant change window
            /// </summary>
            protected void ToConstruct()
            {
                Exception();
                if (this.Controls.Count > 0) { ConstructInterfaceMap(); }
                Constructed = true;

                //border color
                short bcolor = Convert.ToInt16(this.BorderColor);

                //Add top border
                this.sLines.Insert(0, GetColouredLine(this.Border.UpperLeftCorner + GetLine(this.Width - 2, this.Border.HorizontalLine) + this.Border.UpperRightCorner, new List<short>() { bcolor }, new List<int>(), new List<short>() { 0 }, new List<int>()));

                int count = -1;
                //Add header
                if (this.Header)
                {
                    //left side
                    this.sHeader.Insert(0, new ColouredChar() { Char = this.Border.VerticalLine, Color = bcolor });
                    this.sHeader.Insert(1, new ColouredChar() { Char = ' ', Color = bcolor });
                    //right side
                    this.sHeader.Insert(1, new ColouredChar() { Char = ' ', Color = bcolor });
                    this.sHeader.Add(new ColouredChar() { Char = this.Border.VerticalLine, Color = bcolor });

                    //Add nulls to header for border
                    if (this.sHeader.Count < this.Width)
                    {
                        int w = this.Width - this.sHeader.Count;
                        for (int j = 0; j < w; j++)
                        {
                            this.sHeader.Insert(this.sHeader.Count - 1, new ColouredChar() { Char = ' ', Color = 0 });
                        }
                    }

                    //add
                    this.sLines.Insert(1, this.sHeader);

                    this.sLines.Insert(2, GetColouredLine(this.Border.PerpendicularRightward + GetLine(this.Width - 2, this.Border.HorizontalLine) + this.Border.PerpendicularLeftward, new List<short>() { bcolor }, new List<int>(), new List<short>() { 0 }, new List<int>()));
                    count = 3;
                }
                else
                {
                    count = 1;
                }

                //Add bottom border
                this.sLines.Add(GetColouredLine(this.Border.LowerLeftCorner + GetLine(this.Width - 2, this.Border.HorizontalLine) + this.Border.LowerRightCorner, new List<short>() { bcolor }, new List<int>(), new List<short>() { 0 }, new List<int>()));

                for (int i = count; i < this.sLines.Count - 1; i++)
                {
                    //left side
                    this.sLines[i].Insert(0, new ColouredChar() { Char = this.Border.VerticalLine, Color = bcolor });
                    this.sLines[i].Insert(1, new ColouredChar() { Char = ' ', Color = 0 });
                    //right side
                    this.sLines[i].Add(new ColouredChar() { Char = ' ', Color = 0 });
                    this.sLines[i].Add(new ColouredChar() { Char = this.Border.VerticalLine, Color = bcolor });

                    if (this.sLines[i].Count < this.Width)
                    {
                        int w = this.Width - this.sLines[i].Count;
                        for (int j = 0; j < w; j++)
                        {
                            this.sLines[i].Insert(this.sLines[i].Count - 2, new ColouredChar() { Char = ' ', Color = 0 });
                        }
                    }
                }

                //if your text have string less count
                int index = this.sLines.Count - 1;
                while (this.sLines.Count != this.Height - 4)
                {
                    if (this.sLines.Count != this.Height - 4)
                    {
                        this.sLines.Insert(index, new List<ColouredChar>());
                        this.sLines[index] = GetColouredLine(this.Border.VerticalLine + GetLine(this.Width - 2, ' ') + this.Border.VerticalLine, new List<short>() { bcolor }, new List<int>(), new List<short>() { 0 }, new List<int>());
                    }
                }

                //Interface
                for (int i = 0; i < this.Controls.Count; i++)
                {
                    bool a = false;
                    if (i == this.Focus) { a = true; }
                    AddInterface(Controls[i], a);
                }
            }
            /// <summary>
            /// Construct Interface Map to Navigation
            /// </summary>
            protected void ConstructInterfaceMap()
            { this.Controls = new List<Interface>((from c in ((from b in this.Controls orderby b.Top select b).ToList<Interface>()) orderby c.Left select c).ToList<Interface>()); this.Sender = this.Controls[0]; }
            /// <summary>
            /// Add Interface on window
            /// </summary>
            /// <param name="Interface">Button, TextBox, CheckBox</param>
            protected void AddInterface(Interface Interface, bool Active)
            {
                //Get line of chars
                var Line = Interface.Construct(Active);

                //From which string we start merging
                int MergingLine = Interface.Top;

                //foreach merging line
                foreach (var l in Line)
                {
                    int j = 0;
                    //from which char start merging
                    for (int i = Interface.Left; i < l.Count+Interface.Left; i++)
                    {                        
                        this.sLines[MergingLine][i].BackColor = l[j].BackColor;
                        this.sLines[MergingLine][i].Char = l[j].Char;
                        this.sLines[MergingLine][i].Color = l[j].Color;
                        j++;
                    }
                    MergingLine++;
                }
            }
            /// <summary>
            /// For change focus and sender [LOL! here was Critical error! ]
            /// </summary>
            protected void ReconstructInterface()
            {
                //Interface
                for (int i = 0; i < this.Controls.Count; i++)
                {
                    bool a = false;
                    if (i == this.Focus) { a = true; }
                    //AddInterface(Controls[i], a);
                    DRAW_MAIN_VOID(Controls[i].Construct(a), this.Left+Controls[i].Left, this.Top+Controls[i].Top, Controls[i].Width, 3);
                }
            }
            /// <summary>
            /// Check window for fault in construct
            /// </summary>
            /// <returns></returns>
            private void Exception()
            {
                //Window size
                if (this.Height < 0 || this.Width < 0 || this.Left < 0 || this.Top < 0) { throw new WindowsExceptions.WindowSizeException(); }
                if (this.Height < (this.sLines.Count + 1 + 6)) { throw new WindowsExceptions.WindowLowHeigthException(); }
                foreach (List<ColouredChar> lc in this.sLines)
                {
                    if (lc.Count + 4 > this.Width) { throw new WindowsExceptions.WindowTextWidthException(); }
                }
                if (this.sHeader.Count + 4 > this.Width) { throw new WindowsExceptions.WindowTextWidthException(); }
                if (this.Width < 1) { throw new WindowsExceptions.WindowLowWidthException(); }
                //Window position
                if (this.Height > Console.BufferHeight || this.Height >= Console.BufferWidth || this.Left >= Console.BufferWidth || this.Top >= Console.BufferHeight)
                { throw new WindowsExceptions.WindowOutOfDesplayException(); }
                //if (this.Height != this.Width) { throw new WindowsExceptions.WindowAnimationSizeException();}
            }
            /// <summary>
            /// Draw window
            /// </summary>
            public void Draw()
            { if (Animated) { __Draw = this; } _Draw = this; this.OnActivate(); }
            protected static Window __Draw
            {
                set
                {
                    List<Window> Windows = new List<Window>();

                    switch (value.Animation.AnimationDirection)
                    {
                        case TextPosition.Center:
                            {
                                for (int i = 6; i < value.Width - 1 / value.Animation.Frames; i++)
                                {
                                    Window w = new Window() { BorderColor = value.BorderColor, Width = i + 1, Height = i + 1, Left = value.Left + ((value.Width / 2) - ((i + 1) / 2)), Top = value.Top + ((value.Height / 2) - ((i + 1) / 2)) };
                                    if (w.Top <= 0) { w.Top = 1; }
                                    w.Border = value.Border;
                                    w.Text = new Text(w);
                                    Windows.Add(w);
                                }
                                foreach (Window w in Windows)
                                {
                                    _Draw = w;
                                    Thread.Sleep(value.Speed);
                                }
                                break;
                            }
                        case TextPosition.Left:
                        case TextPosition.Right:
                            {
                                for (int i = 3; i < value.Height - 1 / value.Animation.Frames; i++)
                                {
                                    Window w = new Window() { BorderColor = value.BorderColor, Width = i + 1, Height = value.Height, Left = value.Left + ((value.Width / 2) - ((i + 1) / 2)), Top = value.Top };
                                    if (w.Top <= 0) { w.Top = 1; }
                                    w.Border = value.Border;
                                    w.Text = new Text(w);
                                    Windows.Add(w);
                                }
                                foreach (Window w in Windows)
                                {
                                    _Draw = w;
                                    Thread.Sleep(value.Speed);
                                }
                                break;
                            }
                        case TextPosition.None:
                            {
                                for (int i = 6; i < value.Height - 1 / value.Animation.Frames; i++)
                                {
                                    Window w = new Window() { BorderColor = value.BorderColor, Width = value.Width, Height = i + 1, Left = value.Left, Top = value.Top + ((value.Height / 2) - ((i + 1) / 2)) };
                                    if (w.Top <= 0) { w.Top = 1; }
                                    w.Border = value.Border;
                                    w.Text = new Text(w);
                                    Windows.Add(w);
                                }
                                foreach (Window w in Windows)
                                {
                                    _Draw = w;
                                    Thread.Sleep(value.Speed);
                                }
                                break;
                            }
                    }
                }
            }
            protected static Window _Draw
            {
                set
                {
                    SafeFileHandle h = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

                    if (!h.IsInvalid)
                    {
                        if (!value.Constructed) { value.ToConstruct(); }

                        int x = (value.Width); //- value.Left)+1;
                        int y = (value.Height);

                        CharInfo[] buf = new CharInfo[x * y];
                        SmallRect rect = new SmallRect() { Left = (short)(value.Left), Top = (short)value.Top, Right = (short)(value.Width + value.Left), Bottom = (short)((value.Height + value.Top) - 5) };

                        int i = 0;
                        int j = 0;
                        foreach (List<ColouredChar> Line in value.GetLines())
                        {
                            j = 0;
                            foreach (ColouredChar c in Line)
                            {
                                buf[i].Attributes = c.Color;//(short)((short)c.Color | (short)c.BackColor);
                                buf[i].Char = c.Char;
                                i++;
                                j++;
                            }
                            i += (value.Width - j);
                        }
                        //Array.Resize(ref buf, buf.Length - (4 * y));

                        bool b = WriteConsoleOutput(h, buf,
                          new Coord() { X = (short)x, Y = (short)y },
                          new Coord() { X = 0, Y = 0 },
                          ref rect);
                    }
                }
            }
            public static void DRAW_MAIN_VOID(List<List<ColouredChar>> Lines, int left, int top, int WidthCountChars, int Heightcountlines)
            {
                SafeFileHandle h = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

                if (!h.IsInvalid)
                {

                    CharInfo[] buf = new CharInfo[WidthCountChars * Heightcountlines];
                    SmallRect rect = new SmallRect() { Left = (short)(left), Top = (short)top, Right = (short)(WidthCountChars + left), Bottom = (short)((Heightcountlines + top)) };

                    int i = 0;
                    int j = 0;
                    foreach (List<ColouredChar> Line in Lines)
                    {
                        j = 0;
                        foreach (ColouredChar c in Line)
                        {
                            buf[i].Attributes = c.Color;//(short)((short)c.Color | (short)c.BackColor);
                            buf[i].Char = c.Char;
                            i++;
                            j++;
                        }
                        i += (WidthCountChars - j);
                    }
                    //Array.Resize(ref buf, buf.Length - (4 * y));

                    bool b = WriteConsoleOutput(h, buf,
                      new Coord() { X = (short)WidthCountChars, Y = (short)Heightcountlines },
                      new Coord() { X = 0, Y = 0 },
                      ref rect);
                }
            }
        }
        /// <summary>
        /// Interface: Interface; Object;
        /// </summary>
        public class Interface
        {
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
            public ConsoleColor ActiveColor = ConsoleColor.Black;
            /// <summary>
            /// Color of border / label when control inactive
            /// </summary>
            public ConsoleColor InactiveColor = ConsoleColor.Black;
            /// <summary>
            /// Label, or default text
            /// </summary>
            public String Label;
            /// <summary>
            /// Result with params of string (abcdef):  (red)a,(red)b,(blue)c,(green)d,(green)e,Exception;
            /// </summary>
            /// <param name="Line">String line</param>
            /// <param name="ForeColors">List of colors, 0-default color: (Red,Blue,Green)</param>
            /// <param name="ForePositions">List of positions when need change color, 0-default color (2,3,5)</param>
            /// <returns></returns>
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
                if (this.Width % 2 == 0 && this.Label.Length % 2 != 0) { String = ' ' + String; }
                if (this.Width % 2 != 0 && this.Label.Length % 2 == 0) { String += ' '; }
                return String;
            }
            public virtual List<List<ColouredChar>> Construct(bool Active)
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
        /// <summary>
        /// Interface: Button; Clickable;
        /// </summary>
        public class Button : Interface
        {
            public Button(Window Window)
            {
                this.Window = Window;
            }
            public Action OnClick;
            public override List<List<ColouredChar>> Construct(bool Active)
            {
                short cl = 0;
                if (Active) { cl = Convert.ToInt16(this.ActiveColor); } else { cl = Convert.ToInt16(this.InactiveColor); }

                List<List<ColouredChar>> l = new List<List<ColouredChar>>();
                l.Add(GetColouredLine(Window.Border.UpperLeftCorner + GetLine(this.Width - 2, Window.Border.HorizontalLine) + Window.Border.UpperRightCorner, new List<short>() { Convert.ToInt16(Window.BorderColor) }, new List<int>(), new List<short>(), new List<int>()));
                l.Add(GetColouredLine(Window.Border.VerticalLine + this.Middle(this.Label) + Window.Border.VerticalLine, new List<short>() { Convert.ToInt16(Window.BorderColor), cl, Convert.ToInt16(Window.BorderColor) }, new List<int>() { (this.Width / 2) - (this.Label.Length / 2), this.Width - 1 }, new List<short>(), new List<int>()));
                l.Add(GetColouredLine(Window.Border.LowerLeftCorner + GetLine(this.Width - 2, Window.Border.HorizontalLine) + Window.Border.LowerRightCorner, new List<short>() { Convert.ToInt16(Window.BorderColor) }, new List<int>(), new List<short>(), new List<int>()));

                return l;
            }
        }
        /// <summary>
        /// Interface: TextBox; Writeable;
        /// </summary>
        public class TextBox : Interface
        {
            public TextBox(Window Window)
            {
                this.Window = Window;
                this.OnFocus = () => { Console.CursorVisible = true; };
            }            
            public Action OnKeyPress;
            public Action OnEndTyping;
            protected string String="";
            public string Text
            {
                get
                {
                    //string s = String;
                    //String = "";
                    return String;
                }
            }
            public void Clear()
            {
                Console.SetCursorPosition(Window.Left + this.Left + 1, Window.Top + this.Top + 1);
                Console.Write(GetLine(this.Width - 2, ' '));
            }            
            public void Write()
            {
                Console.CursorVisible = true;
                Console.ForegroundColor = this.ActiveColor;
                this.Clear();                
                Console.SetCursorPosition(Window.Left + this.Left + 1, Window.Top + this.Top + 1);
                if (this.String != "") { if (this.String.Length >= this.Width - 2) { Console.Write(String.Substring(String.Length-(this.Width - 2))); } else { Console.Write(String); } }
                bool end = false;
                while (!end)
                {                    
                    ConsoleKeyInfo k = Console.ReadKey(true);                    
                    if (k.Key == ConsoleKey.Enter || k.Key == ConsoleKey.Escape) { if (this.OnEndTyping != null) { this.OnEndTyping(); } end = true; break; }
                    if (
                                    k.KeyChar != (char)27 &&
                                    k.KeyChar != '\0' &&
                                    k.KeyChar != '\t' &&
                                    k.KeyChar != '\\' &&
                                    k.KeyChar != '/' &&
                                    k.KeyChar != ':' &&
                                    k.KeyChar != '*' &&
                                    k.KeyChar != '?' &&
                                    k.KeyChar != '"' &&
                                    k.KeyChar != '<' &&
                                    k.KeyChar != '>' &&
                                    k.KeyChar != '|' &&
                                    k.KeyChar != '.' &&
                                    k.KeyChar != ',' &&
                                    k.KeyChar != '`' &&
                                    k.KeyChar != '~' &&
                                    k.KeyChar != '!' &&
                                    k.KeyChar != '#' &&
                                    k.KeyChar != '№' &&
                                    k.KeyChar != '@' &&
                                    k.KeyChar != '$' &&
                                    k.KeyChar != ';' &&
                                    k.KeyChar != '%' &&
                                    k.KeyChar != '^' &&
                                    k.KeyChar != '&' &&
                                    k.KeyChar != '(' &&
                                    k.KeyChar != ')' &&
                                    k.KeyChar != '-' &&
                                    k.KeyChar != '+' &&
                                    k.KeyChar != '='
                               )
                    { this.String += k.KeyChar; }
                    for (int i = 0; i < this.String.Length; i++)
                    {
                        try
                        {
                            if (String[i] == '\b') { String = String.Substring(0, String.Length - 2); }
                        }
                        catch { }
                    }
                    //if string more then textbox
                    if (this.String.Length >= this.Width - 2)
                    {                        
                        //write substring
                        this.Clear();
                        Console.SetCursorPosition(Window.Left + this.Left + 1, Window.Top + this.Top + 1);
                        Console.Write(String.Substring(this.String.Length - (this.Width-2)));
                    }
                    else
                    {
                        if (k.KeyChar == '\b') { this.Clear(); }
                        Console.SetCursorPosition(Window.Left + this.Left + 1, Window.Top + this.Top + 1);
                        Console.Write(this.String);
                    }
                }
                Console.CursorVisible = false;
            }
            public override List<List<ColouredChar>> Construct(bool Active)
            {
                short cl = 0;
                if (Active) { cl = Convert.ToInt16(this.ActiveColor); } else { cl = Convert.ToInt16(this.InactiveColor); }

                string printf = "";
                if (String != "") { if (String.Length >= this.Width - 2) { printf = String.Substring(0,this.Width - 2); } else { printf = String; } } else { printf = this.Label; }
                List<List<ColouredChar>> l = new List<List<ColouredChar>>();
                l.Add(GetColouredLine(Additional.LightBorder.UpperLeftCorner + GetLine(this.Width - 2, Additional.LightBorder.HorizontalLine) + Additional.LightBorder.UpperRightCorner, new List<short>() { Convert.ToInt16(Window.BorderColor) }, new List<int>(), new List<short>(), new List<int>()));
                l.Add(GetColouredLine(Additional.LightBorder.VerticalLine + printf+GetLine(this.Width-2-printf.Length,' ') + Additional.LightBorder.VerticalLine, new List<short>() { Convert.ToInt16(Window.BorderColor), cl, Convert.ToInt16(Window.BorderColor) }, new List<int>() {1, this.Width - 1 }, new List<short>(), new List<int>()));
                l.Add(GetColouredLine(Additional.LightBorder.LowerLeftCorner + GetLine(this.Width - 2, Additional.LightBorder.HorizontalLine) + Additional.LightBorder.LowerRightCorner, new List<short>() { Convert.ToInt16(Window.BorderColor) }, new List<int>(), new List<short>(), new List<int>()));
                return l;
            }
        }
        /// <summary>
        /// Interface: CheckBox; Boolean;
        /// </summary>
        public class CheckBox : Interface
        {
            public CheckBox(Window Window)
            {
                this.Window = Window;
            }
            public Action OnCheckAdditional;
            protected bool value = false;
            public bool Checked
            { get { return value; } }
            public void OnCheck()
            {
                if (value) { value = false; }
                else { value = true; }
            }
            public override List<List<ColouredChar>> Construct(bool Active)
            {
                short cl = 0;
                char cr = ' ';
                if (this.value) { cr = 'Y'; } else { cr = 'N'; }

                if (Active) { cl = Convert.ToInt16(this.ActiveColor); } else { cl = Convert.ToInt16(this.InactiveColor); }

                List<List<ColouredChar>> l = new List<List<ColouredChar>>();
                l.Add(GetColouredLine(Window.Border.UpperLeftCorner + GetLine(this.Width - 2, Window.Border.HorizontalLine) + Window.Border.UpperRightCorner, new List<short>() { Convert.ToInt16(Window.BorderColor) }, new List<int>(), new List<short>(), new List<int>()));
                l.Add(GetColouredLine(Window.Border.VerticalLine + Middle(cr + this.Label) + Window.Border.VerticalLine, new List<short>() { cl }, new List<int>(), new List<short>(), new List<int>()));
                l.Add(GetColouredLine(Window.Border.LowerLeftCorner + GetLine(this.Width - 2, Window.Border.HorizontalLine) + Window.Border.LowerRightCorner, new List<short>() { Convert.ToInt16(Window.BorderColor) }, new List<int>(), new List<short>(), new List<int>()));

                return l;
            }
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
        /// <summary>
        /// Exceptions, because without ex window just didn't draw
        /// </summary>
        public static class WindowsExceptions
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
        /// <summary>
        /// Class for working with window text
        /// </summary>
        public class Text
        {
            /// <summary>
            /// Window need to set size
            /// </summary>
            /// <param name="Window">Window for which text is written</param>
            public Text(Window Window)
            {
                this.Window = Window;
            }
            /// <summary>
            /// for not-null
            /// </summary>
            protected Window Window = new Window();
            /// <summary>
            /// String lines after set position and colors
            /// </summary>
            protected List<string> Lines = new List<string>();
            /// <summary>
            /// Foreground colors for each string in this.Lines
            /// </summary>
            protected List<List<short>> Colors = new List<List<short>>();
            /// <summary>
            /// Background colors for each string in this.Lines
            /// </summary>
            protected List<List<short>> BackColors = new List<List<short>>();
            /// <summary>
            /// Foreground colors (!)Positions for each string in this.Lines
            /// </summary>
            protected List<List<int>> PositionsFC = new List<List<int>>();
            /// <summary>
            /// Background colors (!)Positions for each string in this.Lines
            /// </summary>
            protected List<List<int>> PositionsBC = new List<List<int>>();
            /// <summary>
            /// Currend line for this.Lines.Add()
            /// </summary>
            protected string NowLine = "";
            /// <summary>
            /// Current string, uses if you write colored string
            /// </summary>
            protected string NowString = "";
            /// <summary>
            /// Buff of foreground colors in this.NowString
            /// </summary>
            protected List<short> NowForeColors = new List<short>();
            /// <summary>
            /// Buff of background colors in this.NowString
            /// </summary>
            protected List<short> NowBackColors = new List<short>();
            /// <summary>
            /// Buff of foreground colors positions in this.NowString
            /// </summary>
            protected List<int> NowForePos = new List<int>();
            /// <summary>
            /// Buff of background colors positions in this.NowString
            /// </summary>
            protected List<int> NowBackPos = new List<int>();
            /// <summary>
            /// Reset buffer colors and positions, and add to Lists
            /// </summary>
            protected void ResetColors()
            {
                this.Colors.Add(new List<short>(NowForeColors));
                this.PositionsFC.Add(new List<int>(NowForePos));
                this.BackColors.Add(new List<short>(NowBackColors));
                this.PositionsBC.Add(new List<int>(NowBackPos));
                //last colors
                short lastcolor = -1;
                try { lastcolor = NowForeColors[NowForeColors.Count - 1]; }
                catch { }
                short lastbcolor = -1;
                try { lastbcolor = NowBackColors[NowBackColors.Count - 1]; }
                catch { }
                //reset
                this.NowBackColors.Clear();
                this.NowBackPos.Clear();
                this.NowForeColors.Clear();
                this.NowForePos.Clear();
                //last Fcolor and Bcolor witch we are set need to be in next string                
                if (lastcolor != -1) { this.NowForeColors.Add(lastcolor); }
                if (lastbcolor != -1) { this.NowBackColors.Add(lastbcolor); }
            }
            /// <summary>
            /// Add spaces to string
            /// </summary>
            /// <param name="String">String for positioning</param>
            /// <returns></returns>
            protected string SetPosition(string String)
            {
                switch (this.TextPosition)
                {
                    case ConsoleWindows.TextPosition.None:
                        {
                            break;
                        }
                    case ConsoleWindows.TextPosition.Left:
                        {
                            if (String.Length < Window.Width - 4)
                            {
                                int l = (Window.Width - 4) - String.Length;
                                for (int i = 0; i < l; i++)
                                {
                                    String += ' ';
                                }
                            }
                            //Lines.Add(NowLine);
                            break;
                        }
                    case ConsoleWindows.TextPosition.Center:
                        {
                            if (String.Length < Window.Width - 4)
                            {
                                int side = ((Window.Width - 4) - String.Length) / 2;
                                for (int i = 0; i < side; i++)
                                {
                                    String = ' ' + String;
                                    String += ' ';
                                }
                                for (int i = 0; i < this.NowForePos.Count; i++)
                                {
                                    this.NowForePos[i] = this.NowForePos[i] + side;
                                }
                                for (int i = 0; i < this.NowBackPos.Count; i++)
                                {
                                    this.NowBackPos[i] = this.NowBackPos[i] + side;
                                }
                            }
                            break;
                        }
                    case ConsoleWindows.TextPosition.Right:
                        {
                            if (String.Length < Window.Width - 4)
                            {
                                int l = (Window.Width - 4) - String.Length;
                                for (int i = 0; i < l; i++)
                                {
                                    String = ' ' + String;
                                }
                                for (int i = 0; i < this.NowForePos.Count; i++)
                                {
                                    this.NowForePos[i] = this.NowForePos[i] + l;
                                }
                                for (int i = 0; i < this.NowBackPos.Count; i++)
                                {
                                    this.NowBackPos[i] = this.NowBackPos[i] + l;
                                }
                            }
                            break;
                        }
                }
                return String;
            }
            /// <summary>
            /// Text position, default=none
            /// </summary>
            public TextPosition TextPosition = TextPosition.None;
            /// <summary>
            /// Current color of text
            /// </summary>
            public ConsoleColor ForegroundColor
            {
                set
                {
                    if (NowString == "") { try { NowForeColors[0] = Convert.ToInt16(value); } catch { NowForeColors = new List<short>() { Convert.ToInt16(value) }; } }
                    else
                    {
                        NowForeColors.Add(Convert.ToInt16(value));
                        try { NowForePos.Add(NowString.Length); }
                        catch { NowForePos.Add(0); }
                    }
                }
            }
            /// <summary>
            /// Current background color or text
            /// </summary>
            public ConsoleColor BackgroundColor
            {
                set
                {
                    if (NowString == "") { try { NowBackColors[0] = Convert.ToInt16(value); } catch { NowBackColors = new List<short>() { Convert.ToInt16(value) }; } }
                    else
                    {
                        NowBackColors.Add(Convert.ToInt16(value));
                        try { NowBackPos.Add(NowString.Length); }
                        catch { NowBackPos.Add(0); }
                    }
                }
            }
            /// <summary>
            /// Add string to current string, you can use '\n' for append line
            /// </summary>
            /// <param name="String"></param>
            public void Write(string String)
            {
                NowString += String;
                NowLine += String;
                if (NowLine.IndexOf('\n') > -1)
                {
                    NowLine = NowLine.Remove(NowLine.IndexOf('\n'));
                    Lines.Add(this.SetPosition(NowLine));
                    ResetColors();
                    NowLine = "";
                    NowString = "";
                }
            }
            /// <summary>
            /// Add '\n' to current string
            /// </summary>
            public void AppendLine()
            {
                Lines.Add(this.SetPosition(NowLine));
                ResetColors();
                NowLine = "";
                NowString = "";
            }
            /// <summary>
            /// Write string with '\n'
            /// </summary>
            /// <param name="String">String</param>
            public void WriteLine(string String)
            {
                NowString = String;
                NowLine += String;

                Lines.Add(this.SetPosition(NowLine));
                ResetColors();
                NowLine = "";
                NowString = "";
            }
            /// <summary>
            /// Return list of string lines in this text
            /// </summary>
            /// <returns></returns>
            public List<string> GetLines() { return this.Lines; }
            /// <summary>
            /// Return list of list foreground colors in this text
            /// </summary>
            /// <returns></returns>
            public List<List<short>> GetForegroundColors() { return this.Colors; }
            /// <summary>
            /// Return list of list background colors in this text
            /// </summary>
            /// <returns></returns>
            public List<List<short>> GetBackgroundColors() { return this.BackColors; }
            /// <summary>
            /// Return list of list foreground colors positions in this text
            /// </summary>
            /// <returns></returns>
            public List<List<int>> GetPositionsForegroundColors() { return this.PositionsFC; }
            /// <summary>
            /// Return list of list background colors positions in this text
            /// </summary>
            /// <returns></returns>
            public List<List<int>> GetPositionsBackgroundColors() { return this.PositionsBC; }
        }
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
        /// <summary>
        /// Animation class
        /// </summary>
        public class Animation
        {
            public string Name;
            /// <summary>
            /// Number which will be divided on width
            /// </summary>
            public double Frames;
            public TextPosition AnimationDirection;
        }
        /// <summary>
        /// Charset for window border
        /// </summary>
        public class Border
        {
            public char UpperLeftCorner { set; get; }
            public char UpperRightCorner { set; get; }
            public char HorizontalLine { set; get; }
            public char VerticalLine { set; get; }
            public char LowerLeftCorner { set; get; }
            public char LowerRightCorner { set; get; }
            public char PerpendicularLeftward { set; get; }
            public char PerpendicularRightward { set; get; }
        }
        /// <summary>
        /// Position of text in window (or animation direct)
        /// </summary>
        public enum TextPosition
        { None = 0, Left = 1, Center = 2, Right = 3 }
        /// <summary>
        /// Example of window
        /// </summary>
        /// <param name="Border">Window will be with border or not</param>
        /// <param name="CustomBorder">Window border will be custom or one of additional</param>
        /// <param name="Header">Window will be with header or not</param>
        /// <param name="Animation">Window will be animated or not</param>
        /// <param name="AnimationType">Window with animation can have one of 3 animations (0-2)</param>
        public static void Example(bool Border = true, bool CustomBorder = false, bool Header = false, bool Animation = false, int AnimationType = 0)
        {
            Window w = new Window();

            //Size
            w.Width = 30;
            w.Height = 30;

            //Position
            w.Left = 0;
            w.Top = 0;

            //Border settings
            var b = Additional.BoldBorder;
            if (Border) { w.BorderColor = ConsoleColor.DarkCyan; }
            if (CustomBorder)
            {
                b.UpperRightCorner = '@';
                b.UpperLeftCorner = '@';
                b.HorizontalLine = '~';
                b.PerpendicularLeftward = '@';
                b.PerpendicularRightward = '@';
            }
            w.Border = b;

            //header
            w.Header = Header;

            //Animation
            if (Animation)
            {
                w.Animated = true;
            }
            if (AnimationType <= 0) { w.Animation = Additional.StadartAnimation; }
            if (AnimationType == 1) { w.Animation = Additional.HorizontalAnimation; }
            if (AnimationType >= 2) { w.Animation = Additional.CenterAnimation; }


            //Text in window
            Text t = new Text(w);
            t.TextPosition = TextPosition.Center;
            t.BackgroundColor = ConsoleColor.Black;
            List<char> rainbow = new List<char>() { 'S', 'c', 'r', 'o', 'l', 'l' };
            for (int i = 0; i < 6; i++)
            {
                t.ForegroundColor = (ConsoleColor)i + 4;
                t.Write(Convert.ToString(rainbow[i]));
            }
            t.AppendLine();
            t.ForegroundColor = ConsoleColor.DarkCyan;
            t.WriteLine("Old scroll of power!");
            t.AppendLine();
            string s = "";
            for (int i = 0; i < w.Width - 4; i++) { s += '~'; }
            t.WriteLine(s);

            t.TextPosition = TextPosition.Center;

            t.ForegroundColor = ConsoleColor.DarkBlue;
            t.Write("Attack Damage: ");
            t.ForegroundColor = ConsoleColor.Green;
            t.Write("+5");
            t.AppendLine();

            t.ForegroundColor = ConsoleColor.DarkMagenta;
            t.Write("Ability Power: ");
            t.ForegroundColor = ConsoleColor.Green;
            t.Write("+2");
            t.AppendLine();

            t.AppendLine();

            t.ForegroundColor = ConsoleColor.DarkCyan;
            s = "";
            for (int i = 0; i < w.Width - 4; i++) { s += '='; }
            t.WriteLine(s);
            t.TextPosition = TextPosition.Right;
            t.ForegroundColor = ConsoleColor.DarkYellow;
            t.WriteLine("<Special Effect>");
            t.ForegroundColor = ConsoleColor.DarkCyan;

            t.WriteLine(s);

            t.TextPosition = TextPosition.Center;

            t.ForegroundColor = ConsoleColor.DarkGreen;
            t.Write("When you use ");
            t.ForegroundColor = ConsoleColor.Red;
            t.Write("*Fireball*");
            t.ForegroundColor = ConsoleColor.DarkGreen;
            t.Write(",");
            t.AppendLine();
            t.WriteLine("you have chance to get");
            t.WriteLine("special effect:");
            t.TextPosition = TextPosition.Left;
            t.ForegroundColor = ConsoleColor.Yellow;
            t.AppendLine();
            t.WriteLine("Battle trance:");
            t.TextPosition = TextPosition.Right;
            t.WriteLine("\"");
            t.TextPosition = TextPosition.Center;
            t.ForegroundColor = ConsoleColor.Red;
            t.Write("Hit Points: ");
            t.ForegroundColor = ConsoleColor.Green;
            t.Write("+1");
            t.AppendLine();
            t.ForegroundColor = ConsoleColor.Blue;
            t.Write("Mana Points: ");
            t.ForegroundColor = ConsoleColor.Green;
            t.Write("+1");
            t.AppendLine();
            t.TextPosition = TextPosition.Left;
            t.ForegroundColor = ConsoleColor.Yellow;
            t.AppendLine();
            t.WriteLine("\"");

            //finally
            w.Text = t;
            w.Draw();
        }
        #region API
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern SafeFileHandle CreateFile(
            string fileName,
            [MarshalAs(UnmanagedType.U4)] uint fileAccess,
            [MarshalAs(UnmanagedType.U4)] uint fileShare,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            [MarshalAs(UnmanagedType.U4)] int flags,
            IntPtr template);

        [DllImport("kernel32.dll", SetLastError = true, EntryPoint = "WriteConsoleOutputW")]
        public static extern bool WriteConsoleOutput(
          SafeFileHandle hConsoleOutput,
          CharInfo[] lpBuffer,
          Coord dwBufferSize,
          Coord dwBufferCoord,
          ref SmallRect lpWriteRegion);

        [StructLayout(LayoutKind.Sequential)]
        public struct Coord
        {
            public short X;
            public short Y;

            public Coord(short X, short Y)
            {
                this.X = X;
                this.Y = Y;
            }
        };

        [StructLayout(LayoutKind.Explicit)]
        public struct CharUnion
        {
            [FieldOffset(0)]
            public char UnicodeChar;
            [FieldOffset(0)]
            public byte AsciiChar;
        }

        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
        public struct CharInfo
        {
            [FieldOffset(0)]
            public char Char;
            [FieldOffset(2)]
            public short Attributes;

        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SmallRect
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }
        #endregion
    }
}

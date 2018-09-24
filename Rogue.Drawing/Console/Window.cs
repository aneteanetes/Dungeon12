using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Console
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
        public void ActivateInterface()
        {
            if (this.Sender.GetType() == typeof(Button)) { if ((this.Sender as Button).OnClick != null) { (this.Sender as Button).OnClick(); } }
            if (this.Sender.GetType() == typeof(TextBox)) { (this.Sender as TextBox).Run(); }
            if (this.Sender.GetType() == typeof(CheckBox)) { if ((this.Sender as CheckBox).OnCheckAdditional != null) { (this.Sender as CheckBox).OnCheckAdditional(); } }
            this.Return = Sender.Return;
            if (Sender.CloseAfterUse) { this.NeedClose = true; }
        }
        /// <summary>
        /// After window drawning, this main method
        /// </summary>
        protected void OnActivate()
        {
            // just wait

            // TODO: delegate it


            //bool Close = false;
            //while (!Close)
            //{
            //    ConsoleKey k = Console.ReadKey(true).Key;
            //    switch (k)
            //    {
            //        case ConsoleKey.LeftArrow:
            //        case ConsoleKey.UpArrow: { up(); break; }
            //        case ConsoleKey.DownArrow:
            //        case ConsoleKey.RightArrow:
            //        case ConsoleKey.Tab: { tab(); break; }
            //        case ConsoleKey.Enter: { this.ActivateInterface(); if (this.NeedClose) { this.NeedClose = false; Close = true; } break; }
            //            //case ConsoleKey.Escape: { Close = true; break; }
            //    }
            //}
        }
        public bool tab()
        {
            if (Focus == this.Controls.Count - 1) { Focus = 0; this.Sender = Controls[0]; } else { Focus++; this.Sender = this.Controls[Focus]; }
            if (this.Sender.OnFocus != null) { this.Sender.OnFocus(); }
            ReconstructInterface(); if (this.Sender.GetType() == typeof(TextBox)) { this.ActivateInterface(); textboxendinput(); if (this.NeedClose) { this.NeedClose = false; return true; } }
            return false;
        }
        public bool up()
        {
            if (Focus == 0)
            { Focus = this.Controls.Count - 1; this.Sender = Controls[this.Controls.Count - 1]; }
            else { Focus--; this.Sender = this.Controls[Focus]; }
            if (this.Sender.OnFocus != null)
            { this.Sender.OnFocus(); }
            ReconstructInterface(); //_Draw = this;
            if (this.Sender.GetType() == typeof(TextBox)) { this.ActivateInterface(); textboxendinput(); if (this.NeedClose) { this.NeedClose = false; return true; } }
            return false;
        }
        public void textboxendinput()
        {
            if (Focus == 0) { up(); }
            else { tab(); }
            //else if (Focus == this.Controls.Count - 1)
            //if (Focus == this.Controls.Count - 1) { tab(); }
            //else { up(); }
        }
        public bool NeedClose = false;
        public bool Constructed = false;
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
        {
            this.Controls = new List<Interface>((from c in ((from b in this.Controls orderby b.Top select b).ToList<Interface>()) orderby c.Left select c).ToList<Interface>()); this.Sender = this.Controls[0];
        }
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
                for (int i = Interface.Left; i < l.Count + Interface.Left; i++)
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

                var ctrl = Controls[i];
                ctrl.Construct(a);
                ctrl.Run();
                ctrl.Publish();

                //DRAW_MAIN_VOID(Controls[i].Construct(a), this.Left + Controls[i].Left, this.Top + Controls[i].Top, Controls[i].Width, 3);
            }
        }
        /// <summary>
        /// Check window for fault in construct
        /// </summary>
        /// <returns></returns>
        private void Exception()
        {
            //Window size
            if (this.Height < 0 || this.Width < 0 || this.Left < 0 || this.Top < 0) { throw new WindowSizeException(); }
            if (this.Height < (this.sLines.Count + 1 + 6)) { throw new WindowLowHeigthException(); }
            foreach (List<ColouredChar> lc in this.sLines)
            {
                if (lc.Count + 4 > this.Width) { throw new WindowTextWidthException(); }
            }
            if (this.sHeader.Count + 4 > this.Width) { throw new WindowTextWidthException(); }
            if (this.Width < 1) { throw new WindowLowWidthException(); }

            //Window position
            if (this.Height > 40 || this.Height >= 100 || this.Left >= 100 || this.Top >= 40)
            { throw new WindowOutOfDesplayException(); }

            //if (this.Height != this.Width) { throw new WindowsExceptions.WindowAnimationSizeException();}
        }
        
        /// <summary>
        /// animation temporary disabled
        /// </summary>
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
                                //_Draw = w;
                                //Thread.Sleep(value.Speed);
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
                                //_Draw = w;
                                //Thread.Sleep(value.Speed);
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
                                //_Draw = w;
                                //Thread.Sleep(value.Speed);
                            }
                            break;
                        }
                }
            }
        }
        
        public override void Publish()
        {
            base.Publish();
            this.OnActivate();
        }

        public override IDrawSession Run()
        {
            this.DrawRegion = new Types.Rectangle
            {
                X = this.Left,
                Y = this.Top,
                Width = this.Width,
                Height = this.Height-4
            };

            if (!this.Constructed)
                this.ToConstruct();

            var lines = this.GetLines();

            foreach (List<ColouredChar> line in lines)
            {
                var linePos = lines.IndexOf(line);

                foreach (ColouredChar c in line)
                {
                    var charPos = line.IndexOf(c);

                    this.Write(linePos, charPos, c.Char.ToString(), (ConsoleColor)c.Color, (ConsoleColor)c.BackColor);
                }
            }

            return base.Run();
        }
    }
}

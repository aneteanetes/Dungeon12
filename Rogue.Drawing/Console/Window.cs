using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rogue.Control.Keys;
using Rogue.Drawing.Impl;
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
        protected List<Interface> ActivatableControls = new List<Interface>();
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
        public void ActivateInterface(KeyArgs args)
        {
            if (this.Sender.GetType() == typeof(Button)) { (this.Sender as Button).OnClick?.Invoke(); }
            if (this.Sender.GetType() == typeof(TextBox))
            {
                //(this.Sender as TextBox).OnKeyPress(args);
            }
            //if (this.Sender.GetType() == typeof(CheckBox)) { if ((this.Sender as CheckBox).OnCheckAdditional != null) { (this.Sender as CheckBox).OnCheckAdditional(); } }
            this.Return = Sender.Return;
            if (Sender.CloseAfterUse) { this.NeedClose = true; }
        }

        public void PropagateInput(KeyArgs args)
        {
            if (this.Sender is TextBox textBox)
            {
                textBox.OnKeyPress(args);
            }
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

        public bool Tab(KeyArgs args)
        {
            if (Focus == this.ActivatableControls.Count - 1)
            {
                Focus = 0;
                this.Sender.Active = false;
                this.Sender = ActivatableControls[0];
            }
            else
            {
                Focus++;
                this.Sender.Active = false;
                this.Sender = this.ActivatableControls[Focus];
            }

            this.Sender.OnFocus?.Invoke();

            ReconstructInterface();

            //if (this.Sender.GetType() == typeof(TextBox))
            //{
            //    this.ActivateInterface(args);
            //    textboxendinput(args);
            //    if (this.NeedClose)
            //    {
            //        this.NeedClose = false; return true;
            //    }
            //}

            return false;
        }

        public bool Up(KeyArgs args)
        {
            if (Focus == 0)
            {
                Focus = this.ActivatableControls.Count - 1;
                this.Sender.Active = false;
                this.Sender = ActivatableControls[this.ActivatableControls.Count - 1];
            }
            else
            {
                Focus--;
                this.Sender.Active = false;
                this.Sender = this.ActivatableControls[Focus];
            }
            this.Sender.OnFocus?.Invoke();

            ReconstructInterface(); //_Draw = this;


            return false;
        }

        public void textboxendinput(KeyArgs args)
        {
            if (Focus == 0) { Up(args); }
            else { Tab(args); }
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

            this.DrawRegion = new Types.Rectangle
            {
                X = this.Left,
                Y = this.Top,
                Width = this.Width,
                Height = this.Height
            };

            Exception();
            if (this.Controls.Count > 0) { ConstructInterfaceMap(); }
            Constructed = true;

            //border color
            short bcolor = Convert.ToInt16(this.BorderColor);

            #region topBorder

            var topBorder = DrawText.Empty(this.Width);
            topBorder.ForegroundColor = new DrawColor(this.BorderColor);

            var emptyLine = GetLine(this.Width - 2, this.Border.HorizontalLine);

            topBorder.ReplaceAt(0, new DrawText(this.Border.UpperLeftCorner.ToString(), this.BorderColor));
            topBorder.ReplaceAt(1, new DrawText(emptyLine, this.BorderColor));
            topBorder.ReplaceAt(emptyLine.Length + 1, new DrawText(this.Border.UpperRightCorner.ToString(), this.BorderColor));

            this.Write(0, 0, topBorder);

            #endregion

            #region size
            DrawText lineWithBorders() => new DrawText(Border.VerticalLine + GetLine(this.Width - 2, ' ') + Border.VerticalLine, this.BorderColor);

            for (int i = 1; i < this.Height-1; i++)
            {
                this.Write(i, 0, lineWithBorders());
            }

            #endregion

            #region bottomBorder

            var bottomBorder = DrawText.Empty(this.Width);
            bottomBorder.ForegroundColor = new DrawColor(this.BorderColor);

            bottomBorder.ReplaceAt(0, new DrawText(this.Border.LowerLeftCorner.ToString(), this.BorderColor));
            bottomBorder.ReplaceAt(1, new DrawText(emptyLine, this.BorderColor));
            bottomBorder.ReplaceAt(emptyLine.Length + 1, new DrawText(this.Border.LowerRightCorner.ToString(), this.BorderColor));

            this.Write(this.Height-1, 0, bottomBorder);

            #endregion

            //Interface
            for (int i = 0; i < this.Controls.Count; i++)
            {
                AddInterface(Controls[i], Controls[i]==this.Sender);
            }
        }
        /// <summary>
        /// Construct Interface Map to Navigation
        /// </summary>
        protected void ConstructInterfaceMap()
        {
            this.ActivatableControls = new List<Interface>((from c in ((from b in this.Controls where b.Activatable orderby b.Top select b).ToList<Interface>()) orderby c.Left select c).ToList<Interface>());
            this.Sender = this.ActivatableControls[0];
            this.Focus = 0;
        }
        /// <summary>
        /// Add Interface on window
        /// </summary>
        /// <param name="Interface">Button, TextBox, CheckBox</param>
        protected void AddInterface(Interface Interface, bool Active)
        {
            //Get line of chars
            var lines = Interface.Construct(Active);
            var mergeLine = Interface.Top; //какого хуя тут был магический +1 ?
            foreach (var line in lines)
            {
                this.Write(mergeLine, Interface.Left, line);
                mergeLine++;
            }
        }
        /// <summary>
        /// For change focus and sender [LOL! here was Critical error! ]
        /// </summary>
        protected void ReconstructInterface()
        {
            //Interface
            for (int i = 0; i < this.ActivatableControls.Count; i++)
            {
                var ctrl = ActivatableControls[i];
                ctrl.Construct(ActivatableControls[i]==Sender);
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
            if (!this.Constructed)
                this.ToConstruct();

            return base.Run();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Drawing.Console
{
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
                case TextPosition.None:
                    {
                        break;
                    }
                case TextPosition.Left:
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
                case TextPosition.Center:
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
                case TextPosition.Right:
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
}

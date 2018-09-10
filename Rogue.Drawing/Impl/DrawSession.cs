using System;
using System.Collections.Generic;
using Rogue.Types;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Impl
{
    public class DrawSession : IDrawSession
    {
        /// <summary>
        /// automatic clear this <see cref="DrawRegion"/>
        /// if disabled - clearing only current symbol
        /// </summary>
        public bool AutoClear { get; set; } = true;

        private List<IDrawText> buffer = new List<IDrawText>();

        private Rectangle _drawRegion;
        public Rectangle DrawRegion
        {
            get => _drawRegion;
            set
            {
                this._drawRegion = value;

                this.buffer = new List<IDrawText>();

                for (int i = 0; i < this._drawRegion.Height; i++)
                {
                    buffer.Add(DrawText.Empty(this._drawRegion.Width));
                }
            }
        }

        public void Write(int linePos, int charPos, IDrawText text)
        {
            this.buffer[linePos].InsertAt(charPos, text);
        }

        public void Write(int linePos, int charPos, string text, ConsoleColor foreColor = 0, ConsoleColor backColor = 0)
            => this.Write(linePos, charPos, new DrawText(text, foreColor, backColor));

        public void Write(int linePos, int charPos, string text, IDrawColor foreColor = null, IDrawColor backColor = null)
            => this.Write(linePos, charPos, new DrawText(text, foreColor, backColor));

        public void Batch(int linePos, int charPos, List<IDrawText> lines)
        {
            foreach (var line in lines)
            {
                this.buffer[linePos].InsertAt(charPos, line);

                linePos++;
            }
        }
        
        protected virtual void Clear()
        {
            Draw.RunSession<ClearSession>(x => x.DrawRegion = this.DrawRegion);
        }

        public virtual IDrawSession Run()
        {
            return this;
        }

        public virtual void Publish()
        {
            if (this.AutoClear)
                this.Clear();
            

            throw new System.NotImplementedException();
        }

        protected void WriteStatFull(string stringBuffer, int line, ConsoleColor color, ConsoleColor backColor = ConsoleColor.Black)
        {
            var pos = (23 / 2) - ((stringBuffer.Length) / 2);
            stringBuffer = DrawHelp.FullLine(stringBuffer.Length, stringBuffer, stringBuffer.Length - 1);
            this.Write(line, pos + 1, stringBuffer, color, backColor);
        }

        protected void WriteStatFull(string stringBuffer, int line, IDrawColor color, IDrawColor backColor = null)
        {
            var pos = (23 / 2) - ((stringBuffer.Length) / 2);
            stringBuffer = DrawHelp.FullLine(stringBuffer.Length, stringBuffer, stringBuffer.Length - 1);
            this.Write(line, pos + 1, stringBuffer, color, backColor);
        }

        protected void WriteHeader(string stringBuffer)
        {
            int Count = (100 / 2) - (stringBuffer.Length / 2);
            stringBuffer = DrawHelp.FullLine(stringBuffer.Length, stringBuffer, stringBuffer.Length - 1);
            this.Write(1, Count + 1, new DrawText(stringBuffer, ConsoleColor.DarkGreen));
        }
    }
}

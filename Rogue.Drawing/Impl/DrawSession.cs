using System;
using System.Collections.Generic;
using System.Linq;
using Rogue.Control.Events;
using Rogue.Entites;
using Rogue.Settings;
using Rogue.Types;
using Rogue.View.Interfaces;
using Rogue.View.Publish;

namespace Rogue.Drawing.Impl
{
    public class DrawSession : IDrawSession
    {
        protected ColorSchema ColorSchema = new ColorSchema();

        protected IDrawColor ColorSchemaColor => new DrawColor(ColorSchema.R, ColorSchema.G, ColorSchema.B, ColorSchema.A);

        /// <summary>
        /// automatic clear this <see cref="DrawRegion"/>
        /// if disabled - clearing only current symbol
        /// </summary>
        public bool AutoClear { get; set; } = true;

        private List<IDrawText> buffer = new List<IDrawText>();

        public Rectangle DrawRegion
        {
            get => SessionRegion;
            set
            {
                this.SessionRegion = value;

                this.buffer = new List<IDrawText>();

                for (int i = 0; i < this.SessionRegion.Height; i++)
                {
                    buffer.Add(DrawText.Empty((int)this.SessionRegion.Width));
                }
            }
        }

        public IEnumerable<IDrawText> TextContent => this.buffer.ToArray().Concat(WanderingText);

        protected List<IDrawText> WanderingText = new List<IDrawText>();

        public virtual Rectangle SessionRegion { get; private set; }

        public Rectangle Location => SessionRegion;

        public virtual IEnumerable<IDrawable> Drawables { get; set; }

        protected List<IDrawablePath> Paths = new List<IDrawablePath>();
        public IEnumerable<IDrawablePath> DrawablePaths => Paths;

        public void Write(int linePos, int charPos, IDrawText text)
        {
            this.buffer[linePos].ReplaceAt(charPos, text);
        }

        public void Write(int linePos, int charPos, string text, ConsoleColor foreColor = 0, ConsoleColor backColor = 0)
            => this.Write(linePos, charPos, new DrawText(text, foreColor, backColor));

        public void Write(int linePos, int charPos, string text, IDrawColor foreColor = null, IDrawColor backColor = null)
            => this.Write(linePos, charPos, new DrawText(text, foreColor, backColor));

        public void Batch(int linePos, int charPos, List<IDrawText> lines)
        {
            foreach (var line in lines)
            {
                this.buffer[linePos].ReplaceAt(charPos, line);

                linePos++;
            }
        }

        public virtual IDrawSession Run()
        {
            return this;
        }

        public virtual void Publish()
        {
            PublishManager.Publish(new List<IDrawSession>() { this });
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

        public virtual bool IsControlable => false;

        public virtual void Handle(ControlEventType @event) { }
    }
}
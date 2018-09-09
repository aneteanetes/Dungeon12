using System.Collections.Generic;
using Rogue.Settings;
using Rogue.Types;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Impl
{
    public class DrawSession : IDrawSession
    {
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

        public void Publish()
        {
            if (this.AutoClear)
                this.Clear();
            

            throw new System.NotImplementedException();
        }
    }
}

using Dungeon.Drawing;
using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.Data
{
    public class PersistColor : DrawColor
    {
        public PersistColor() : base(ConsoleColor.White)
        {
        }

        public string KnownColor { get; set; }

        public IDrawColor GetColor()
        {
            if (KnownColor != default)
            {
                return typeof(DrawColor).GetStaticProperty(KnownColor).As<IDrawColor>();
            }

            return new DrawColor(this.R, this.G, this.B, this.A)
            {
                Opacity=this.Opacity
            };
        }
    }
}

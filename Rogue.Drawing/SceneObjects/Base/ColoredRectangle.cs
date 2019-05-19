namespace Rogue.Drawing.SceneObjects.Base
{
    using System;
    using Rogue.Drawing.Impl;
    using Rogue.Settings;
    using Rogue.View.Interfaces;

    public class ColoredRectangle : HandleSceneControl
    {
        public ConsoleColor Color { get; set; }

        public bool Fill { get; set; }

        private double opacity;
        public double Opacity
        {
            get => opacity;
            set
            {
                opacity = value;
                drawablePath = null;
            }
        }

        public int Depth { get; set; } = 1;

        public double Round { get; set; } = 0;

        protected void UpdatePath()
        {
            this.drawablePath = null;
        }

        private DrawablePath drawablePath;
        public override IDrawablePath Path
        {
            get
            {
                if (drawablePath == null)
                {
                    var color = new DrawColor(this.Color)
                    {
                        Opacity = Opacity,
                        A = 255
                    };

                    drawablePath = new DrawablePath
                    {
                        Fill = this.Fill,
                        BackgroundColor = color,
                        Depth = this.Depth,
                        PathPredefined = View.Enums.PathPredefined.Rectangle,
                        Region = this.Position,
                        Radius = (float)this.Round
                    };
                }

                return drawablePath;
            }
        }

        public ColoredRectangle WithText(IDrawText drawText, bool center = false)
        {
            if (center)
            {
                this.AddTextCenter(drawText);
            }
            else
            {
                var textControl = new TextControl(drawText);
                this.Children.Add(textControl);
            }

            return this;
        }

        public ColoredRectangle DarkPanel()
        {
            Color = ConsoleColor.Black;
            Depth = 1;
            Fill = true;
            Opacity = 0.5;
            Round = 5;

            return this;
        }

    }

    public class DarkRectangle : ColoredRectangle
    {
        public DarkRectangle()
        {
            Color = ConsoleColor.Black;
            Depth = 1;
            Fill = true;
            Opacity = 0.5;
            Round = 5;
        }
    }
}
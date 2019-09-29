namespace Rogue.Drawing.SceneObjects.Base
{
    using System;
    using Rogue.Drawing.Impl;
    using Rogue.Settings;
    using Rogue.Types;
    using Rogue.View.Enums;
    using Rogue.View.Interfaces;

    public class LineSceneControl : HandleSceneControl
    {
        public ConsoleColor Color { get; set; }

        public string Texture { get; set; }
        
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

        private (Point from, Point to) linePath;

        public LineSceneControl(Point from, Point to, ConsoleColor color= ConsoleColor.Black, string texture="")
        {
            linePath = (from, to);
            Color = color;
            this.Texture = texture;
        }

        public int Depth { get; set; } = 1;
        
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
                        Fill = true,
                        BackgroundColor = color,
                        Depth = this.Depth,
                        PathPredefined = PathPredefined.Line,
                        Texture = this.Texture,
                        Paths = new System.Collections.Generic.List<Point>()
                        {
                            this.linePath.from,
                            this.linePath.to
                        }
                    };
                }

                return drawablePath;
            }
        }
    }
}
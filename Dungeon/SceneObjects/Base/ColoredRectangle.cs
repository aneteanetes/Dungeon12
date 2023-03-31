namespace Dungeon.Drawing.SceneObjects
{
    using Dungeon.Drawing;
    using Dungeon.Drawing.Impl;
    using Dungeon.GameObjects;
    using Dungeon.SceneObjects;
    using Dungeon.Utils;
    using Dungeon.View.Interfaces;
    using System;

    [Hidden]
    public class ColoredRectangle<TComponent> : SceneControl<TComponent>
        where TComponent : class
    {
        public bool Fill { get; set; }

        private double opacity;
        public new double Opacity
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

        public ColoredRectangle(TComponent component) : base(component)
        {
        }

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
                        Region = this.BoundPosition,
                        Radius = (float)this.Round
                    };
                }

                return drawablePath;
            }
        }

        public ColoredRectangle<TComponent> DarkPanel()
        {
            Color = new DrawColor(ConsoleColor.Black);
            Depth = 1;
            Fill = true;
            Opacity = 0.5;
            Round = 5;

            return this;
        }

    }

    public class DarkRectangle : ColoredRectangle<GameComponentEmpty>
    {
        public DarkRectangle() : base(GameComponentEmpty.Empty)
        {
            Color = new DrawColor(ConsoleColor.Black);
            Depth = 1;
            Fill = true;
            Opacity = .5;
            Round = 5;
        }
    }

    public class BlackRectangle : DarkRectangle
    {
        public BlackRectangle()
        {
            Fill = false;
            Opacity = 1;
        }
    }
}
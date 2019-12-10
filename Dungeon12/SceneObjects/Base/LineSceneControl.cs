namespace Dungeon12.Drawing.SceneObjects
{
    using System;
    using Dungeon.Drawing;
    using Dungeon.Drawing.Impl;
    using Dungeon.GameObjects;
    using Dungeon12.SceneObjects; using Dungeon.SceneObjects;
    using Dungeon.Settings;
    using Dungeon.Types;
    using Dungeon.View.Enums;
    using Dungeon.View.Interfaces;

    public class LineSceneModel : GameComponent
    {
        public Point From { get; set; }

        public Point To { get; set; }

        public ConsoleColor Color { get; set; } = ConsoleColor.Black;

        public string Texture { get; set; } = "";
    }

    public class LineSceneControl : Dungeon12.SceneObjects.HandleSceneControl<LineSceneModel>
    {        
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

        public LineSceneControl(LineSceneModel model) : base(model)
        {
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
                    var color = new DrawColor(Component.Color)
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
                        Texture = Component.Texture,
                        Paths = new System.Collections.Generic.List<Point>()
                        {
                            this.Component.From,
                            this.Component.To
                        }
                    };
                }

                return drawablePath;
            }
        }
    }
}
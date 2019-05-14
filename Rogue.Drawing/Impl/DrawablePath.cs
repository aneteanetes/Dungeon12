namespace Rogue.Drawing.Impl
{
    using System.Collections.Generic;
    using Rogue.Types;
    using Rogue.View.Enums;
    using Rogue.View.Interfaces;

    public class DrawablePath : IDrawablePath
    {
        public float Depth { get; set; } = 1;
        public bool Fill { get; set; }
        public IDrawColor BackgroundColor { get; set; }
        public IDrawColor ForegroundColor { get; set; }
        public Rectangle Region { get; set; }

        public List<Point> Paths = new List<Point>();
        public IEnumerable<Point> Path => Paths;

        public float Angle { get; set; }

        public PathPredefined PathPredefined { get; set; }

        public float Radius { get; set; }
    }
}
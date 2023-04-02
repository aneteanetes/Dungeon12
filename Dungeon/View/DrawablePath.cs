namespace Dungeon.Drawing.Impl
{
    using System;
    using System.Collections.Generic;
    using Dungeon.Types;
    using Dungeon.View.Enums;
    using Dungeon.View.Interfaces;

    public class DrawablePath : IDrawablePath
    {
        public string Uid { get; } = Guid.NewGuid().ToString();

        public double Depth { get; set; } = 1;

        public bool Fill { get; set; }

        public IDrawColor BackgroundColor { get; set; }

        public IDrawColor ForegroundColor { get; set; }

        public Square Region { get; set; }

        public List<Dot> Paths = new List<Dot>();
        public IEnumerable<Dot> Path => Paths;

        public double Angle { get; set; }

        public string Texture { get; set; }

        public PathPredefined PathPredefined { get; set; }

        public float Radius { get; set; }
    }
}
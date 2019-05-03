namespace Rogue.Entites.Animations
{
    using System.Collections.Generic;
    using Rogue.Types;

    public class AnimationMap
    {
        public string TileSet { get; set; }

        public Point Size { get; set; }

        public List<Point> Frames { get; set; }

        public Direction Direction { get; set; }

        public double FramesPerSecond { get; set; }
    }
}
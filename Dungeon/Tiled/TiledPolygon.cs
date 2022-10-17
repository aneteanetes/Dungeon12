using Dungeon.Types;
using System;

namespace Dungeon.Tiled
{
    public class TiledPolygon
    {
        public TiledPolygon(uint gid) => Gid = gid;

        public uint Gid { get; set; }

        public string FileName { get; set; }

        public bool FlippedHorizontally { get; set; }

        public bool FlippedVertically { get; set; }

        public bool FlippedDiagonally { get; set; }

        public int TileOffsetX { get; set; }

        public int TileOffsetY { get; set; }

        public Point Position { get; set; } = Point.Zero;

        public TiledLayer Layer { get; set; }
    }
}

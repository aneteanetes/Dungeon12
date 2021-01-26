using Dungeon.Types;
using Dungeon.View.Interfaces;
using System;

namespace Dungeon.SceneObjects.Tilemaps
{
    public class Tile : ITile
    {
        public Tile()
        {
            Uid = Guid.NewGuid().ToString();
        }

        public int X { get; set; }

        public int Y { get; set; }

        public int Left { get; set; }

        public int Top { get; set; }

        public string Source { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string Uid { get; }

        public Rectangle TileRegion { get; set; }

        public Rectangle TilePosition { get; set; }
    }
}
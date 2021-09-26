using Dungeon.GameObjects;
using Dungeon.View.Enums;
using System.Collections.Generic;

namespace SidusXII.Models.Map
{
    public class MapCellComponent : GameComponent
    {
        public List<object> Objects { get; set; } = new List<object>();

        public List<TileInfo> Tiles { get; set; } = new List<TileInfo>();

        public bool Visible { get; set; }

        public bool Fog { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public bool Spawn { get; set; }
    }

    public class TileInfo
    {
        public string AsmImgTile { get; set; }

        public FlipStrategy Flip { get; set; }
    }
}
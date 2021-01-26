using Dungeon.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon.SceneObjects.Tilemaps
{
    public class TileMap : ITileMap
    {
        public List<ITile> Tiles { get; set; } = new List<ITile>();
    }
}
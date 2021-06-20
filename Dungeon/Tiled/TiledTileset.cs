using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Dungeon.Tiled
{
    [DebuggerDisplay("{name}")]
    public class TiledTileset
    {
        public string name { get; set; }

        public int tilewidth { get; set; }

        public int tileheight { get; set; }

        public int firstgid { get; set; }

        public int tilecount { get; set; }

        public string image { get; set; }

        public int TileIndexFrom => firstgid;

        public int TiledIndexTo => firstgid + tilecount;

        private List<uint> _tileids;
        public List<uint> TileGids
        {
            get
            {
                if (_tileids == null)
                {
                    _tileids = Tiles
                        .Select(x => firstgid + x.Id)
                        .Select(x => Convert.ToUInt32(x))
                        .ToList();
                }

                return _tileids;
            }
        }

        public List<TiledTile> Tiles { get; set; } = new List<TiledTile>();
    }
}
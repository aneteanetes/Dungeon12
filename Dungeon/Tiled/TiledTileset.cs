using Dungeon.Types;
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

        /// <summary>
        /// Тайлы получают свой id по рассчёту ширины, высоты и размера
        /// </summary>
        public bool Autotiled { get; set; }

        public Point Coords(uint gid)
        {
            if (gid== 0)
                return Point.Zero;

            if (Autotiled)
            {
                if (gid % tileheight ==0)
                {
                    // x == 0
                    // y == gid/32
                    return new Point(0, gid/tileheight);
                }
                var y = gid / tileheight;

                var integerGid = y* tileheight;

                var x = gid - integerGid - 1;

                return new Point(x, y);
            }
            return Point.Zero;
        }

        public int TileIndexFrom => firstgid;

        public int TiledIndexTo => firstgid + tilecount;

        private uint[] _tileids;
        public uint[] TileGids
        {
            get
            {
                if (_tileids == null)
                {
                    if (Autotiled)
                    {
                        _tileids = Enumerable.Range(1, 2336)
                            .Select(x => Convert.ToUInt32(x))
                            .ToArray();
                    }
                    else
                    {
                        _tileids = Tiles
                            .Select(x => firstgid + x.Id)
                            .Select(x => Convert.ToUInt32(x))
                            .ToArray();
                    }
                }

                return _tileids;
            }
        }

        public List<TiledTile> Tiles { get; set; } = new List<TiledTile>();
    }
}
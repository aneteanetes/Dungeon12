using Dungeon.Resources;
using Dungeon.Utils.XElementExtensions;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Dungeon.Tiled
{
    public class TiledMap
    {
        const uint FLIPPED_HORIZONTALLY_FLAG = 0x80000000;
        const uint FLIPPED_VERTICALLY_FLAG = 0x40000000;
        const uint FLIPPED_DIAGONALLY_FLAG = 0x20000000;

        private TiledMap() { }

        public static TiledMap Load(string resourceName)
        {
            var res = ResourceLoader.Load(resourceName);
            var xdoc = XDocument.Load(res.Stream);
            var map = xdoc.Root;

            var tiledMap = new TiledMap
            {
                width = map.TagAttrInteger(nameof(width)),
                height = map.TagAttrInteger(nameof(height)),
                tilewidth = map.TagAttrInteger(nameof(tilewidth)),
                tileheight = map.TagAttrInteger(nameof(tileheight)),
            };

            foreach (var xmlTileSet in map.Elements("tileset"))
            {
                var tileSet = new TiledTileset()
                {
                    firstgid = xmlTileSet.TagAttrInteger(nameof(TiledTileset.firstgid)),
                    tilecount = xmlTileSet.TagAttrInteger(nameof(TiledTileset.tilecount)),
                    tilewidth = xmlTileSet.TagAttrInteger(nameof(TiledTileset.tilewidth)),
                    tileheight = xmlTileSet.TagAttrInteger(nameof(TiledTileset.tileheight)),
                    name = xmlTileSet.TagAttrString(nameof(TiledTileset.name)),
                };

                tileSet.Tiles = xmlTileSet
                    .Elements("tile")
                    .Select(x => new TiledTile()
                    {
                        Id = x.TagAttrInteger("id"),
                        File = Path.GetFileName(x.Element("image").TagAttrString("source"))
                    })
                    .ToList();

                tiledMap.Tilesets.Add(tileSet);
            }

            foreach (var xmlLayer in map.Elements("layer"))
            {
                var layer = new TiledLayer()
                {
                    name = xmlLayer.TagAttrString(nameof(TiledLayer.name)),
                    height= xmlLayer.TagAttrInteger(nameof(TiledLayer.height)),
                    width= xmlLayer.TagAttrInteger(nameof(TiledLayer.width))
                };

                var dataTag = xmlLayer.Element("data");

                var gids = new List<uint>();

                var chunks = dataTag.Elements("chunk");
                if (chunks.Count() > 0)
                {
                    chunks.ForEach((chunk =>
                    {
                        var gidsChunk = chunk.Value.Split(",", System.StringSplitOptions.RemoveEmptyEntries).Select(x => uint.Parse(x));
                        gids.AddRange(gidsChunk);
                    }));
                }

                if (gids.Count == 0)
                {
                    gids = dataTag.Value.Split(",", System.StringSplitOptions.RemoveEmptyEntries).Select(x => uint.Parse(x)).ToList();
                }

                foreach (var gidHASHED in gids)
                {
                    // Read out the flags
                    bool flipped_horizontally = (gidHASHED & FLIPPED_HORIZONTALLY_FLAG) != 0;
                    bool flipped_vertically = (gidHASHED & FLIPPED_VERTICALLY_FLAG) != 0;
                    bool flipped_diagonally = (gidHASHED & FLIPPED_DIAGONALLY_FLAG) != 0;

                    var gid = gidHASHED & ~(FLIPPED_HORIZONTALLY_FLAG | FLIPPED_VERTICALLY_FLAG | FLIPPED_DIAGONALLY_FLAG);

                    layer.Tiles.Add(new TiledPolygon()
                    {
                        FileName = tiledMap.TileNameByGid(gid).File,
                        FlippedDiagonally = flipped_diagonally,
                        FlippedHorizontally = flipped_horizontally,
                        FlippedVertically = flipped_vertically
                    });
                }

                tiledMap.Layers.Add(layer);
            }

            return tiledMap;
        }

        public int width { get; set; }

        public int height { get; set; }

        public int tilewidth { get; set; }

        public int tileheight { get; set; }

        public List<TiledTileset> Tilesets { get; set; } = new List<TiledTileset>();

        private TiledTile TileNameByGid(uint gid)
        {
            if (gid == 0)
                return new TiledTile();

            var tileset = this.Tilesets.FirstOrDefault(x => x.TileGids.Contains(gid));
            var tile =  tileset.Tiles.FirstOrDefault(x => x.Id == Math.Abs(gid - tileset.firstgid));

            return tile;
        }

        public List<TiledLayer> Layers { get; set; } = new List<TiledLayer>();
    }
}
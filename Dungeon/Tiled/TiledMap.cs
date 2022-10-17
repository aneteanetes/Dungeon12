using Dungeon.Resources;
using Dungeon.Types;
using Dungeon.Utils.XElementExtensions;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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
                        File = ResourceFile(x.Element("image").TagAttrString("source"))
                    })
                    .ToList();

                if (tileSet.Tiles.Count == 0)
                    tileSet.Autotiled=true;

                tiledMap.Tilesets.Add(tileSet);
            }

            ProcessLayers(map, tiledMap);
            ProcessObjects(map, tiledMap);

            return tiledMap;
        }

        private static string ResourceFile(string file)
        {
            return file.Replace("../", "");
        }

        private static void ProcessObjects(XElement map, TiledMap tiledMap)
        {
            var objProps = typeof(TiledObject)
                .GetProperties()
                .Where(p => Char.IsLower(p.Name[0]))
                .ToArray();

            var tiledObjProps = typeof(TiledObjectProperty)
                .GetProperties()
                .Where(p => Char.IsLower(p.Name[0]))
                .ToArray();

            foreach (var objectlayer in map.Elements("objectgroup"))
            {
                foreach (var objtag in objectlayer.Elements())
                {
                    var tobj = new TiledObject()
                    {
                        objectgroup = objectlayer.Attribute("name").Value
                    };

                    foreach (var prop in objProps)
                    {
                        var attr = objtag.Attribute(prop.Name);
                        if (attr != null)
                        {
                            tobj.SetPropertyExprConverted(prop.Name, attr.Value.Replace(".",","));
                        }
                    }

                    if (tobj.gid != 0)
                    {
                        tobj.file = tiledMap.TileFileNameByGid(tobj.gid);
                    }

                    var props = objtag.Element("properties");
                    if (props != default)
                    {
                        foreach (var xmlprop in props.Elements())
                        {
                            var p = new TiledObjectProperty();
                            foreach (var tiledObjProp in tiledObjProps)
                            {
                                var attr = xmlprop.Attribute(tiledObjProp.Name);
                                if (attr != null)
                                {
                                    p.SetPropertyExprConverted(tiledObjProp.Name, attr.Value);
                                }
                            }
                            tobj.Properties.Add(p);
                        }
                    }

                    var xmlPolygon = objtag.Element("polygon");
                    if(xmlPolygon!=null)
                    {
                        var xmlPoints = xmlPolygon.Attribute("points");
                        if (xmlPoints != null)
                        {
                            var pointsString = xmlPoints.Value;
                            tobj.Polygon = pointsString.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                                .Select(pointstr =>
                                {
                                    var p = pointstr.Split(",", StringSplitOptions.RemoveEmptyEntries);
                                    return new Types.Point(p[0], p[1]);
                                }).ToArray();
                        }
                    }

                    tiledMap.Objects.Add(tobj);
                }
            }
        }

        private static void ProcessLayers(XElement map, TiledMap tiledMap)
        {
            foreach (var xmlLayer in map.Elements("layer"))
            {
                var layer = new TiledLayer()
                {
                    name = xmlLayer.TagAttrString(nameof(TiledLayer.name)),
                    height = xmlLayer.TagAttrInteger(nameof(TiledLayer.height)),
                    width = xmlLayer.TagAttrInteger(nameof(TiledLayer.width))
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
                    gids = dataTag.Value
                        .Split(",", System.StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => uint.Parse(x))
                        .ToList();
                }

                var iX = 0;
                var iY = 0;
                foreach (var gidHASHED in gids)
                {
                    // Read out the flags
                    bool flipped_horizontally = (gidHASHED & FLIPPED_HORIZONTALLY_FLAG) != 0;
                    bool flipped_vertically = (gidHASHED & FLIPPED_VERTICALLY_FLAG) != 0;
                    bool flipped_diagonally = (gidHASHED & FLIPPED_DIAGONALLY_FLAG) != 0;

                    var gid = gidHASHED & ~(FLIPPED_HORIZONTALLY_FLAG | FLIPPED_VERTICALLY_FLAG | FLIPPED_DIAGONALLY_FLAG);

                    var coords = tiledMap.ParseCoordinates(gid);

                    layer.Tiles.Add(new TiledPolygon(gid)
                    {
                        FileName = tiledMap.TileFileNameByGid(gid),
                        FlippedDiagonally = flipped_diagonally,
                        FlippedHorizontally = flipped_horizontally,
                        FlippedVertically = flipped_vertically,
                        Layer = layer,
                        TileOffsetX=coords.Xi,
                        TileOffsetY=coords.Yi,
                        Position = new Point(iX,iY)
                    });

                    iX++;
                    if (iX>layer.width-1)
                    {
                        iX=0;
                        iY++;
                    }

                }

                tiledMap.Layers.Add(layer);
            }
        }

        public int width { get; set; }

        public int height { get; set; }

        public int tilewidth { get; set; }

        public int tileheight { get; set; }

        public List<TiledObject> Objects { get; set; } = new List<TiledObject>();

        public List<TiledTileset> Tilesets { get; set; } = new List<TiledTileset>();

        private Point ParseCoordinates(uint gid)
        {
            var tileset = this.Tilesets.FirstOrDefault(x => x.TileGids.Contains(gid));
            if (tileset != null)
                return tileset.Coords(gid);

            return Point.Zero;
        }

        private string TileFileNameByGid(uint gid)
        {
            if (gid == 0)
                return null;

            var tileset = this.Tilesets.FirstOrDefault(x => x.TileGids.Contains(gid));

            if (tileset.Autotiled)
                return tileset.name;

            return tileset.Tiles
                .FirstOrDefault(x => x.Id == Math.Abs(gid - tileset.firstgid))
                ?.File;
        }

        public List<TiledLayer> Layers { get; set; } = new List<TiledLayer>();
    }
}
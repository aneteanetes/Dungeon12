using Dungeon.Resources;
using Dungeon.Utils.XElementExtensions;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Dungeon.Tiled
{
    public class TiledMap
    {
        private TiledMap() { }

        public static TiledMap Load(string resourceName)
        {
            var res = ResourceLoader.Load(resourceName);
            var xdoc = XDocument.Load(res.Stream);
            var map = xdoc.Root.Element("map");

            var tiledMap = new TiledMap
            {
                width = map.IA(nameof(width)),
                height = map.IA(nameof(height)),
                tilewidth = map.IA(nameof(tilewidth)),
                tileheight = map.IA(nameof(tileheight)),
            };

            foreach (var tileset in map.Elements("tileset"))
            {
                var tiledSet = new TiledTileset()
                {
                    firstgid = tileset.IA(nameof(TiledTileset.firstgid)),
                    tilecount = tileset.IA(nameof(TiledTileset.tilecount)),
                    tilewidth = tileset.IA(nameof(TiledTileset.tilewidth)),
                    tiledheight = tileset.IA(nameof(TiledTileset.tiledheight)),
                };
            }

            return default;
        }

        public int width { get; set; }

        public int height { get; set; }

        public int tilewidth { get; set; }

        public int tileheight { get; set; }

        public List<TiledTileset> Tilesets { get; set; } = new List<TiledTileset>();
    }
}
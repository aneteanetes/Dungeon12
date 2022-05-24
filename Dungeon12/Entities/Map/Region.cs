using Dungeon;
using Dungeon.Physics;
using Dungeon.Resources;
using Dungeon.Tiled;
using Dungeon.Types;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.Entities.Map
{
    internal class Region
    {
        public List<Location> Locations { get; set; }

        public bool Indoor { get; set; }

        public string DefaultLocationTile { get; set; }

        public List<Point> Lines { get; set; } = new List<Point>();

        public List<PhysicalObject> Objects { get; set; } = new List<PhysicalObject>();

        public Point Size { get; set; }

        public Point Position { get; set; }

        public Point PositionVisual { get; set; }

        public string Title { get; set; }

        public string MapId { get; set; }

        public static Region Load(string id) //ShipFaithIsland
        {
            var region = ResourceLoader.LoadJson<Region>($"Regions/{id}.json".AsmRes());
            var tiled = TiledMap.Load($"Maps/{region.MapId}.tmx".AsmRes());            

            region.Locations = tiled.Objects
                .GroupBy(t => new { t.x, t.y })
                .Select(g => CreateLocation(g, region))
                .Where(x => x != null)
                .ToList();

            foreach (var location in region.Locations)
            {
                var linksIds = location.IndexLinks
                    .ToArray();

                location.Region = region;

                location.Links = region.Locations
                    .Where(x => linksIds.Contains(x.Index))
                    .ToList();
            }

            return region;
        }

        private static Location CreateLocation(IEnumerable<TiledObject> tiles, Region region)
        {
            var line = tiles.FirstOrDefault(t => t.objectgroup == "Lines");
            if (line != default)
            {
                region.Lines.Add(SetPosition(line));
                return null;
            }

            var tile = tiles.FirstOrDefault(t => t.objectgroup == "Cells");

            var location = new Location
            {
                Size = new Point(tile.width, tile.height),

                Index = tile.GetPropValue<int>("index"),
                IndexLinks = tile.GetPropValue<string>("IndexLinks")
                    ?.Split(",", System.StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => int.Parse(x))
                    .ToArray() ?? new int[0],

                ObjectId = tile?.GetPropValue<string>("objectid"),

                BackgroundImage = tile.file,

                IsOpen = tile.GetPropValue<bool>("isopen")
            };

            location.Position = SetPosition(tile);

            if (location.ObjectId != null)
                try
                {
                    location.Polygon = ResourceLoader.LoadJson<Polygon>($"Objects/{location.ObjectId}.json".AsmRes(), @throw: false);
                    if (location.Polygon == null)
                        location.Polygon = new Polygon(); // это пока не все объекты пока разработка
                    location.Polygon.Init();
                }
                catch { }

            return location;
        }

        private static Point SetPosition(TiledObject tile)
        {
            var position = new Point(tile.x, (tile.y / 148) - 1);

            if (position.Y % 2 == 0)
            {
                //чётный

                if (position.X < 0)
                    position.X = 0;
                else if (position.X == 83)
                    position.X = 1;
                else
                {
                    position.X = (position.X - 83) / 166;
                    position.X += 1;
                }
            }
            else
            {
                //не чётный

                if (position.X != 0)
                {
                    position.X /= 166;
                }

            }

            position.X *= 96;
            if (!position.IsEvenY)
            {
                position.X += 48;
            }

            position.Y *= 86;

            return position;
        }
    }
}
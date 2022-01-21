using Dungeon;
using Dungeon.Physics;
using Dungeon.Resources;
using Dungeon.Tiled;
using Dungeon.Types;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon12.Entities.Map
{
    public class Region
    {
        public List<Location> Locations { get; set; }

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

            var area = tiled.Objects.FirstOrDefault(x => x.Properties.FirstOrDefault(p => p.name == "Area") != default);

            region.Size = new Point(area.width, area.height);
            region.Position = new Point(area.x, area.y);

            region.Locations = tiled.Objects
                .GroupBy(t => new { t.x, t.y })
                .Select(g => CreateLocation(g, area, region))
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

        private static Location CreateLocation(IEnumerable<TiledObject> tiles, TiledObject area, Region region)
        {
            var service = tiles.FirstOrDefault(x => x.objectgroup == "Service");
            if (service != null)
                return null;

            var uiObject = tiles.FirstOrDefault(x => x.objectgroup == "UI");
            if (uiObject != default)
            {
                region.Objects.Add(new PhysicalObject()
                {
                    Image = uiObject.file,
                    Size = new PhysicalSize(uiObject.width, uiObject.height),
                    Position = new PhysicalPosition(uiObject.x - area.x, (uiObject.y - uiObject.height) - area.y)
                });
                return null;
            }

            var line = tiles.FirstOrDefault(t => t.objectgroup == "Lines");
            if (line != default)
            {
                region.Lines.Add(new Point(line.x - area.x, (line.y - line.height) - area.y));
                return null;
            }

            var tile = tiles.FirstOrDefault(t => t.objectgroup == "Cells");
            var tileObj = tiles.FirstOrDefault(t => t.objectgroup == "Objects");

            var location = new Location
            {
                Size = new Point(tile.width, tile.height),
                Position = new Point(tile.x - area.x, (tile.y - tile.height) - area.y),

                Index = tile.GetPropValue<int>("index"),
                IndexLinks = tile.GetPropValue<string>("IndexLinks")
                    ?.Split(",", System.StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => int.Parse(x))
                    .ToArray() ?? new int[0],

                ObjectId = tileObj.GetPropValue<string>("objectid"),

                BackgroundImage = tile.file,
                ObjectImage = tileObj.file,

                IsOpen = tile.GetPropValue<bool>("isopen")
            };

            if (location.ObjectId != null)
                try
                {
                    location.Polygon = ResourceLoader.LoadJson<Polygon>($"Objects/{location.ObjectId}.json".AsmRes(),@throw:false);
                    if (location.Polygon == null)
                        location.Polygon = new Polygon(); // это пока не все объекты пока разработка
                    location.Polygon.Init();
                }
                catch { }

            return location;
        }
    }
}
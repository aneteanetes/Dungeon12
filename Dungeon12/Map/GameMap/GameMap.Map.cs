namespace Dungeon12.Map
{
    using Dungeon.Data;
    using Dungeon12.Data.Region;
    using Dungeon12.Map.Objects;
    using Dungeon.Physics;
    using Dungeon.Resources;
    using Dungeon.Types;
    using Force.DeepCloner;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;
    using static Dungeon12.Global;
    using Dungeon;

    public partial class GameMap
    {
        public IEnumerable<PhysicalObject> SafeZones { get; set; } = new List<PhysicalObject>();

        public bool InSafe(MapObject @object) => SafeZones.Any(safeZone => safeZone.IntersectsWith(@object));

        public string MapIdentifyId { get; private set; }

        public bool IsUnderLevel { get; set; }

        public string InitRegion(string name)
        {
            Global.GameState.Map = this;
            MapIdentifyId = name;

            var persistRegion = Dungeon.Store.Entity<Region>(e => e.Name == name).First();
            LoadTexturePositions(name);

            this.IsUnderLevel = persistRegion.IsUnderLevel;
            if (!IsUnderLevel)
            {
                Global.GameState.Region = this;
            }

            this.SafeZones = persistRegion.SafeZones.Select(safeZone => safeZone * 32);

            foreach (var regionObject in persistRegion.Objects)
            {
                if(regionObject.Obstruct && persistRegion.Offset!=default)
                {
                    regionObject.Position.X += persistRegion.Offset.X;
                    regionObject.Position.Y += persistRegion.Offset.Y;
                }

                var obj = Map.MapObject.Create(regionObject);
                obj.Destroy += () => { this.MapObject.Remove(obj); };
                this.MapObject.Add(obj);

                if (!(obj is Wall))
                {
                    this.Objects.Add(obj);
                }
            }

            this.Name = persistRegion.Display;
            this.LoadedRegionData = persistRegion;

            return persistRegion.Name;
        }

        public Region LoadedRegionData { get; set; }

        /// <summary>
        /// Расположение текстур
        /// </summary>
        public List<PhysicalObject> Textures { get; set; } = new List<PhysicalObject>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapSaveModel"></param>
        /// <param name="loadLocal">загрузить карту в объект при этом не присваивая её текущей карте</param>
        /// <returns></returns>
        public string LoadRegion(MapSaveModel mapSaveModel, bool loadLocal=false)
        {
            MapIdentifyId = mapSaveModel.Name;
            Loaded = true;

            if (!loadLocal)
            {
                if (Global.GameState.Map != this)
                {
                    Global.GameState.Map = this;
                }
            }

            var persistRegion = Dungeon.Store.Entity<Region>(e => e.Name == mapSaveModel.Name).First();
            LoadTexturePositions(MapIdentifyId);

            this.SafeZones = persistRegion.SafeZones.Select(safeZone => safeZone * 32);

            foreach (var regionObject in persistRegion.Objects)
            {
                if (regionObject.Obstruct && persistRegion.Offset != default)
                {
                    regionObject.Position.X += persistRegion.Offset.X;
                    regionObject.Position.Y += persistRegion.Offset.Y;
                }
                var obj = Map.MapObject.Create(regionObject,false);
                if (obj != default)
                {
                    obj.Destroy += () => { this.MapObject.Remove(obj); };
                    this.MapObject.Add(obj);

                    if (!(obj is Wall))
                    {
                        this.Objects.Add(obj);
                    }
                }
            }

            this.Name = persistRegion.Display;
            this.LoadedRegionData = persistRegion;

            foreach (var saveableObject in mapSaveModel.Objects)
            {
                saveableObject.Reload();
                this.MapObject.Add(saveableObject);
                this.Objects.Add(saveableObject);
            }

            return persistRegion.Name;
        }

        private void LoadTexturePositions(string identify)
        {
            var res = $"{Global.GameAssemblyName}/Resources/Data/Regions/{identify}Textures.json".Embedded();
            if (ResourceLoader.Exists(res))
            {
                var texturesData = ResourceLoader.Load(res).Stream.AsString();
                this.Textures = JsonConvert.DeserializeObject<PhysicalObjectProjection[]>(texturesData).Select(x =>
                {
                    var po = x.PhysicalObject;
                    po.Position = new PhysicalPosition()
                    {
                        X = po.Position.X * 32,
                        Y = po.Position.Y * 32
                    };
                    return po;
                }).ToList();
            }
        }

        public Point RandomizeLocation(Point point, RandomizePositionTry @try =null, int count=0)
        {
            if(count>100)
            {
                return @try.Existed.FirstOrDefault();
            }

            if (@try == null)
            {
                @try = new RandomizePositionTry();
            }

            point.X += RandomizePosition(@try);
            point.Y += RandomizePosition(@try);

            if (MapObject.Exists(new MapObject()
            {
                Location = point,
                Size = new PhysicalSize() { Height = 16, Width = 16 }
            }))
            {
                @try.Existed.Add(point.DeepClone());
                point = RandomizeLocation(point, @try,++count);
            }

            return point;
        }

        private double RandomizePosition(RandomizePositionTry @try = null)
        {
            var dir = RandomDungeon.Next(0, 2) == 0 ? 1 : -1;
            var offset = RandomDungeon.Next(0, 3);

            if (offset == 1)
                return 0;

            var awayRange = 0.01 * @try.TryCount;

            var val = offset * awayRange * dir;

            return val;
        }

        public class RandomizePositionTry
        {
            public List<Point> Existed { get; set; } = new List<Point>();

            public int TryCount => Existed.Count == 0
                ? 1
                : ((Existed.Count - 1) % 8) + 1;
        }
    }
}

namespace Dungeon.Map
{
    using Dungeon.Data;
    using Dungeon.Data.Region;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.Entities.Enemy;
    using Dungeon.Loot;
    using Dungeon.Map.Objects;
    using Dungeon.Physics;
    using Dungeon.Scenes.Manager;
    using Dungeon.Types;
    using Dungeon.View.Interfaces;
    using Force.DeepCloner;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class GameMap
    {
        public IEnumerable<PhysicalObject> SafeZones { get; set; } = new List<PhysicalObject>();

        public bool InSafe(MapObject @object) => SafeZones.Any(safeZone => safeZone.IntersectsWith(@object));

        public string MapIdentifyId { get; private set; }

        public string InitRegion(string name)
        {
            Global.GameState.Map = this;
            MapIdentifyId = name;

            var persistRegion = Database.Entity<Region>(e => e.Name == name).First();

            this.SafeZones = persistRegion.SafeZones.Select(safeZone => safeZone * 32);

            foreach (var regionObject in persistRegion.Objects)
            {
                var obj = Map.MapObject.Create(regionObject);
                obj.Destroy += () => { this.MapObject.Remove(obj); };
                this.MapObject.Add(obj);

                if (!(obj is Empty) && !(obj is Wall))
                {
                    this.Objects.Add(obj);
                }
            }

            this.Name = persistRegion.Display;
            this.LoadedRegionData = persistRegion;

            return persistRegion.Name;
        }

        public Region LoadedRegionData { get; set; }

        public string LoadRegion(MapSaveModel mapSaveModel)
        {
            MapIdentifyId = mapSaveModel.Name;
            Loaded = true;

            if (Global.GameState.Map != this)
            {
                Global.GameState.Map = this;
            }

            var persistRegion = Database.Entity<Region>(e => e.Name == mapSaveModel.Name).First();

            this.SafeZones = persistRegion.SafeZones.Select(safeZone => safeZone * 32);

            foreach (var regionObject in persistRegion.Objects)
            {
                var obj = Map.MapObject.Create(regionObject,false);
                if (obj != default)
                {
                    obj.Destroy += () => { this.MapObject.Remove(obj); };
                    this.MapObject.Add(obj);

                    if (!(obj is Empty) && !(obj is Wall))
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

        public class MobData : Dungeon.Data.Persist
        {
            public int Level { get; set; }

            public string Name { get; set; }

            public string Tileset { get; set; }

            public Point Size { get; set; }

            public Point Position { get; set; }

            public Rectangle TileSetRegion { get; set; }

            public Enemy Enemy { get; set; }

            public double MovementSpeed { get; set; }

            public Point VisionMultiples { get; set; }

            public Point AttackRangeMultiples { get; set; }
        }

        private void SpawnEnemies(int count)
        {
            var data = Database.Entity<MobData>(x => x.Level == this.Level)
                .FirstOrDefault();

            for (int i = 0; i < count; i++)
            {
                var mob = new Mob(data.Enemy.DeepClone())
                {
                    Tileset = data.Tileset,
                    TileSetRegion = data.TileSetRegion,
                    Name = data.Name,
                    Size = new PhysicalSize()
                    {
                        Width = data.Size.X * 32,
                        Height = data.Size.Y * 32
                    },
                    MovementSpeed = data.MovementSpeed,
                    VisionMultiple = data.VisionMultiples,
                    AttackRangeMultiples = data.AttackRangeMultiples
                };

                mob.Entity.Name = mob.Name;

                bool setted = false;

                while (!(setted = TrySetLocation(mob))) ;

                if (setted)
                {
                    this.MapObject.Add(mob);
                    this.Objects.Add(mob);
                }
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

        private bool TrySetLocation(Mob mob)
        {
            var x = Dungeon.RandomDungeon.Next(20, 70);
            var y = Dungeon.RandomDungeon.Next(20, 70);

            mob.Location = new Point(x, y);

            var otherObject = this.MapObject.Query(mob).Nodes.Any(node => node.IntersectsWith(mob));
            if (otherObject)
                return false;

            if (InSafe(mob))
            {
                return false;
            }
            
            return true;
        }
    }
}

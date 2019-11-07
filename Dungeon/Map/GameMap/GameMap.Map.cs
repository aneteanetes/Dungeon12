namespace Dungeon.Map
{
    using Dungeon.Data;
    using Dungeon.Data.Region;
    using Dungeon.Entities.Enemy;
    using Dungeon.Loot;
    using Dungeon.Map.Objects;
    using Dungeon.Physics;
    using Dungeon.Types;
    using Force.DeepCloner;
    using System.Collections.Generic;
    using System.Linq;

    public partial class GameMap
    {
        private IEnumerable<PhysicalObject> SafeZones = new List<PhysicalObject>();

        public bool InSafe(MapObject @object) => SafeZones.Any(safeZone => safeZone.IntersectsWith(@object));

        public string LoadRegion(string name)
        {
            var persistRegion = Database.Entity<Region>(e => e.Name == name).First();

            this.SafeZones = persistRegion.SafeZones.Select(safeZone => safeZone * 32);

            foreach (var regionObject in persistRegion.Objects)
            {
                var obj = MapObject.Create(regionObject);
                obj.Destroy += () => { this.Map.Remove(obj); };
                this.Map.Add(obj);

                if (!(obj is Empty) && !(obj is Wall))
                {
                    this.Objects.Add(obj);
                }
            }

            SpawnEnemies(50);

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
                var mob = new Mob()
                {
                    Enemy = data.Enemy.DeepClone(),
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

                mob.Enemy.Name = mob.Name;

                bool setted = false;

                while (!(setted = TrySetLocation(mob))) ;

                if (setted)
                {
                    mob.Die += () =>
                    {
                        List<MapObject> publishObjects = new List<MapObject>();

                        var loot = LootGenerator.Generate();

                        if (loot.Gold > 0)
                        {
                            var money = new Money() { Amount = loot.Gold };
                            money.Location = RandomizeLocation(mob.Location.DeepClone());
                            money.Destroy += () => Map.Remove(money);
                            Map.Add(money);

                            publishObjects.Add(money);
                        }

                        foreach (var item in loot.Items)
                        {
                            var lootItem = new Loot()
                            {
                                Item = item
                            };

                            lootItem.Location = RandomizeLocation(mob.Location.DeepClone());
                            lootItem.Destroy += () => Map.Remove(lootItem);

                            Map.Add(lootItem);
                            publishObjects.Add(lootItem);
                        }

                        this.Map.Remove(mob);

                        publishObjects.ForEach(this.PublishObject);
                    };

                    this.Map.Add(mob);
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

            if (Map.Exists(new MapObject()
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
            var x = Dungeon.RandomDungeon.Next(20, 80);
            var y = Dungeon.RandomDungeon.Next(20, 80);

            mob.Location = new Point(x, y);

            var otherObject = this.Map.Query(mob).Nodes.Any(node => node.IntersectsWith(mob));
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

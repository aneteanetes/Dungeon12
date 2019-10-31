namespace Dungeon.Map
{
    using Dungeon.Data;
    using Dungeon.Data.Homes;
    using Dungeon.Data.Mobs;
    using Dungeon.Data.Npcs;
    using Dungeon.Data.Region;
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
                this.Map.Add(obj);
            }

            SpawnEnemies(20);

            return persistRegion.Name;
        }

        public void LoadMerchant(MapObject mapObject)
        {
            mapObject.Merchant = new Merchants.Merchant();
            mapObject.Merchant.FillBackpacks();
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
                    MovementSpeed=data.MovementSpeed,
                    VisionMultiple=data.VisionMultiples,
                    AttackRangeMultiples=data.AttackRangeMultiples
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
                                Item=item
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

        public Point RandomizeLocation(Point point, RandomizePositionTry @try =null)
        {
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
                point = RandomizeLocation(point, @try);
            }

            return point;
        }

        private double RandomizePosition(RandomizePositionTry @try = null)
        {
            var dir = RandomRogue.Next(0, 2) == 0 ? 1 : -1;
            var offset = RandomRogue.Next(0, 3);

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
            var x = Dungeon.RandomRogue.Next(20, 80);
            var y = Dungeon.RandomRogue.Next(20, 80);

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

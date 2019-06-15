namespace Rogue.Map
{
    using Force.DeepCloner;
    using Rogue.Data.Conversations;
    using Rogue.Data.Homes;
    using Rogue.Data.Mobs;
    using Rogue.Data.Npcs;
    using Rogue.Data.Region;
    using Rogue.DataAccess;
    using Rogue.Loot;
    using Rogue.Map.Objects;
    using Rogue.Physics;
    using Rogue.Settings;
    using Rogue.Types;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public partial class GameMap
    {
        private IEnumerable<PhysicalObject> SafeZones = new List<PhysicalObject>();

        public bool InSafe(MapObject @object) => SafeZones.Any(safeZone => safeZone.IntersectsWith(@object));

        public string LoadRegion(string name)
        {
            var persistRegion = Database.Entity<Region>(e => e.Name == name).First();

            this.SafeZones = persistRegion.SafeZones.Select(safeZone => safeZone * 32);

            foreach (var item in persistRegion.Objects)
            {
                if (item.Obstruct)
                {
                    var wall = MapObject.Create("~");
                    wall.Location = new Point(item.Position.X, item.Position.Y);

                    if (item.Region == null)
                    {
                        var measure = Global.DrawClient.MeasureImage(item.Image);
                        wall.Size = new PhysicalSize
                        {
                            Width=measure.X,
                            Height=measure.Y
                        };
                    }
                    this.Map.Add(wall);
                }
            }

            foreach (var npc in persistRegion.NPCs)
            {
                var data = Database.Entity<NPCData>(x => x.IdentifyName == npc.IdentifyName).FirstOrDefault();

                var mapNpc = new NPC()
                {
                    NPCEntity = data.NPC.DeepClone(),
                    Tileset = data.Tileset,
                    TileSetRegion = data.TileSetRegion,
                    Name = data.Name,
                    Size = new PhysicalSize()
                    {
                        Width = data.Size.X * 32,
                        Height = data.Size.Y * 32
                    },
                    MovementSpeed = data.MovementSpeed,
                    Location = npc.Position
                };

                BindConversations(data, mapNpc);

                mapNpc.NPCEntity.MoveRegion = mapNpc.NPCEntity.MoveRegion * 32;

                mapNpc.Die += () =>
                {
                    this.Map.Remove(mapNpc);
                };

                this.Map.Add(mapNpc);
                this.Objects.Add(mapNpc);
            }

            foreach (var home in persistRegion.Homes)
            {
                var data = Database.Entity<HomeData>(x => x.IdentifyName == home.IdentifyName).FirstOrDefault();

                var mapHome = new Home()
                {
                     ScreenImage = data.ScreenImage,
                     Frames=data.Frames,
                    Name = data.Name,
                    Size = new PhysicalSize()
                    {
                        Width = 32,
                        Height = 32
                    },
                    Location = home.Position
                };

                BindConversations(data, mapHome);

                this.Map.Add(mapHome);
                this.Objects.Add(mapHome);
            }

            SpawnEnemies(20);

            return persistRegion.Name;
        }

        public void Load(string identity)
        {
            var persistMap = Database.Entity<Data.Maps.Map>(e => e.Identity == identity).First();

            this.Name = persistMap.Name;

            int x = 0;
            int y = 0;

            var template = persistMap.Template.Trim();

            foreach (var line in template.Replace("\r","").Split('\n'))
            {
                var listLine = new List<List<Map.MapObject>>();

                x = 0;

                foreach (var @char in line)
                {
                    var mapObj = MapObject.Create(@char.ToString());
                    mapObj.Location = new Point(x, y);
                    mapObj.Region = new Rectangle
                    {
                        Height = 32,
                        Width = 32,
                        Pos = mapObj.Location
                    };

                    if (mapObj.Obstruction)
                    {
                        this.Map.Add(mapObj);
                    }

                    listLine.Add(new List<MapObject>() { mapObj });
                    x++;
                }

                y++;

                this.MapOld.Add(listLine);
            }
            
            foreach (var obj in persistMap.Mobs)
            {
                var mob = new Mob()
                {
                    Enemy = obj.Enemy,
                    Size = new PhysicalSize()
                    {
                        Height = obj.Size.X * 32,
                        Width = obj.Size.Y * 32
                    },
                    Location = obj.Position,
                    Tileset = obj.Tileset,
                    TileSetRegion = obj.TileSetRegion,
                };

                mob.Die += () =>
                {
                    this.Map.Remove(mob);
                };

                this.Map.Add(mob);
                this.Objects.Add(mob);
            }

            SpawnEnemies(2);
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

        private Point RandomizeLocation(Point point)
        {
            point.X += RandomizePosition();
            point.Y += RandomizePosition();

            return point;
        }

        private double RandomizePosition()
        {
            var dir = RandomRogue.Next(0, 2) == 0 ? 1 : -1;
            var offset = RandomRogue.Next(0, 3);

            if (offset == 1)
                return 0;

            var val = offset * 0.2 * dir;

            return val;
        }

        private bool TrySetLocation(Mob mob)
        {
            var x = Rogue.RandomRogue.Next(20, 80);
            var y = Rogue.RandomRogue.Next(20, 80);

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

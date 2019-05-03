using Force.DeepCloner;
using Rogue.Data.Mobs;
using Rogue.DataAccess;
using Rogue.Entites.Enemy;
using Rogue.Map.Objects;
using Rogue.Physics;
using Rogue.Settings;
using Rogue.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Rogue.Map
{
    public partial class GameMap
    {
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
                        var nodes = this.Map.Query(mapObj, true);

                        foreach (var node in nodes)
                        {
                            node.Nodes.Add(mapObj);
                        }
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

                var nodes = this.Map.Query(mob, true);

                foreach (var node in nodes)
                {
                    node.Nodes.Add(mob);
                }

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
                    //Name = data.Name,
                    Size = new PhysicalSize() {
                        Width = data.Size.X * 32,
                        Height = data.Size.Y * 32
                    }
                };


                bool setted = false;
                for (int j = 0; j < 100; j++)
                {
                    if (setted = TrySet(mob))
                    {
                        break;
                    }
                }

                if (setted)
                {
                    mob.Die += () =>
                    {
                        this.Map.Remove(mob);
                    };

                    var nodes = this.Map.Query(mob, true);

                    foreach (var node in nodes)
                    {
                        node.Nodes.Add(mob);
                    }

                    this.Objects.Add(mob);
                }
            }
        }

        private bool TrySet(Mob mob)
        {
            var x = Random.Next(3, 32);
            var y = Random.Next(3, 18);

            mob.Location = new Point(x, y);

            return !this.Map.Query(mob).Nodes.Any(node => node.Location.X == x && node.Location.Y == y);
        }
    }
}

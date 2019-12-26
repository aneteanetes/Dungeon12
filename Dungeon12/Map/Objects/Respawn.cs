using Dungeon;
using Dungeon.Data.Attributes;
using Dungeon12.Data.Region;
using Dungeon12.Map;
using Dungeon12.Map.Infrastructure;
using Dungeon.Physics;
using Dungeon.Types;
using Dungeon12.Database.Respawn;
using System.Diagnostics;
using System.Linq;
using Dungeon.View.Interfaces;
using Dungeon12.SceneObjects.Map;
using System.Collections.Generic;
using System;
using Dungeon12.Map.Events;

namespace Dungeon12.Map.Objects
{
    [Template("Respawn")]
    [DataClass(typeof(RespawnData))]
    public class Respawn : MapObject
    {
        public PhysicalObject SpawnArea { get; set; }

        /// <summary>
        /// Грязнущий хак:
        /// <para>
        /// Респаун сохраняется, а значит не будет делать новые объекты
        /// При этом старые объекты сохраняются
        /// Но когда мы переходим в другой регион респаун спадает потому что мы сохраняем только текущий регион
        /// </para>
        /// </summary>
        public override bool Saveable => true;

        public int RespawnSeconds { get; set; }

        public List<string> MobsAlive { get; set; } = new List<string>();

        public RespawnPointData[] PointData { get; set; }

        private string id { get; set; }

        protected override void Load(RegionPart regionPart)
        {
            var data = regionPart.As<RespawnData>();

            if (data.VariableCondition != default)
            {
                if (Global.GameState.Character[data.VariableCondition] == default)
                {
                    return;
                }
            }

            this.RespawnSeconds = data.RespawnSeconds;

            SpawnArea = data.Zone;
            PointData = data.Respawns;

            Global.Events.Subscribe<GameMapLoadedEvent>(MapLoaded);
        }

        private void  MapLoaded(GameMapLoadedEvent @event)
        {
            Spawn();
        }

        public void Spawn()
        {
            foreach (var spawnData in PointData)
            {
                for (int i = 0; i < spawnData.Amount; i++)
                {
                    var mapObject = Create(new RegionPart()
                    {
                        Icon = spawnData.Icon,
                        IdentifyName = spawnData.Identify
                    });
                    mapObject.Uid = this.Uid + i;
                    BindDestory(mapObject);
                    SetLocation(100, mapObject);
                }
            }
        }

        public override void Reload(HashSet<MapObject> objects)
        {
            objects.Where(x => x.Uid.Contains(this.Uid) && x != this).ForEach(BindDestory);
            StartReespawnTimer();
        }

        private void BindDestory(MapObject mapObject)
        {
            mapObject.Destroy += () =>
            {
                MobsAlive.Remove(mapObject.Uid);
                StartReespawnTimer();
            };
        }

        private void StartReespawnTimer()
        {
            if (this.MobsAlive.Count == 0 && RespawnSeconds > 0)
            {
                Global.Time.Timer()
                .After(TimeSpan.FromSeconds(RespawnSeconds).TotalMilliseconds)
                .Do(() => Spawn())
                .Trigger();
            }
        }

        private void SetLocation(int tries, MapObject mob)
        {
            for (int i = 0; i < tries; i++)
            {
                if (TrySetLocation(mob))
                {
                    MobsAlive.Add(mob.Uid);
                    break;
                }
            }
        }

        private bool TrySetLocation(MapObject mob)
        {
            var pos = SpawnArea.Position;
            var posMax = SpawnArea.MaxPosition;

            var x = RandomDungeon.Range(pos.X, posMax.X);
            var y = RandomDungeon.Range(pos.Y, posMax.Y);

            mob.Location = new Point(x, y);

            var map = Global.GameState.Map;

            var otherObject = map.MapObject.Query(mob).Nodes.Any(node => node.IntersectsWithOrContains(mob));
            if (otherObject)
                return false;

            if (InSafe(mob))
            {
                return false;
            }

            bool underTexture = false;

            // объект попадает на "пол" - какую либо текстуру
            if (Global.GameState.Map.Textures.Any(t => t.IntersectsWithOrContains(mob)))
            {
                if(map.MapObject.Query(mob).Nodes.Any(node => node.IntersectsWithOrContains(mob)))
                {
                    Debugger.Break();
                }

                underTexture = true;
            }

            if (!underTexture)
                return false;

            otherObject = map.MapObject.Query(mob).Nodes.Any(node => node.IntersectsWithOrContains(mob));
            if (otherObject)
                return false;

            map.MapObject.Add(mob);
            map.Objects.Add(mob);
            map.PublishObject?.Invoke(mob);

            return true;
        }

        public bool InSafe(MapObject @object) => Global.GameState.Map.SafeZones.Any(safeZone => safeZone.IntersectsWith(@object));
    }
}
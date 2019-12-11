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

            SpawnArea = data.Zone;

            foreach (var spawnData in data.Respawns)
            {
                for (int i = 0; i < spawnData.Amount; i++)
                {
                    var mapObject = Create(new RegionPart()
                    {
                        Icon = spawnData.Icon,
                        IdentifyName = spawnData.Identify
                    });
                    SetLocation(20, mapObject);
                }
            }
        }

        private void SetLocation(int tries, MapObject mob)
        {
            for (int i = 0; i < tries; i++)
            {
                if (TrySetLocation(mob))
                    break;
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

            map.MapObject.Add(mob);
            map.Objects.Add(mob);
            map.PublishObject?.Invoke(mob);

            return true;
        }

        public bool InSafe(MapObject @object) => Global.GameState.Map.SafeZones.Any(safeZone => safeZone.IntersectsWith(@object));
    }
}
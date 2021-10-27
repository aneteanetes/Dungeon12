using Dungeon12.Map;
using Dungeon12.Map.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon12
{
    public static class GamemapExtensions
    {
        public static IEnumerable<NPCMap> Enemies(this GameMap gamemap, MapObject @object)
        {
            IEnumerable<NPCMap> mobs = Enumerable.Empty<NPCMap>();

            var moveAreas = gamemap.MapObject.QueryContainer(@object).SelectMany(x => x.Nodes);

            mobs = moveAreas.Where(node =>
            {
                if (node is NPCMap nPCMap)
                {
                    return nPCMap.IsEnemy;
                }
                return false;
            }).Select(node => node as NPCMap)
                .Where(node => @object.IntersectsWith(node))
                .ToArray();

            return mobs.ToArray();
        }
    }
}

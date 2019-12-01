using Dungeon.Map;
using Dungeon12.Map.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dungeon
{
    public static class GamemapExtensions
    {
        public static IEnumerable<NPCMap> Enemies(this GameMap gamemap, MapObject @object)
        {
            IEnumerable<NPCMap> mobs = Enumerable.Empty<NPCMap>();

            var moveArea = gamemap.MapObject.Query(@object);
            if (moveArea != null)
            {
                mobs = moveArea.Nodes.Where(node =>
                {
                    if (node is NPCMap nPCMap)
                    {
                        return nPCMap.IsEnemy;
                    }
                    return false;
                }).Select(node => node as NPCMap)
                    .Where(node => @object.IntersectsWith(node))
                    .ToArray();
            }

            return mobs.ToArray();
        }
    }
}

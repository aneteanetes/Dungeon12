using Dungeon.Engine.Projects;
using Dungeon.Events;

namespace Dungeon.Engine.Events
{
    public class ResourceAddEvent : IEvent
    {
        public DungeonEngineResourcesGraph Resource { get; set; }

        public DungeonEngineResourcesGraph ParentResource { get; set; }

        public ResourceAddEvent(DungeonEngineResourcesGraph res, DungeonEngineResourcesGraph parent)
        {
            Resource = res;
            ParentResource = parent;
        }
    }
}

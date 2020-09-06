using Dungeon.Engine.Projects;
using Dungeon.Events;

namespace Dungeon.Engine.Events
{
    public class ResourceAddEvent : IEvent
    {
        public string ResourceFilePath { get; set; }

        public DungeonEngineResourcesGraph ParentResource { get; set; }

        public ResourceAddEvent(string res, DungeonEngineResourcesGraph parent)
        {
            ResourceFilePath = res;
            ParentResource = parent;
        }
    }
}

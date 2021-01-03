using Dungeon.Engine.Projects;
using Dungeon.Events;

namespace Dungeon.Engine.Events
{
    public class ResourceAddEvent : IEvent
    {
        public string ResourceFilePath { get; set; }

        public ResourcesGraph ParentResource { get; set; }

        public ResourceAddEvent(string res, ResourcesGraph parent)
        {
            ResourceFilePath = res;
            ParentResource = parent;
        }
    }
}

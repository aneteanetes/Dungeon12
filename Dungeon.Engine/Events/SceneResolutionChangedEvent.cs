using Dungeon.Events;
using Dungeon.Types;

namespace Dungeon.Engine.Events
{
    public class SceneResolutionChangedEvent : IEvent
    {
        public Point Size { get; set; }

        public SceneResolutionChangedEvent(int width, int height) => Size = new Point(width, height);
    }
}
using Dungeon.Events;
using Dungeon.Types;

namespace Dungeon.Engine.Events
{
    public class SceneResolutionChangedEvent : IEvent
    {
        public Dot Size { get; set; }

        public SceneResolutionChangedEvent(int width, int height) => Size = new Dot(width, height);
    }
}
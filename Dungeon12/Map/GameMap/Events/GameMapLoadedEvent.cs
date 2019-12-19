using Dungeon.Events;

namespace Dungeon12.Map.Events
{
    public class GameMapLoadedEvent : IEvent
    {
        public GameMap GameMap { get; set; }
    }
}

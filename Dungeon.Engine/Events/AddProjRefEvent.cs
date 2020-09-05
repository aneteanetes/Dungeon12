using Dungeon.Engine.Editable;
using Dungeon.Events;

namespace Dungeon.Engine.Events
{
    public class AddProjRefEvent : IEvent
    {
        public DungeonEngineReference Reference { get; set; }

        public AddProjRefEvent(DungeonEngineReference @ref) => Reference = @ref;
    }
}

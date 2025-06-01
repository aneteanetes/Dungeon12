using Dungeon.Engine.Editable;
using Dungeon.Events;

namespace Dungeon.Engine.Events
{
    public class AddProjRefEvent : IEvent
    {
        public Reference Reference { get; set; }

        public AddProjRefEvent(Reference @ref) => Reference = @ref;
    }
}

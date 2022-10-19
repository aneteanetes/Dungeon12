using Dungeon.Types;

namespace Dungeon.Control.Events
{
    public interface IControlEventHandler
    {
        void Handle(ControlEventType @event);

        Square Location { get; }
    }
}
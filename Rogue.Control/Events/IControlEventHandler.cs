using Rogue.Types;

namespace Rogue.Control.Events
{
    public interface IControlEventHandler
    {
        void Handle(ControlEventType @event);

        Rectangle Location { get; }
    }
}
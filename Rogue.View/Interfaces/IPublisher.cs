namespace Rogue.View.Interfaces
{
    using System.Collections.Generic;

    public interface IPublisher
    {
        void Publish(List<IDrawSession> drawSessions);
    }
}
namespace Rogue.View.Interfaces
{
    using System.Collections.Generic;

    public interface IDrawClient
    {
        void Draw(IEnumerable<IDrawSession> drawSessions);
    }
}
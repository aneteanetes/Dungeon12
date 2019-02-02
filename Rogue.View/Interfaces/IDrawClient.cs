namespace Rogue.View.Interfaces
{
    using System.Collections.Generic;

    public interface IDrawClient
    {
        void Draw(IEnumerable<IDrawSession> drawSessions);

        void SetScene(IScene scene);

        void Animate(IAnimationSession animationSession);
    }
}
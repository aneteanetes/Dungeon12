namespace Rogue.View.Interfaces
{
    using Rogue.Types;
    using System.Collections.Generic;

    public interface IDrawClient : ICamera
    {
        void Draw(IEnumerable<IDrawSession> drawSessions);

        void SetScene(IScene scene);

        Point MeasureText(IDrawText drawText);

        void Animate(IAnimationSession animationSession);
    }
}
namespace Rogue.View.Interfaces
{
    using Rogue.Types;
    using System.Collections.Generic;

    public interface IDrawClient : ICamera
    {
        void Draw(IEnumerable<IDrawSession> drawSessions);

        void SetScene(IScene scene);

        Point MeasureText(IDrawText drawText);

        Point MeasureImage(string image);

        void SaveObject(ISceneObject sceneObject, string path, Point offset, string runtimeCacheName=null);

        void Animate(IAnimationSession animationSession);
    }
}
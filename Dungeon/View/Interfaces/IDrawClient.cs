namespace Dungeon.View.Interfaces
{
    using Dungeon.Types;
    using System.Collections.Generic;

    public interface IDrawClient : ICamera
    {
        void Draw(IEnumerable<IDrawSession> drawSessions);

        void SetScene(IScene scene);

        Point MeasureText(IDrawText drawText);

        Point MeasureImage(string image);

        void SaveObject(ISceneObject sceneObject, string path, Point offset, string runtimeCacheName=null);

        void Animate(IAnimationSession animationSession);

        void Drag(ISceneObject @object, ISceneObject area = null);

        void Drop();

        void SetCursor(string texture);

        /// <summary>
        /// Полностью кэширует объект, в т.ч. и маску
        /// </summary>
        /// <param name="object"></param>
        void CacheObject(ISceneObject @object);
    }
}
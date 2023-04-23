using System;

namespace Dungeon.View.Interfaces
{
    public interface IGameComponent
    {
        public string Name { get; set; }

        ISceneObject SceneObject { get; set; }

        void SetView(ISceneObject sceneObject);

        /// <summary>
        /// Вызвать уничтожение объекта.
        /// </summary>
        void Destroy();

        bool IsDestroyed { get; }

        void Init();
    }
}

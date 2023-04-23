using Dungeon.View.Interfaces;
using System;

namespace Dungeon.GameObjects
{
    public class GameComponentEmpty : IGameComponent
    {
        public ISceneObject SceneObject { get; set; }

        public void SetView(ISceneObject sceneObject)
        {
            SceneObject = sceneObject;
        }

        public virtual void Destroy()
        {
            SceneObject.Destroy();
        }

        public void Init() { }

        public static GameComponentEmpty Empty { get; } = new GameComponentEmpty();

        public string Name { get; set; }

        public bool IsDestroyed { get; set; }
    }
}
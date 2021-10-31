using Dungeon.View.Interfaces;

namespace Dungeon.GameObjects
{
    public class GameComponentEmpty : IGameComponent
    {
        public ISceneObject SceneObject { get; set; }

        public void SetView(ISceneObject sceneObject)
        {
            SceneObject = sceneObject;
        }

        public void Destroy()
        {
            SceneObject.Destroy?.Invoke();
        }

        public void Init() { }

        public static GameComponentEmpty Empty { get; } = new GameComponentEmpty();

        public string Name { get; set; }
    }
}

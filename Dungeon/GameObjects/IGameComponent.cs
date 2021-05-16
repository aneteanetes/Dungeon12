namespace Dungeon.View.Interfaces
{
    public interface IGameComponent
    {
        public string Name { get; set; }

        ISceneObject SceneObject { get; set; }

        void SetView(ISceneObject sceneObject);

        void Destroy();

        void Init();
    }
}

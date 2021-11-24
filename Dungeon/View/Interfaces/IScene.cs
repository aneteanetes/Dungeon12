namespace Dungeon.View.Interfaces
{
    public interface IScene
    {
        ISceneLayer[] Layers { get; }

        ISceneLayer GetLayer(string name);

        bool AbsolutePositionScene { get; }

        void Destroy();
    }
}
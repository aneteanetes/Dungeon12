namespace Dungeon.View.Interfaces
{
    public interface IScene
    {
        string Uid { get; } 

        ISceneLayer[] Layers { get; }

        ISceneLayer GetLayer(string name);

        bool AbsolutePositionScene { get; }

        void Destroy();

        void Loaded();
    }
}
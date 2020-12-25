namespace Dungeon.View.Interfaces
{
    public interface IScene
    {
        ISceneLayer[] Layers { get; }

        bool AbsolutePositionScene { get; }

        void Destroy();
    }
}
namespace Dungeon.View.Interfaces
{
    public interface ISceneLayer
    {
        ISceneObject[] Objects { get; }

        IEffect[] SceneGlobalEffects { get; }

        double Width { get; }

        double Height { get; }

        double Left { get; }

        double Top { get; }

        bool Destroyed { get; }
    }
}
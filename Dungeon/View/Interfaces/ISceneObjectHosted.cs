namespace Dungeon.View.Interfaces
{
    public interface ISceneObjectHosted : ISceneObject
    {
        ISceneObject Host { get; set; }
    }
}

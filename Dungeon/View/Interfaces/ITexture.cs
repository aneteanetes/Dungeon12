namespace Dungeon.View.Interfaces
{
    using Dungeon.Types;

    public interface ITexture
    {
        bool Contains(Dot point, Dot actualSize);
    }
}

namespace Dungeon.View.Interfaces
{
    using Dungeon.Types;

    public interface IDrawContext
    {
        IDrawColor BackgroundColor { get; set; }
        IDrawColor ForegroundColor { get; set; }

        Rectangle Region { get; set; }
    }
}
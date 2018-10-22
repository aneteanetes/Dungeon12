namespace Rogue.View.Interfaces
{
    using Rogue.Types;

    public interface IDrawContext
    {
        IDrawColor BackgroundColor { get; set; }
        IDrawColor ForegroundColor { get; set; }

        Rectangle Region { get; set; }
    }
}
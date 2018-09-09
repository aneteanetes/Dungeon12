namespace Rogue.View.Interfaces
{
    public interface IDrawContext
    {
        IDrawColor BackgroundColor { get; set; }
        IDrawColor ForegroundColor { get; set; }
    }
}

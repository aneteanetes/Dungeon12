namespace Rogue.View.Interfaces
{
    public interface IDrawable : IDrawContext
    {
        string Icon { get; set; }

        string Name { get; set; }
    }
}
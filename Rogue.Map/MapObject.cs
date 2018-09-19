namespace Rogue.Map
{
    using Rogue.View.Interfaces;

    public abstract class MapObject : IDrawable
    {
        public string Icon { get; set; }
        public string Name { get; set; }
        public IDrawColor BackgroundColor { get; set; }
        public IDrawColor ForegroundColor { get; set; }

        public abstract void Interact();
    }
}
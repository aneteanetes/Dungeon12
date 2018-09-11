using Rogue.View.Interfaces;

namespace Rogue.Entites
{
    public class Drawable : IDrawable
    {
        public string Icon { get; set; }
        public string Name { get; set; }
        public IDrawColor BackgroundColor { get; set; }
        public IDrawColor ForegroundColor { get; set; }
    }
}
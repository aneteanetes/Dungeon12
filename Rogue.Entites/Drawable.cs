using Rogue.Types;
using Rogue.View.Interfaces;

namespace Rogue.Entites
{
    /// <summary>
    /// Отрисовываемый
    /// </summary>
    public class Drawable : IDrawable
    {
        public string Icon { get; set; }
        public string Name { get; set; }
        public IDrawColor BackgroundColor { get; set; }
        public IDrawColor ForegroundColor { get; set; }

        public virtual string Tileset => "";

        public virtual Rectangle TileSetRegion => default;

        public virtual Rectangle Region => default;
    }
}
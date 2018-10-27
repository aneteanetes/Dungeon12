using Rogue.Types;

namespace Rogue.View.Interfaces
{
    public interface IDrawable : IDrawContext
    {
        string Icon { get; }

        string Name { get; }

        string Tileset { get; }

        Rectangle TileSetRegion { get; }

        /// <summary>
        /// Контейнер для рисования
        /// </summary>
        bool Container { get; }
    }
}
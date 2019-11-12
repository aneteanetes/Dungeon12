using Dungeon.Types;

namespace Dungeon.View.Interfaces
{
    public interface IDrawable : IGameComponent, IDrawContext
    {
        string Uid { get; }

        string Icon { get; }

        /// <summary>
        /// Возвращает <see cref="Icon"/> или значение
        /// <para>По сути это обратная совместимость которую надо будет выпилить</para>
        /// </summary>
        string Image { get; }

        string Name { get; }

        string Tileset { get; }

        Rectangle TileSetRegion { get; }

        /// <summary>
        /// Контейнер для рисования
        /// </summary>
        bool Container { get; }
    }
}
namespace Dungeon.View.Interfaces
{
    using System.Collections.Generic;
    using Dungeon.Control.Events;
    using Dungeon.Types;

    /// <summary>
    /// Сессия отрисовки в определённом месте
    /// </summary>
    public interface IDrawSession : IControlEventHandler
    {
        bool IsControlable { get; }

        void Write(int linePos, int charPos, IDrawText text);

        void Batch(int linePos, int charPos, List<IDrawText> lines);

        IDrawSession Run();

        bool AutoClear { get; }

        Square SessionRegion { get; }

        IEnumerable<IDrawText> TextContent { get; }

        IEnumerable<IDrawable> Drawables { get; }

        IEnumerable<IDrawablePath> DrawablePaths { get; }

        void Publish();
    }
}
namespace Rogue.View.Interfaces
{
    using System.Collections.Generic;
    using Rogue.Types;

    /// <summary>
    /// Сессия отрисовки в определённом месте
    /// </summary>
    public interface IDrawSession
    {
        void Write(int linePos, int charPos, IDrawText text);

        void Batch(int linePos, int charPos, List<IDrawText> lines);

        IDrawSession Run();

        bool AutoClear { get; }

        Rectangle Region { get; }

        IEnumerable<IDrawText> Content { get; }

        void Publish();
    }
}
namespace Rogue.View.Interfaces
{
    using System.Collections.Generic;
    using Rogue.Types;

    public interface IDrawText : IDrawContext, IGraph<IDrawText>
    {
        IEnumerable<IDrawText> Data { get; }

        string StringData { get; }

        int CharsCount { get; }

        void Append(IDrawText drawText);

        void Prepend(IDrawText drawText);

        void InsertAt(int index, IDrawText drawText);
    }
}
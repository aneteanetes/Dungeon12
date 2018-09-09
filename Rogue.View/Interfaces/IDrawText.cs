using Rogue.Types;

namespace Rogue.View.Interfaces
{
    public interface IDrawText : IDrawContext, IGraph<IDrawText>
    {
        string Data { get; }

        int CharsCount { get; }

        void Append(IDrawText drawText);

        void Prepend(IDrawText drawText);

        void InsertAt(int index, IDrawText drawText);
    }
}
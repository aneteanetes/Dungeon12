namespace Rogue.View.Interfaces
{
    using System.Collections.Generic;
    using Rogue.Types;

    public interface IDrawText : IDrawContext, IGraph<IDrawText>
    {
        IEnumerable<IDrawText> Data { get; }

        string StringData { get; }
        
        int Length { get; }

        void Append(IDrawText drawText);

        void Prepend(IDrawText drawText);

        void ReplaceAt(int index, IDrawText drawText);
    }
}
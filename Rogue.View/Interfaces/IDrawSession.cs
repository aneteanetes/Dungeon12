using System.Collections.Generic;

namespace Rogue.View.Interfaces
{
    public interface IDrawSession
    {
        void Write(int linePos, int charPos, IDrawText text);

        void Batch(int linePos, int charPos, List<IDrawText> lines);

        IDrawSession Run();

        IEnumerable<IDrawText> Content { get; }

        void Publish();
    }
}
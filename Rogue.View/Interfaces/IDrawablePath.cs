namespace Rogue.View.Interfaces
{
    using System.Collections.Generic;
    using Rogue.Types;
    using Rogue.View.Enums;

    public interface IDrawablePath : IDrawContext
    {
        float Depth { get; }

        bool Fill { get; }

        float Angle { get; }

        PathPredefined PathPredefined { get; }

        IEnumerable<Point> Path { get; }
    }
}
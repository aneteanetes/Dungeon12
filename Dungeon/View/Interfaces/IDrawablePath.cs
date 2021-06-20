namespace Dungeon.View.Interfaces
{
    using System.Collections.Generic;
    using Dungeon.Types;
    using Dungeon.View.Enums;

    public interface IDrawablePath : IDrawContext
    {
        string Uid { get; }

        float Depth { get; }

        bool Fill { get; }

        float Angle { get; }

        float Radius { get; }

        PathPredefined PathPredefined { get; }

        IEnumerable<Point> Path { get; }

        string Texture { get; }
    }
}
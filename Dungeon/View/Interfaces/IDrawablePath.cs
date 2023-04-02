namespace Dungeon.View.Interfaces
{
    using System.Collections.Generic;
    using Dungeon.Types;
    using Dungeon.View.Enums;

    public interface IDrawablePath : IDrawContext
    {
        string Uid { get; }

        double Depth { get; }

        bool Fill { get; }

        double Angle { get; }

        float Radius { get; }

        PathPredefined PathPredefined { get; }

        IEnumerable<Dot> Path { get; }

        string Texture { get; }
    }
}
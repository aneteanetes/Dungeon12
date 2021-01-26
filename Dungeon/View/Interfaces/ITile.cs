using Dungeon.Types;

namespace Dungeon.View.Interfaces
{
    public interface ITile
    {
        string Uid { get; }

        string Source { get; }

        int X { get; }

        int Y { get; }

        int Left { get;  }

        int Top { get;  }

        int Width { get;  }

        int Height { get; }

        Rectangle TileRegion { get; set; }

        Rectangle TilePosition { get; set; }
    }
}

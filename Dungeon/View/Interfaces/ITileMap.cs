using System.Collections.Generic;

namespace Dungeon.View.Interfaces
{
    public interface ITileMap
    {
        List<ITile> Tiles { get; set; }
    }
}
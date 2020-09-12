using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dungeon.Engine.Editable.TileMap
{
    public class DungeonEngineTilemapLayer
    {
        public string Name { get; set; }

        [Display(Name="Уровень игрока", Description ="Игрок будет рисоваться на этом уровне")]
        public bool PlayerLevel { get; set; }

        public List<DungeonEngineTilemapTile> Tiles { get; set; }
    }
}

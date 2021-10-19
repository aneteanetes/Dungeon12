using Dungeon;
using Dungeon.GameObjects;
using Dungeon.View.Enums;
using SidusXII.SceneObjects.Main.Map;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SidusXII.Models.Map
{
    [DebuggerDisplay("{X},{Y}")]
    public class MapCellComponent : GameComponent
    {
        public List<object> Objects { get; set; } = new List<object>();

        public List<TileInfo> Tiles { get; set; } = new List<TileInfo>();

        public bool Visible { get; set; }

        public string Fog { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public bool Spawn { get; set; }

        public MapCellComponent Cell { get; private set; }

        public List<MapCellComponent> Around = new List<MapCellComponent>();

        public MapCellComponent BottomLeft { get; private set; }

        public MapCellComponent BottomRight { get; private set; }

        public MapCellComponent Left { get; private set; }

        public MapCellComponent Right { get; private set; }

        public MapCellComponent TopLeft { get; private set; }

        public MapCellComponent TopRight { get; private set; }

        Dictionary<string, MapCellComponent> Cells;

        private static string Index(int x, int y) => $"{x},{y}";

        public void InitAround(Dictionary<string, MapCellComponent> cells)
        {
            var x = this.X;
            var y = this.Y;

            Cells = cells;
            Cell = Cells[Index(x, y)];

            Left = Cells[Index(x - 1, y)];
            TopLeft = Cells[Index(x - 1, y - 1)];
            BottomLeft = Cells[Index(x - 1, y + 1)];

            Right = Cells[Index(x + 1, y)];
            TopRight = Cells[Index(x + 1, y - 1)];
            BottomRight = Cells[Index(x, y + 1)];

            Around = new List<MapCellComponent>()
            {
                Left,
                TopLeft,
                BottomLeft,
                Right,
                TopRight,
                BottomRight
            };
        }

        public void CreateFog()
        {
            //if (!NearHaveNotVisible)
            //    return;

            string fogtile = "";

            //if (TopLeft.Visible)
            //{
            //    fogtile = "fogofwar_righttop.png";
            //}
            if (TopRight.Visible)
            {
                fogtile = "fogofwar_lefttop.png";
            }

            if (fogtile != "")
                this.Fog = fogtile;
        }

        public bool NearHaveNotVisible => Around.Any(x => !x.Visible);
    }

    public class TileInfo
    {
        public string AsmImgTile { get; set; }

        public FlipStrategy Flip { get; set; }
    }
}
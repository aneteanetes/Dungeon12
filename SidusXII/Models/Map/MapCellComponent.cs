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

        public List<MapCellPart> FogPartsForDelete = new List<MapCellPart>();

        public int X { get; set; }

        public int Y { get; set; }

        public bool Spawn { get; set; }

        public MapCellComponent Cell { get; private set; }

        public List<MapCellComponent> Around = new List<MapCellComponent>();

        public MapCellComponent Left { get; private set; }

        public MapCellComponent LeftTop { get; private set; }

        public MapCellComponent LeftBottom { get; private set; }

        public MapCellComponent Right { get; private set; }

        public MapCellComponent RightTop { get; private set; }

        public MapCellComponent RightBottom { get; private set; }

        public Dictionary<string, MapCellComponent> Cells { get; set; }

        private static string Index(int x, int y) => $"{x},{y}";

        public void InitAround()
        {
            var x = this.X;
            var y = this.Y;

            var even = y % 2 == 0;

            Cell = Cells[Index(x, y)];

            try
            {
                Left = Cells[Index(x - 1, y)];
            }
            catch { }
            try
            {
                LeftTop = Cells[Index(x - (even ? 1 : 0), y - 1)];
            }
            catch { }
            try
            {
                LeftBottom = Cells[Index(x - (even ? 1 : 0), y + 1)];
            }
            catch { }
            try
            {

                Right = Cells[Index(x + 1, y)];
            }
            catch { }
            try
            {
                RightTop = Cells[Index(x + (even ? 0 : 1), y - 1)];
            }
            catch { }
            try
            {
                RightBottom = Cells[Index(x + (even ? 0 : 1), y + 1)];
            }
            catch { }

            Around = new List<MapCellComponent>()
            {
                Left,
                LeftTop,
                LeftBottom,
                Right,
                RightTop,
                RightBottom
            };
        }

        public void ClearFog()
        {
            if (Left.Visible)
            {
                this.FogPartsForDelete.Add(MapCellPart.L);
            }
            if (LeftTop.Visible)
            {
                this.FogPartsForDelete.Add(MapCellPart.LT);
            }
            if (LeftBottom.Visible)
            {
                this.FogPartsForDelete.Add(MapCellPart.LB);
            }
            if (Right.Visible)
            {
                this.FogPartsForDelete.Add(MapCellPart.R);
            }
            if (RightTop.Visible)
            {
                this.FogPartsForDelete.Add(MapCellPart.RT);
            }
            if (RightBottom.Visible)
            {
                this.FogPartsForDelete.Add(MapCellPart.RB);
            }
        }

        public bool NearHaveNotVisible => Around.Any(x => !x.Visible);
    }

    public class TileInfo
    {
        public string AsmImgTile { get; set; }

        public FlipStrategy Flip { get; set; }
    }
}
using Dungeon;
using Dungeon.GameObjects;
using Dungeon.View.Enums;
using SidusXII.SceneObjects.Main.Map;
using System;
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

        public List<MapCellPart> FogPartsForDelete { get; set; } = new List<MapCellPart>();

        public int X { get; set; }

        public int Y { get; set; }

        public bool Spawn { get; set; }

        public MapCellComponent Cell { get; private set; }

        public List<MapCellComponent> Around = new List<MapCellComponent>();

        public MapCellComponent L { get; private set; }

        public MapCellComponent LT { get; private set; }

        public MapCellComponent LB { get; private set; }

        public MapCellComponent R { get; private set; }

        public MapCellComponent RT { get; private set; }

        public MapCellComponent RB { get; private set; }

        public Dictionary<string, MapCellComponent> Cells { get; set; }

        private static string Index(int x, int y) => $"{x},{y}";

        public void InitAround()
        {
            if (Around.Count > 0)
                return;

            var x = this.X;
            var y = this.Y;

            Cell = Cells[Index(x, y)];

            var setCell = SetCellXY(x, y);

            setCell(MapCellPart.L);
            setCell(MapCellPart.LT);
            setCell(MapCellPart.LB);
            setCell(MapCellPart.R);
            setCell(MapCellPart.RT);
            setCell(MapCellPart.RB);

        }

        private void SetCellOld()
        {
            var x = this.X;
            var y = this.Y;

            var even = y % 2 == 0;

            Cell = Cells[Index(x, y)];

            try
            {
                L = Cells[Index(x - 1, y)];
            }
            catch { }
            try
            {
                LT = Cells[Index(x - (even ? 1 : 0), y - 1)];
            }
            catch { }
            try
            {
                LB = Cells[Index(x - (even ? 1 : 0), y + 1)];
            }
            catch { }
            try
            {

                R = Cells[Index(x + 1, y)];
            }
            catch { }
            try
            {
                RT = Cells[Index(x + (even ? 0 : 1), y - 1)];
            }
            catch { }
            try
            {
                RB = Cells[Index(x + (even ? 0 : 1), y + 1)];
            }
            catch { }
        }

        private Func<MapCellPart, bool> SetCellXY(int xVal, int yVal)
        {
            var even = yVal % 2 == 0;

            return (MapCellPart part) =>
            {
                var x = xVal;
                var y = yVal;

                switch (part)
                {
                    case MapCellPart.L:
                        x -= 1;
                        break;

                    case MapCellPart.LT:
                        x -= (even ? 1 : 0);
                        y -= 1;
                        break;

                    case MapCellPart.LB:
                        x -= (even ? 1 : 0);
                        y += 1;
                        break;

                    case MapCellPart.R:
                        x += 1;
                        break;

                    case MapCellPart.RT:
                        x += (even ? 0 : 1);
                        y -= 1;
                        break;

                    case MapCellPart.RB:
                        x += (even ? 0 : 1);
                        y += 1;
                        break;

                    default:
                        break;
                }

                var indx = Index(x, y);
                if (Cells.ContainsKey(indx))
                {
                    var cell = Cells[indx];
                    this.SetPropertyExpr(part.ToString(), cell);
                    this.Around.Add(cell);
                }

                return true;
            };
        }

        public void ClearFog()
        {
            if (L?.Visible ?? false)
            {
                this.FogPartsForDelete.Add(MapCellPart.L);
            }
            if (LT?.Visible ?? false)
            {
                this.FogPartsForDelete.Add(MapCellPart.LT);
            }
            if (LB?.Visible ?? false)
            {
                this.FogPartsForDelete.Add(MapCellPart.LB);
            }
            if (R?.Visible ?? false)
            {
                this.FogPartsForDelete.Add(MapCellPart.R);
            }
            if (RT?.Visible ?? false)
            {
                this.FogPartsForDelete.Add(MapCellPart.RT);
            }
            if (RB?.Visible ?? false)
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
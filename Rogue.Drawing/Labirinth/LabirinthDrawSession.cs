using System.Collections.Generic;
using System.Linq;
using Rogue.Drawing.Impl;
using Rogue.Map;
using Rogue.Settings;
using Rogue.Types;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Labirinth
{
    public class LabirinthDrawSession : DrawSession
    {
        private DrawingSize DrawingSize = new DrawingSize();
        
        public Location Location { get; set; }

        public override IDrawSession Run()
        {
            var drawables = new List<IDrawable>();

            for (int y = 0; y < this.Location.Map.Count; y++)
            {
                var line = this.Location.Map[y];
                for (int x = 0; x < line.Count; x++)
                {
                    var cell = line[x];
                    var pos = new Point { X = x, Y = y };

                    if (cell[0].Icon == "#")
                    {
                        AddWall(drawables, pos);
                    }
                    else
                    {
                        AddObject(drawables, cell, pos);
                    }
                }
            }

            Drawables = drawables;
            return this;
        }

        private void AddObject(List<IDrawable> drawables, List<MapObject> cell, Point pos)
        {
            foreach (var item in cell)
            {
                item.Region = new Rectangle
                {
                    X = pos.X + 2,
                    Y = pos.Y + 2,
                    Height = 1,
                    Width = 1
                };

                drawables.Add(item);
            }
        }

        private void AddWall(List<IDrawable> drawables, Point pos)
        {
            List<bool[]> square = new List<bool[]>();

            TopSquare(pos, square);
            square.Add(GetLine(pos.X, this.Location.Map[pos.Y]));
            BotSquare(pos, square);
            PosSquare(pos, square);

            MapWallTile(square,pos);
        }

        private void PosSquare(Point pos, List<bool[]> square)
        {
            if (pos.Y < DrawingSize.MapLines - 2)
            {
                var positionalLine = this.Location.Map[pos.Y + 2];
                square.Add(GetLine(pos.X, positionalLine));
            }
            else
            {
                square.Add(EmptyLine);
            }
        }

        private void BotSquare(Point pos, List<bool[]> square)
        {
            if (pos.Y == DrawingSize.MapLines - 1)
            {
                square.Add(EmptyLine);
            }
            else
            {
                var botLine = this.Location.Map[pos.Y +1];
                square.Add(GetLine(pos.X, botLine));
            }
        }

        private void TopSquare(Point pos, List<bool[]> square)
        {
            if (pos.Y == 0)
            {
                square.Add(EmptyLine);
            }
            else
            {
                var topLine = this.Location.Map[pos.Y - 1];
                square.Add(GetLine(pos.X, topLine));
            }
        }

        private bool[] EmptyLine => Enumerable.Range(0, 3).Select(x => false).ToArray();

        private bool[] GetLine(int xPos, List<List<MapObject>> data)
        {
            bool IsWall(List<MapObject> cell)
            {
                return cell[0].Icon == "#";
            }

            var result = new List<bool>();

            if (xPos == 0)
            {
                result.Add(false);
            }
            else
            {
                result.Add(IsWall(data[xPos - 1]));
            }

            result.Add(IsWall(data[xPos]));

            if (xPos == DrawingSize.MapChars - 1)
            {
                result.Add(false);
            }
            else
            {
                result.Add(IsWall(data[xPos + 1]));
            }

            return result.ToArray();
        }

        private void MapWallTile(List<bool[]> wallMap, Point pos)
        {
            var isometricMap = IsometricMap(wallMap);
            var tile = DetermineTitle(isometricMap);
            var mapObj = this.Location.Map[pos.Y][pos.X][0];

            mapObj.TileSetRegion = new Rectangle
            {
                X = tile.X,
                Y = tile.Y,
                Height = 24,
                Width = 24
            };

            mapObj.Region = new Rectangle
            {
                X = pos.X + 2,
                Y = pos.Y + 2,
                Height = 24,
                Width = 24
            };
        }

        private List<bool[]> IsometricMap(List<bool[]> wallMap)
        {
            bool IsTop(int x, int y)
            {
                return wallMap[y][x] && (wallMap[y + 1][x] || !wallMap[y + 1][x]);
            }

            var topMap = new List<bool[]>();

            for (int i = 0; i < wallMap.Count - 1; i++)
            {
                var item = wallMap[i];
                var line = new List<bool>();

                for (int j = 0; j < item.Length; j++)
                {
                    var itm = item[j];

                    line.Add(IsTop(j, i));
                }

                topMap.Add(line.ToArray());
            }

            return topMap;
        }

        private Point DetermineTitle(List<bool[]> isometricMap)
        {
            var topLeft = isometricMap[0][0];
            var top = isometricMap[0][1];
            var topRight = isometricMap[0][2];
            var left = isometricMap[1][0];
            var mid = isometricMap[1][1];
            var right = isometricMap[1][2];
            var botLeft = isometricMap[2][0];
            var bot = isometricMap[2][1];
            var botRight = isometricMap[2][2];

            var yTexture = 0;
            var xTexture = 0;

            if (mid)
            {

                if (!topLeft && !topRight && !botLeft && !botRight
                    /*&& left && right && top && bot*/)
                {
                    yTexture = 2;
                    xTexture = 8;
                }

                if (!left/*&& right && bot*/)
                {
                    yTexture = 3;
                    xTexture = 0;
                }

                if (!bot)
                {
                    yTexture = 5;
                    xTexture = 2;
                }

                if (!left && !bot /*&& top && left*/ /*&& !topRight*/)
                {
                    yTexture = 2;
                    xTexture = 7;
                }

                if (!topLeft && !botLeft && !right
                    /*&& left && top && bot*/)
                {
                    yTexture = 3;
                    xTexture = 3;
                }

                if (!top && !botLeft && !botRight
                    /*&& left && right && bot*/)
                {
                    yTexture = 3;
                    xTexture = 2;
                }



                if (!bot && !right /*&& !topLeft*//* && left && top*/)
                {
                    yTexture = 3;
                    xTexture = 1;
                }


                if (!top && !right /*&& !botLeft*/ /*&& left && bot*/)
                {
                    yTexture = 2;
                    xTexture = 9;
                }

                if (!top && !bot /*&& left*/ /*&& right*/)
                {
                    yTexture = 2;
                    xTexture = 3;
                }


                if (!top && !left /*&& right && bot*/)
                {
                    yTexture = 2;
                    xTexture = 5;
                }

                if (!left && !right /*&& top && bot*/)
                {
                    yTexture = 2;
                    xTexture = 6;
                }

                if (!left && !top && !right /*&& bot*/)
                {
                    yTexture = 2;
                    xTexture = 1;
                }

                if (!top && !bot && !right /*&& left*/)
                {
                    yTexture = 2;
                    xTexture = 4;
                }

                if (!left && !top && !bot /*&& right*/)
                {
                    yTexture = 2;
                    xTexture = 2;
                }

                if (!left && !right && !bot /*&& top*/)
                {
                    yTexture = 2;
                    xTexture = 0;
                }

                if (!bot && !top && !left && !right)
                {
                    yTexture = 1;
                    xTexture = 9;
                }
            }

            return new Point
            {
                X = xTexture,
                Y = yTexture
            };
        }
    }
}
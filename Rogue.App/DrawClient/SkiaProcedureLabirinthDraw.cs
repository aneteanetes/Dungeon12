namespace Rogue.App.DrawClient
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Rogue.Resources;
    using Rogue.Scenes;
    using Rogue.Settings;
    using Rogue.View.Interfaces;
    using SkiaSharp;

    public class SkiaProcedureLabirinthDraw
    {
        private Random random;
        private IDrawSession labirinthDrawSession;
        private SKCanvas canvas;

        public SkiaProcedureLabirinthDraw(IDrawSession labirinthDrawSession, SKCanvas canvas, Random random)
        {
            this.labirinthDrawSession = labirinthDrawSession;
            this.canvas = canvas;
            this.random = random;

            this.DrawMethodMapping = new Dictionary<char, Action<float, float, List<List<char>>>>()
            {
                {'.',this.DrawFloor },
                {'#',this.DrawWall }
            };
        }

        private SKBitmap _tileset;
        private SKBitmap Tileset
        {
            get
            {
                if (_tileset == null)
                {
                    var stream = ResourceLoader.Load("Rogue.Resources.Images.Tiles.dblue.png");
                    _tileset = SKBitmap.Decode(stream);
                }

                return _tileset;
            }
        }

        //private SKBitmap _itemTileset;
        //private SKBitmap ItemTileset
        //{
        //    get
        //    {
        //        if (_itemTileset == null)
        //        {
        //        }

        //        return _itemTileset;
        //    }
        //}

        private readonly Dictionary<string, SKBitmap> tilesetsCache = new Dictionary<string, SKBitmap>();

        private SKBitmap TileSetByName(string tilesetName)
        {
            if(!tilesetsCache.TryGetValue(tilesetName, out var bitmap))
            {
                var stream = ResourceLoader.Load(tilesetName,tilesetName);
                bitmap = SKBitmap.Decode(stream);

                tilesetsCache.Add(tilesetName, bitmap);
            }

            return bitmap;
        }

        public void Draw()
        {
            var y = labirinthDrawSession.Region.Y * 24 + 3;
            var contentList = new List<IDrawText>(labirinthDrawSession.Content);

            for (int yLine = 0; yLine < contentList.Count; yLine++)
            {
                var x = labirinthDrawSession.Region.X * 11.5625f - 3;
                var fullLine = new List<char>(contentList[yLine].StringData);

                for (int xLine = 0; xLine < fullLine.Count; xLine++)
                {
                    List<List<char>> square = new List<List<char>>();
                    
                    if (yLine == 0)
                    {
                        square.Add(new List<char>() { ' ', ' ', ' ' });
                    }
                    else
                    {
                        var topLine = contentList[yLine - 1].StringData;
                        square.Add(GetLine(xLine, topLine));
                    }

                    square.Add(GetLine(xLine, contentList[yLine].StringData));


                    if (yLine == DrawingSize.MapLines - 1)
                    {
                        square.Add(new List<char>() { ' ', ' ', ' ' });
                    }
                    else
                    {
                        var botLine = contentList[yLine + 1].StringData;
                        square.Add(GetLine(xLine, botLine));
                    }

                    if (yLine < DrawingSize.MapLines - 2)
                    {
                        var botLine = contentList[yLine + 2].StringData;
                        square.Add(GetLine(xLine, botLine));
                    }
                    else
                    {
                        square.Add(new List<char>() { ' ', ' ', ' ' });
                    }

                    var @char= fullLine[xLine];
                    if (DrawMethodMapping.TryGetValue(@char, out var drawMethod))
                    {
                        drawMethod(x, y, square);
                    }
                    else
                    {
                        DrawObject(x, y, @char);
                    }

                    x += 24;
                }

                y += 24;
            }
        }

        private static readonly DrawingSize DrawingSize = new DrawingSize();

        /// <summary>
        /// Получает линию из строки учитывая есть там лево или право
        /// </summary>
        /// <returns></returns>
        private List<char> GetLine(int pos, string data)
        {
            var result = new List<char>();

            if (pos == 0)
            {
                result.Add(' ');
            }
            else
            {
                result.Add(data[pos - 1]);
            }

            result.Add(data[pos]);

            if (pos == DrawingSize.MapChars - 1)
            {
                result.Add(' ');
            }
            else
            {
                result.Add(data[pos + 1]);
            }

            return result;
        }

        private readonly Dictionary<char, Action<float, float, List<List<char>>>> DrawMethodMapping;

        private void DrawFloor(float x, float y, List<List<char>> drawContext)
        {
            var randomX = random.Next(0, 8);

            canvas.DrawBitmap(Tileset, new SKRect
            {
                Left = 24 * randomX,
                Top = 24 * 0,
                Size = new SKSize
                {
                    Height = 24,
                    Width = 24
                }
            }, new SKRect
            {
                Top = y - 24,
                Left = x,
                Size = new SKSize
                {
                    Height = 24,
                    Width = 24
                }
            });
        }

        private void DrawObject(float x, float y, char symbol)
        {
            if(symbol=='@')
            {
                this.DrawCharacter(x, y, SceneManager.CurrentManager.Current.Player);
            }
        }

        private void DrawCharacter(float x, float y, IDrawable drawable)
        {
            DrawFloor(x, y, null);
            var tileset = this.TileSetByName(drawable.Tileset);

            canvas.DrawBitmap(tileset, new SKRect
            {
                Left =drawable.Region.X,
                Top = drawable.Region.Y,
                Size = new SKSize
                {
                    Height = drawable.Region.Height,
                    Width = drawable.Region.Width
                }
            }, new SKRect
            {
                Top = y - 24,
                Left = x,
                Size = new SKSize
                {
                    Height = 24,
                    Width = 24
                }
            });
        }

        private void DrawWall(float x, float y, List<List<char>> drawContext)
        {
            var yTexture = 1;
            var xTexture = 0;

            //если под этой стеной есть стена или если под этой стеной нихуя нет

            //drawContext = drawContext.Skip(1).ToList();
            var topMap = new List<List<bool>>();

            for (int i = 0; i < drawContext.Count-1; i++)
            {
                var item = drawContext[i];
                var line = new List<bool>();

                for (int j = 0; j < item.Count; j++)
                {
                    var itm = item[j];

                    line.Add(IsTop(j, i, drawContext));
                }

                topMap.Add(line);
            }

            var topLeft = topMap[0][0];
            var top = topMap[0][1];
            var topRight = topMap[0][2];
            var left = topMap[1][0];
            var mid = topMap[1][1];
            var right = topMap[1][2];
            var botLeft = topMap[2][0];
            var bot = topMap[2][1];
            var botRight = topMap[2][2];


            bool mirror = false;
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
                    mirror = true;
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

            //    yTexture = 1;
            //    xTexture = 9;

            //    //если слева есть стена и выше её не стена
            //    bool left = false;
            //    if (drawContext[2][0]=='#' && drawContext[1][0]!='#')
            //    {
            //        left = true;
            //        yTexture = 2;
            //        xTexture = 4;
            //    }

            //    //если справа есть стена и выше её не стена
            //    bool right = false;
            //    if (drawContext[2][2]== '#' && drawContext[1][2] != '#')
            //    {
            //        right = true;
            //        yTexture = 2;
            //        xTexture = 2;
            //    }

            //    if(left && right)
            //    {
            //        yTexture = 2;
            //        xTexture = 3;
            //    }
            //}

            ////если НАД этой стеной есть стена
            //if(drawContext[1][1]=='#' && top)
            //{
            //    yTexture = 2;
            //    xTexture = 6;

            //    //если ПОД этой стеной одна стена но потом нихуя
            //    if(drawContext[4][1]==' ')
            //    {
            //        yTexture = 2;
            //        xTexture = 0;
            //    }
            //}

            //здесь будет логика по выбору текстуры

            canvas.DrawBitmap(Tileset, new SKRect
            {
                Left = 24 * xTexture,
                Top = 24 * yTexture,
                Size = new SKSize
                {
                    Height = 24,
                    Width = 24
                }
            }, new SKRect
            {
                Top = y - 24,
                Left = x,
                Size = new SKSize
                {
                    Height = 24,
                    Width = 24
                }
            });
        }

        private bool IsTop(int x, int y, List<List<char>> drawContext)
        {

            return (drawContext[y][x] == '#' && (drawContext[y + 1][x] == '#' || drawContext[y + 1][x] == ' '));
        }
    }
}
using System;
using System.Collections.Generic;
using Rogue.Drawing.Impl;
using Rogue.Map;
using Rogue.Settings;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Labirinth
{
    public class LabirinthDrawSession : DrawSession
    {
        private DrawingSize DrawingSize = new DrawingSize();

        public LabirinthDrawSession()
        {
            this.DrawRegion = new Types.Rectangle
            {
                X = 2,
                Y = 2,
                Width = DrawingSize.MapChars,
                Height = DrawingSize.MapLines,
            };
        }

        public Location Location { get; set; }

        public override IDrawSession Run()
        {
            var map = Location.Map;

            var batch = new List<IDrawText>();

            for (int y = 0; y < DrawingSize.MapLines; y++)
            {
                var line = DrawText.Empty(DrawingSize.MapChars);

                for (int x = 0; x < DrawingSize.MapChars; x++)
                {
                    var drawColor = map[y][x].ForegroundColor;
                    if (drawColor == null)
                        drawColor = new DrawColor(Location.Biom);

                    line.ReplaceAt(x, new DrawText(map[y][x].Icon, drawColor));
                    //if (map[x][y].Enemy != null)
                    //{
                    //    line.ReplaceAt(x, new DrawText(/*Enemy.Icon*/"E", /*Enemy.Chest*/ConsoleColor.DarkRed));
                    //}
                    //else if (map[x][y].Player != null)
                    //{
                    //    line.ReplaceAt(x, new DrawText(/*Enemy.Icon*/"@", /*Enemy.Chest*/ConsoleColor.Red));
                    //}
                    //else if (map[x][y].Item != null)
                    //{
                    //    line.ReplaceAt(x, new DrawText(/*Enemy.Icon*/"*", /*Enemy.Chest*/ConsoleColor.Green));
                    //}
                    //else if (map[x][y].Object != null)
                    //{
                    //    line.ReplaceAt(x, new DrawText(/*Enemy.Icon*/"!", /*Enemy.Chest*/ConsoleColor.Cyan));
                    //    //if (map[x][y].Object.Name == "Exit")
                    //    //else if (map[x][y].Object.Icon == '↨')                        
                    //}
                    //else if (map[x][y].Wall != null)
                    //{
                    //    line.ReplaceAt(x, new DrawText("#", BiomView.GetView().ForegroundColor));
                    //}
                    //else if (map[x][y].Trap != null)
                    //{
                    //    line.ReplaceAt(x, new DrawText("`", ConsoleColor.DarkGray));
                    //}
                }

                this.Write(y, 0, line);
            }

            //this.Batch(0, 0, batch);

            return this;
        }
    }
}
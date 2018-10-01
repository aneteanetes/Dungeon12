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
        public LabirinthDrawSession()
        {
            this.DrawRegion = new Types.Rectangle
            {
                X = 0,
                Y = 0,
                Width = 72,//DrawingSize.MapChars,
                Height = 23//DrawingSize.MapLines,
            };
        }

        public Location Location { get; set; }

        public IView<Biom> BiomView { get; set; }

        public DrawingSize DrawingSize { get; set; }

        public override IDrawSession Run()
        {
            var map = Location.Map;

            var batch = new List<IDrawText>();

            //72 * 23 потому что интерфейс рядом есть            
            for (int y = 0; y < 23; y++)
            {
                var line = DrawText.Empty(71);

                for (int x = 0; x < 71; x++)
                {
                    line.ReplaceAt(x, new DrawText(map[y][x].Icon));
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
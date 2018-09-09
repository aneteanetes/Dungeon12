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
                X = 1,
                Y = 1,
                Width = DrawingSize.MapChars,
                Height = DrawingSize.MapLines,
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
            for (int y = 0; y < DrawingSize.MapLines; y++)
            {
                var line = DrawText.Empty(DrawingSize.MapChars);

                for (int x = 0; x < DrawingSize.MapChars; x++)
                {
                    if (map[x][y].Enemy != null)
                    {
                        line.InsertAt(x, new DrawText(/*Enemy.Icon*/"E", /*Enemy.Chest*/ConsoleColor.DarkRed));
                    }
                    else if (map[x][y].Player != null)
                    {
                        line.InsertAt(x, new DrawText(/*Enemy.Icon*/"@", /*Enemy.Chest*/ConsoleColor.Red));
                    }
                    else if (map[x][y].Item != null)
                    {
                        line.InsertAt(x, new DrawText(/*Enemy.Icon*/"*", /*Enemy.Chest*/ConsoleColor.Green));
                    }
                    else if (map[x][y].Object != null)
                    {
                        line.InsertAt(x, new DrawText(/*Enemy.Icon*/"!", /*Enemy.Chest*/ConsoleColor.Cyan));
                        //if (map[x][y].Object.Name == "Exit")
                        //else if (map[x][y].Object.Icon == '↨')                        
                    }
                    else if (map[x][y].Wall != null)
                    {
                        line.InsertAt(x, new DrawText("#", BiomView.GetView().ForegroundColor));
                    }
                    else if (map[x][y].Trap != null)
                    {
                        line.InsertAt(x, new DrawText("`", ConsoleColor.DarkGray));
                    }
                }

                batch.Add(line);
            }

            this.Batch(1, 1, batch);

            return this;
        }
    }
}
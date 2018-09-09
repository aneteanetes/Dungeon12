using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Drawing
{
    public static class LabDraw
    {
        /// <summary>
        /// Redraw character location
        /// </summary>
        /// <param name="xold">Char was there (x)</param>
        /// <param name="yold">Char was there (y)</param>
        /// <param name="xnew">Char will be there (x)</param>
        /// <param name="ynew">Char will be there (y)</param>
        public static void ReDrawObject(int xold, int yold, int xnew, int ynew)
        {
            var Lab = Rogue.RAM.Map;
            //Console.SetCursorPosition(xold + 2, yold + 1);
            //Console.Write(' ');
            char oldchar = ' ';
            Int16 oldatr = 0;
            if (Lab.Map[xold][yold].Trap != null) { oldatr = Convert.ToInt16(ConsoleColor.DarkGray); oldchar = '?'; }
            if (Lab.Map[xold][yold].Object != null && Lab.Map[xold][yold].Object.Icon == '$') { oldatr = Convert.ToInt16(ConsoleColor.Yellow); oldchar = '$'; }
            if (Lab.Map[xold][yold].Item != null) { oldatr = Convert.ToInt16(Lab.Map[xold][yold].Item.Color); oldchar = Lab.Map[xold][yold].Item.Icon(); }

            //Console.ForegroundColor = Rogue.RAM.Player.Color;
            //Console.SetCursorPosition(xnew + 2, ynew + 1);
            //Console.Write(Rogue.RAM.Player.Icon);


            DrawEngine.winAPIDraw.Helpers.ReDrawOneChar(Convert.ToInt16(xold), Convert.ToInt16(yold), Convert.ToInt16(xnew), Convert.ToInt16(ynew), oldchar, Rogue.RAM.Player.Icon, oldatr, Convert.ToInt16(Rogue.RAM.Player.Color));
        }
        /// <summary>
        /// ReDrawCharacter, without position
        /// </summary>
        public static void ReDrawObject()
        {
            var Lab = Rogue.RAM.Map;
            bool was = false;
            for (int y = 0; y < 23; y++)
            {
                if (!was)
                {
                    for (int x = 0; x < 71; x++)
                    {
                        if (Lab.Map[x][y].Player != null)
                        {
                            char mchar = Lab.Map[x][y].Player.Icon;
                            Int16 matr = Convert.ToInt16(Lab.Map[x][y].Player.Color);
                            DrawEngine.winAPIDraw.Helpers.ReDrawOneChar(Convert.ToInt16(x), Convert.ToInt16(y), Convert.ToInt16(x), Convert.ToInt16(y), mchar, mchar, matr, matr);
                            return;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Destroy object draw
        /// </summary>
        /// <param name="xold">x</param>
        /// <param name="yold">y</param>
        public static void ReDrawObject(int xold, int yold)
        {
            char c = '\0';
            Int16 col = 0;
            if (Rogue.RAM.Map.Map[xold][yold].Player != null)
            {
                c = Rogue.RAM.Player.Icon;
                col = Convert.ToInt16(Rogue.RAM.Player.Color);
            }
            DrawEngine.winAPIDraw.Helpers.ReDrawOneChar((Int16)xold, (Int16)yold, (Int16)xold, (Int16)yold, ' ', c, 0, col);
        }
        /// <summary>
        /// Draw new object in cell
        /// </summary>
        /// <param name="xnew">x location</param>
        /// <param name="ynew">y location</param>
        /// <param name="I">Item</param>
        /// <param name="M">Monster</param>
        /// <param name="D">Door</param>
        /// <param name="W">Wall</param>
        public static void ReDrawObject(int xnew, int ynew, MechEngine.Item I = null, MechEngine.Monster M = null, MechEngine.ActiveObject D = null, MechEngine.Wall W = null)
        {
            char mchar = new char();
            Int16 matr = new short();
            if (I != null)
            {
                mchar = I.Icon();
                matr = Convert.ToInt16(I.Color);
            }
            else if (M != null)
            {
                mchar = M.Icon;
                matr = Convert.ToInt16(M.Chest);
            }
            else if (D != null)
            {
                mchar = D.Icon;
                matr = Convert.ToInt16(D.Color);
            }
            else if (W != null)
            {
                mchar = '#';
                matr = Convert.ToInt16(Rogue.RAM.Map.Biom);
            }
            DrawEngine.winAPIDraw.Helpers.ReDrawOneChar(Convert.ToInt16(xnew), Convert.ToInt16(ynew), Convert.ToInt16(xnew), Convert.ToInt16(ynew), mchar, mchar, matr, matr);
        }
    }
}

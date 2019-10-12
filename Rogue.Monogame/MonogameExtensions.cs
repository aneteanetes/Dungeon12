using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rogue.View.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Monogame
{
    public static class MonogameExtensions
    {
        public static void SetPixel(this Texture2D texture2D, int x, int y, Color c)
        {
            Rectangle r = new Rectangle(x, y, 1, 1);
            Color[] color = new Color[1];
            color[0] = c;

            texture2D.SetData(0, r, color, 0, 1);
        }

        public static Color GetPixel(this Texture2D texture2D, int x, int y)
        {
            Rectangle r = new Rectangle(x, y, 1, 1);
            Color[] color = new Color[1];
            texture2D.GetData(0, r, color, 0, 1);
            return color[0];
        }

        public static Color Convert(this IDrawColor drawColor) => new Color(drawColor.R, drawColor.G, drawColor.B, drawColor.A);
    }
}

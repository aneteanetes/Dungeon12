using System;
using System.Collections.Generic;
using System.Text;
using Rogue.Entites;
using Rogue.Types;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Impl
{
    internal static class TileCampatibility
    {
        //protected virtual bool UI => false;
        private const string LightBarsTileset = "Rogue.Resources.Images.GUI.Light Bars.png";

        //private void TileCampatibility()
        //{
        //    foreach (var item in this.buffer)
        //    {
        //        foreach (var part in item.Data)
        //        {

        //        }

        //        if (item.StringData.Any(c => OldCharEntries.Contains(c)))
        //        {

        //        }
        //    }
        //}

        //private void ReplaceDrawText(IDrawText drawText)
        //{
        //    if (!drawText.IsEmptyInside)
        //    {
        //        foreach (var drawPart in drawText.Data)
        //        {
        //            ReplaceDrawText(drawPart);
        //        }
        //    }
        //    else
        //    {
        //        if (drawText.StringData.coun)
        //    }
        //}
        
        public static Drawable TileAssigment(char c)
        {
            switch (c)
            {
                case '═': return new Drawable() { Tileset = LightBarsTileset, TileSetRegion = new Rectangle { X = 10, Y = 0, Width = 17, Height = 10 } };
                case '║': return new Drawable() { Tileset = LightBarsTileset, TileSetRegion = new Rectangle { X = 0, Y = 10, Width = 10, Height = 17 } };
                case '╔': return new Drawable() { Tileset = LightBarsTileset, TileSetRegion = new Rectangle { X = 0, Y = 0, Width = 10, Height = 10 } };
                case '╗': return new Drawable() { Tileset = LightBarsTileset, TileSetRegion = new Rectangle { X = 54, Y = 0, Width = 10, Height = 10 } };
                case '╠': return new Drawable() { Tileset = LightBarsTileset, TileSetRegion = new Rectangle { X = 0, Y = 27, Width = 10, Height = 10 } };
                case '╣': return new Drawable() { Tileset = LightBarsTileset, TileSetRegion = new Rectangle { X = 54, Y = 27, Width = 10, Height = 10 } };
                case '╚': return new Drawable() { Tileset = LightBarsTileset, TileSetRegion = new Rectangle { X = 0, Y = 54, Width = 10, Height = 10 } };
                case '╝': return new Drawable() { Tileset = LightBarsTileset, TileSetRegion = new Rectangle { X = 54, Y = 54, Width = 10, Height = 10 } };
                default: return null;
            }
        }
    }
}


//b.HorizontalLine = '═';
//                b.LowerLeftCorner = '╚';
//                b.LowerRightCorner = '╝';
//                b.PerpendicularLeftward = '╣';
//                b.PerpendicularRightward = '╠';
//                b.UpperLeftCorner = '╔';
//                b.UpperRightCorner = '╗';
//                b.VerticalLine = '║';
//                return b;
//            }
//        }
//        public static Border LightBorder
//{
//    get
//    {
//        Border b = new Border();
//        b.HorizontalLine = '─';
//        b.LowerLeftCorner = '└';
//        b.LowerRightCorner = '┘';
//        b.PerpendicularLeftward = '┤';
//        b.PerpendicularRightward = '├';
//        b.UpperLeftCorner = '┌';
//        b.UpperRightCorner = '┐';
//        b.VerticalLine = '│';
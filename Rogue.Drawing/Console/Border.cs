using System.Collections.Generic;
using Rogue.Drawing.Impl;
using Rogue.Types;
using Rogue.View.Interfaces;

namespace Rogue.Drawing.Console
{
    /// <summary>
    /// Charset for window border
    /// </summary>
    public class Border
    {
        public char UpperLeftCorner { set; get; }
        public char UpperRightCorner { set; get; }
        public char HorizontalLine { set; get; }
        public char VerticalLine { set; get; }
        public char LowerLeftCorner { set; get; }
        public char LowerRightCorner { set; get; }
        public char PerpendicularLeftward { set; get; }
        public char PerpendicularRightward { set; get; }

        public IEnumerable<IDrawable> CompleteBorder(Rectangle drawRegion)
        {
            List<IDrawable> borderList = new List<IDrawable>();

            var topCorner = TileCampatibility.TileAssigment('╔');
            topCorner.Region = new Types.Rectangle
            {
                X = drawRegion.X,
                Y = drawRegion.Y,
                Height = 24,
                Width = 24
            };
            borderList.Add(topCorner);

            for (int i = (int)drawRegion.X + 1; i < drawRegion.X+1+ (drawRegion.Width - 1); i++)
            {
                var top = TileCampatibility.TileAssigment('═');
                top.Region = new Types.Rectangle
                {
                    X = i,
                    Y = drawRegion.Y,
                    Height = 24,
                    Width = 24
                };
                borderList.Add(top);
            }

            var topCorner2 = TileCampatibility.TileAssigment('╗');
            topCorner2.Region = new Types.Rectangle
            {
                X = drawRegion.X + drawRegion.Width - 1,
                Y = drawRegion.Y,
                Height = 24,
                Width = 24
            };
            borderList.Add(topCorner2);

            for (int i = (int)drawRegion.Y + 1; i < drawRegion.Y+1 + drawRegion.Height - 2; i++)
            {
                var leftVert = TileCampatibility.TileAssigment('║');
                leftVert.Region = new Types.Rectangle
                {
                    X = drawRegion.X,
                    Y = i,
                    Height = 24,
                    Width = 24
                };
                borderList.Add(leftVert);

                var rightVert = TileCampatibility.TileAssigment('║');
                rightVert.Region = new Types.Rectangle
                {
                    X = drawRegion.X+drawRegion.Width-1,
                    Y = i,
                    Height = 24,
                    Width = 24
                };
                borderList.Add(rightVert);
            }

            var botCorner = TileCampatibility.TileAssigment('╚');
            botCorner.Region = new Types.Rectangle
            {
                X = drawRegion.X,
                Y = drawRegion.Y + drawRegion.Height-2,
                Height = 24,
                Width = 24
            };
            borderList.Add(botCorner);

            for (int i = (int)drawRegion.X + 1; i < drawRegion.X + (drawRegion.Width - 1); i++)
            {
                var top = TileCampatibility.TileAssigment('═');
                top.Region = new Types.Rectangle
                {
                    X = i,
                    Y = drawRegion.Y + drawRegion.Height -2,
                    Height = 24,
                    Width = 24
                };
                borderList.Add(top);
            }

            var botCorner2 = TileCampatibility.TileAssigment('╝');
            botCorner2.Region = new Types.Rectangle
            {
                X = drawRegion.X + drawRegion.Width - 1,
                Y = drawRegion.Y + drawRegion.Height - 2,
                Height = 24,
                Width = 24
            };
            borderList.Add(botCorner2);

            return borderList;
        }
    }
}

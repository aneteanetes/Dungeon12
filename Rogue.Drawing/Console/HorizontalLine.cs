namespace Rogue.Drawing.Console
{
    using System.Collections.Generic;
    using System.Linq;
    using Rogue.Drawing.Impl;
    using Rogue.View.Interfaces;

    public class HorizontalLine : Interface
    {
        public HorizontalLine(Window window)
        {
            this.Parent = window;
        }

        public override IEnumerable<IDrawable> ConstructTiles()
        {
            var linePath = new DrawablePath()
            {
                ForegroundColor = ColorSchemaColor,
                Depth = 5f
            };

            var startX = (this.Parent.Left + this.Left) * 24;
            var startY = (this.Parent.Top + this.Top) * 24;

            linePath.Paths.Add(new Types.Point(startX,startY));
            linePath.Paths.Add(new Types.Point(startX+(this.Width * 24), startY+(this.Height * 24)));

            this.Paths.Add(linePath);



            //var drawRegion = new Types.Rectangle
            //{
            //    X = this.Window.Left+ this.Left,
            //    Y = this.Window.Top+this.Top,
            //    Width = this.Window.Width,
            //    Height = 1
            //};

            //var left = TileCampatibility.TileAssigment('╠');
            //left.Region = new Types.Rectangle
            //{
            //    X = drawRegion.X,
            //    Y = drawRegion.Y,
            //    Height = 24,
            //    Width = 24
            //};
            //this.DrawableList.Add(left);

            //for (int i = (int)drawRegion.X + 1; i < drawRegion.X + 1 + (drawRegion.Width - 1); i++)
            //{
            //    var line = TileCampatibility.TileAssigment('═');
            //    line.Region = new Types.Rectangle
            //    {
            //        X = i,
            //        Y = drawRegion.Y,
            //        Height = 24,
            //        Width = 24
            //    };
            //    this.DrawableList.Add(line);
            //}

            //var right = TileCampatibility.TileAssigment('╣');
            //right.Region = new Types.Rectangle
            //{
            //    X = drawRegion.X+drawRegion.Width-1,
            //    Y = drawRegion.Y,
            //    Height = 24,
            //    Width = 24
            //};
            //this.DrawableList.Add(right);

            return this.DrawableList;
        }        

        public override IDrawSession Run()
        {
            return base.Run();
        }
    }
}
namespace Rogue.Drawing.GUI
{
    using Rogue.Drawing.Controls;
    using Rogue.Drawing.Impl;
    using Rogue.Settings;
    using Rogue.Types;
    using Rogue.View.Interfaces;

    public class GUIBorderDrawSession : DrawSession
    {
        private DrawingSize DrawingSize = new DrawingSize();

        public GUIBorderDrawSession()
        {
            AutoClear = false;
            this.DrawRegion = new Types.Rectangle
            {
                X = 1,
                Y = 1,
                Height = DrawingSize.WindowLines-1,
                Width = DrawingSize.WindowChars-1
            };
        }

        public override IDrawSession Run()
        {
            new Window()
            {
                Large = true,
                Left = 0.25f,
                Top = 1.3f,
                Width = DrawingSize.MapChars + 1.5f,
                Height = DrawingSize.MapLines + 1.4f
            }.Run().Publish();

            new ButtonsWindow()
            {
                Left = DrawingSize.MapChars + 1.9f,
                Top = DrawingSize.MapLines + 1.4f + 1.5f,
                Width = DrawingSize.WindowChars - DrawingSize.MapChars - 1.7f,
                Height = 2

            }.Run().Publish();

            new Image("Rogue.Resources.Images.Icons.helmet.png")
            {
                Left = DrawingSize.MapChars + 2.1f,
                Top = DrawingSize.MapLines + 1.4f + 1.5f,
                Width = 2,
                Height = 2,
                ImageTileRegion = new Rectangle
                {
                    X = 0,
                    Y = 0,
                    Height = 64,
                    Width = 64
                }
            }.Run().Publish();

            new Image("Rogue.Resources.Images.Icons.backpack.png")
            {
                Left = DrawingSize.MapChars + 2.1f+2,
                Top = DrawingSize.MapLines + 1.4f + 1.5f,
                Width = 2,
                Height = 2,
                ImageTileRegion = new Rectangle
                {
                    X = 0,
                    Y = 0,
                    Height = 64,
                    Width = 64
                }
            }.Run().Publish();

            new Image("Rogue.Resources.Images.Icons.map.png")
            {
                Left = DrawingSize.MapChars + 2.1f + 4,
                Top = DrawingSize.MapLines + 1.4f + 1.5f,
                Width = 2,
                Height = 2,
                ImageTileRegion = new Rectangle
                {
                    X = 0,
                    Y = 0,
                    Height = 64,
                    Width = 64
                }
            }.Run().Publish();

            new Image("Rogue.Resources.Images.Icons.tome.png")
            {
                Left = DrawingSize.MapChars + 2.1f + 6,
                Top = DrawingSize.MapLines + 1.4f + 1.5f,
                Width = 2,
                Height = 2,
                ImageTileRegion = new Rectangle
                {
                    X = 0,
                    Y = 0,
                    Height = 64,
                    Width = 64
                }
            }.Run().Publish();
            
            new Image("Rogue.Resources.Images.Icons.x.png")
            {
                Left = DrawingSize.MapChars + 2.1f + 8,
                Top = DrawingSize.MapLines + 1.4f + 1.5f,
                Width = 2,
                Height = 2,
                ImageTileRegion = new Rectangle
                {
                    X = 0,
                    Y = 0,
                    Height = 64,
                    Width = 64
                }
            }.Run().Publish();

            return this;
        }
    }
}
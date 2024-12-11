using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.SceneObjects.Base;
using Dungeon.View.Enums;

namespace Nabunassar.SceneObjects.Base
{
    internal class BorderMap : EmptySceneObject
    {
        public BorderMap(double width, double height, double opacity = .9, string imageMap= null) : this(new BorderConfiguration()
        {
            Width = width,
            Height = height,
            Size=32,
            ImagesPath= imageMap ?? "UI/bordermin/bord2.png",
            Opacity =opacity
        })
        { }

        public void Resize(double width, double height)
        {
            rightup.Left=width-size;
            leftdown.Top=height-size;

            rightdown.Left=width-size;
            rightdown.Top=height-size;

            left.Height=height-size*2;
            right.Height=height-size*2;
            right.Left=width-size;

            down.Width=width-size*2;
            down.Top=height-size;

            up.Width=width-size*2;

            back.Width=width;
            back.Height=height;
        }

        ImageObject left, right, up, down, leftup, rightup,leftdown,rightdown;
        DarkRectangle back;

        BorderConfiguration _settings;
        double size;

        public BorderMap(BorderConfiguration settings)
        {
            this.Width = settings.Width;
            this.Height = settings.Height;

            var padding = settings.Padding;
            size = settings.Size;

            if (settings.Opacity>0)
                back = this.AddChild(new DarkRectangle() { Width=settings.Width, Height=settings.Height, Opacity=settings.Opacity });

            leftup = this.AddChild(new ImageObject(settings.ImagesPath)
            {
                Width = size,
                Height = size,
                Mode = DrawMode.Tiled,
                ImageRegion = new Dungeon.Types.Square()
                {
                    Height = size,
                    Width = size
                },
                Color = Global.CommonColor,
                Left=-padding,
                Top=-padding
                
            });

            rightup = this.AddChild(new ImageObject(settings.ImagesPath)
            {
                Width = size,
                Height = size,
                Left = (this.Width - size)+padding,
                Top = -padding,
                Mode = DrawMode.Tiled,
                ImageRegion = new Dungeon.Types.Square()
                {
                    Height = size,
                    Width = size,
                    X = size * 2
                },
                Color = Global.CommonColor
            });

            leftdown = this.AddChild(new ImageObject(settings.ImagesPath)
            {
                Width = size,
                Height = size,
                Top = (this.Height - size) +padding,
                Left=-padding,
                Mode = DrawMode.Tiled,
                ImageRegion = new Dungeon.Types.Square()
                {
                    Height = size,
                    Width = size,
                    Y = size * 2
                },
                Color = Global.CommonColor
            });

            rightdown = this.AddChild(new ImageObject(settings.ImagesPath)
            {
                Width = size,
                Height = size,
                Left = (this.Width - size) + padding,
                Top = (this.Height - size) + padding,
                Mode = DrawMode.Tiled,
                ImageRegion = new Dungeon.Types.Square()
                {
                    Height = size,
                    Width = size,
                    X = size * 2,
                    Y = size * 2
                },
                Color = Global.CommonColor
            });

            left = this.AddChild(new ImageObject(settings.ImagesPath)
            {
                Width = size,
                Height = (this.Height - size * 2)+(padding * 2),
                Top = size-padding,
                Left = -padding,
                Mode = DrawMode.Tiled,
                ImageRegion = new Dungeon.Types.Square()
                {
                    Height = size,
                    Width = size,
                    Y = size
                },
                Color = Global.CommonColor
            });

            right = this.AddChild(new ImageObject(settings.ImagesPath)
            {
                Width = size,
                Height = (this.Height - size * 2)+(padding*2),
                Top = size - padding,
                Left = (this.Width - size)+padding,
                Mode = DrawMode.Tiled,
                ImageRegion = new Dungeon.Types.Square()
                {
                    Height = size,
                    Width = size,
                    X = size * 2,
                    Y = size
                },
                Color = Global.CommonColor
            });

            down = this.AddChild(new ImageObject(settings.ImagesPath)
            {
                Width = (this.Width - size * 2)+(padding*2),
                Height = size,
                Top = (this.Height - size) + padding,
                Left = size - padding,
                Mode = DrawMode.Tiled,
                ImageRegion = new Dungeon.Types.Square()
                {
                    Height = size,
                    Width = size,
                    X = size,
                    Y = size * 2
                },
                Color = Global.CommonColor
            });

            up = this.AddChild(new ImageObject(settings.ImagesPath)
            {
                Width = (this.Width - size * 2) + (padding * 2),
                Top=-padding,
                Height = size,
                Left = size - padding,
                Mode = DrawMode.Tiled,
                ImageRegion = new Dungeon.Types.Square()
                {
                    Height = size,
                    Width = size,
                    X = size
                },
                Color = Global.CommonColor
            });
        }
    }
}
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.View.Enums;

namespace Dungeon12.SceneObjects.Base
{
    internal class NineSliceBorder : EmptySceneObject
    {
        public NineSliceBorder(double width, double height) : this(new NineSliceSettings()
        {
            Width = width,
            Height = height,
            Size=5,
            ImagesPath="UI/border/"
        })
        { }

        public NineSliceBorder(NineSliceSettings settings)
        {
            this.Width = settings.Width;
            this.Height = settings.Height;

            var size = settings.Size;

            this.AddChild(new DarkRectangle() { Width=settings.Width, Height=settings.Height, Opacity=0.9 });

            this.AddChild(new ImageObject($"{settings.ImagesPath}leftup.png") { Width=size, Height=size });
            this.AddChild(new ImageObject($"{settings.ImagesPath}rightup.png") { Width=size, Height=size, Left=this.Width-size });
            this.AddChild(new ImageObject($"{settings.ImagesPath}leftdown.png") { Width=size, Height=size, Top=this.Height-size });
            this.AddChild(new ImageObject($"{settings.ImagesPath}rightdown.png") { Width=size, Height=size, Left=this.Width-size, Top=this.Height-size });

            this.AddChild(new ImageObject($"{settings.ImagesPath}left.png") { Width=size, Height=this.Height-size*2, Top=size, Mode= DrawMode.Tiled });
            this.AddChild(new ImageObject($"{settings.ImagesPath}right.png") { Width=size, Height=this.Height-size*2, Top=size, Left=this.Width-size, Mode= DrawMode.Tiled });

            this.AddChild(new ImageObject($"{settings.ImagesPath}down.png") { Width=this.Width-size*2, Height=size, Top=this.Height-size, Left=size, Mode= DrawMode.Tiled });
            this.AddChild(new ImageObject($"{settings.ImagesPath}up.png") { Width=this.Width-size*2, Height=size, Left=size, Mode= DrawMode.Tiled });

        }
    }

    public class NineSliceSettings
    {
        public double Size { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public string ImagesPath { get; set; }
    }
}

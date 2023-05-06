using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.SceneObjects.Base;
using Dungeon.View.Enums;
using Dungeon.View.Interfaces;

namespace Dungeon12.SceneObjects.Base
{
    internal class Border : EmptySceneObject
    {
        public Border(double width, double height, double opacity = .9) : this(new NineSliceSettings()
        {
            Width = width,
            Height = height,
            Size=5,
            ImagesPath="UI/border/",
            Opacity =opacity
        })
        { }

        public Border(NineSliceSettings settings)
        {
            this.Width = settings.Width;
            this.Height = settings.Height;

            var size = settings.Size;

            if (settings.Opacity>0)
                this.AddChild(new DarkRectangle() { Width=settings.Width, Height=settings.Height, Opacity=settings.Opacity });

            this.AddChild(new ImageObject($"{settings.ImagesPath}leftup.png") { Width=size, Height=size, Mode= DrawMode.Tiled });
            this.AddChild(new ImageObject($"{settings.ImagesPath}rightup.png") { Width=size, Height=size, Left=this.Width-size, Mode= DrawMode.Tiled });
            this.AddChild(new ImageObject($"{settings.ImagesPath}leftdown.png") { Width=size, Height=size, Top=this.Height-size, Mode= DrawMode.Tiled });
            this.AddChild(new ImageObject($"{settings.ImagesPath}rightdown.png") { Width=size, Height=size, Left=this.Width-size, Top=this.Height-size, Mode= DrawMode.Tiled });

            this.AddChild(new ImageObject($"{settings.ImagesPath}left.png") { Width=size, Height=this.Height-size*2, Top=size, Mode= DrawMode.Tiled });
            this.AddChild(new ImageObject($"{settings.ImagesPath}right.png") { Width=size, Height=this.Height-size*2, Top=size, Left=this.Width-size, Mode= DrawMode.Tiled });

            this.AddChild(new ImageObject($"{settings.ImagesPath}down.png") { Width=this.Width-size*2, Height=size, Top=this.Height-size, Left=size, Mode= DrawMode.Tiled });
            this.AddChild(new ImageObject($"{settings.ImagesPath}up.png") { Width=this.Width-size*2, Height=size, Left=size, Mode= DrawMode.Tiled });
        }
    }

    internal class NineSliceSettings
    {
        public double Size { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public string ImagesPath { get; set; }

        public double Opacity { get; set; } = .9;

        private static NineSliceSettings _default;
        public static NineSliceSettings Default 
            => _default ??= new NineSliceSettings()
            {
                Size=5,
                ImagesPath="UI/border/",
            };

        public NineSliceSettings BindDefaults(ISceneObject sceneObject)
        {
            if (this.Width==default)
                this.Width = sceneObject.Width;

            if (this.Height==default)
                this.Height = sceneObject.Height;

            if (this.Opacity==default)
                this.Opacity = -1;

            if (this.Size==default)
                this.Size=5;

            if (this.ImagesPath==default)
                this.ImagesPath="UI/border/";

            return this;
        }
    }
}
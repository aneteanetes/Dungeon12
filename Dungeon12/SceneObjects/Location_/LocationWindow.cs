using Dungeon;
using Dungeon.Control;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.Entities.Map;
using Dungeon12.SceneObjects.Base;

namespace Dungeon12.SceneObjects.Location_
{
    public class LocationWindow : SceneControl<MapPoint>, IAutoUnfreeze
    {
        public LocationWindow(MapPoint component) : base(component)
        {
            DungeonGlobal.Freezer.Freeze(this);

            Left = 475;
            Top=35;

            Width = 970;
            Height = 790;
            Image = "UI/Windows/Location/location.png".AsmImg();

            if (component.Closeable)
                AddChild(new Close(this));

            foreach (var obj in component.Objects)
            {
                this.AddChild(new LocationObject(obj));
            }

            this.AddChild(new Title(component.Name)
            {
                Left=40,
                Top=24
            });

            this.AddChild(new Description(component.Description)
            {
                Left=40,
                Top=90
            });
        }

        private class Description : EmptySceneObject
        {
            public Description(string desc)
            {
                this.Width=890;
                this.Height=63;

                this.AddTextCenter(desc.SegoeUI().InSize(13).InColor(Global.CommonColorLight).WithWordWrap());
            }
        }

        private class Close : EmptySceneControl, ITooltiped
        {
            public IDrawText TooltipText { get; set; }

            public bool ShowTooltip => true;

            private LocationWindow _locationWindow;

            public Close(LocationWindow locationWindow)
            {
                _locationWindow = locationWindow;

                TooltipText = "Закрыть".AsDrawText();

                Width = 60;
                Height = 60;

                Left = 880;
                Top = 18;

                Image = "UI/Windows/Location/framex.png".AsmImg();
            }

            public override void Click(PointerArgs args)
            {
                _locationWindow.Destroy();
                base.Click(args);
            }
        }

        //private class ObjectScene : SceneControl<MapObject>, ITooltiped
        //{
        //    ImageObject _border;

        //    public ObjectScene(MapObject component, MapObjectInfo info) : base(component)
        //    {
        //        this.Width = 72;
        //        this.Height = 72;

        //        this.Left = info.X * 76;
        //        this.Top = info.Y * 76;

        //        this.Image = $"UI/layout/location/b.png".AsmImg();
        //        this.AddChild(new ImageObject($"Icons/{component.Icon}.png"));
        //        _border = this.AddChild(new ImageObject($"UI/layout/location/r.png"));

        //        TooltipText = component.Name.AsDrawText();
        //    }

        //    public IDrawText TooltipText { get; set; }

        //    public bool ShowTooltip => true;

        //    public override void Focus()
        //    {
        //        _border.Image = $"UI/layout/location/a.png".AsmImg();
        //        base.Focus();
        //    }

        //    public override void Unfocus()
        //    {
        //        _border.Image = $"UI/layout/location/r.png".AsmImg();
        //        base.Unfocus();
        //    }
        //}
    }
}
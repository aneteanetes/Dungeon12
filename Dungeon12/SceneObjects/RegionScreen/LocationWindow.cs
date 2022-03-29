using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing.SceneObjects;
using Dungeon.Resources;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.Entities.Map;
using Dungeon12.Entities.Objects;

namespace Dungeon12.SceneObjects.RegionScreen
{
    public class LocationWindow : SceneControl<MapPoint>, IAutoUnfreeze
    {
        public LocationWindow(MapPoint component) : base(component)
        {
            Global.Freezer.Freeze(this);

            this.Width = 430;
            this.Height = 460;
            this.Image = "UI/layout/location/back.png".AsmImg();

            var container = this.AddChild(new EmptySceneObject()
            {
                Left = 56,
                Top = 6,
                Width = 318,
                Height = 45
            });

            container.AddTextCenter(component.Name.AsDrawText().Gabriela().InSize(15).InColor(Global.CommonColor));

            this.AddChild(new ImageObject("UI/layout/location/grid.png")
            {
                Width = 376,
                Height = 376,
                Left = 27,
                Top = 63
            });

            this.AddChild(new ImageObject("UI/layout/location/grida.png")
            {
                Width = 376,
                Height = 376,
                Left = 27,
                Top = 63
            });

            foreach (var info in component.Objects)
            {
                var mapobj = ResourceLoader.LoadJson<MapObject>($"Objects/{info.Id}.json");
                this.AddChild(new ObjectScene(mapobj, info));
            }

            if (component.Closeable)
                this.AddChild(new Close(this));
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

                this.Width = 29;
                this.Height = 29;

                this.Left = 399;
                this.Top = 29;

                this.Image = "UI/layout/location/x.png".AsmImg();
            }

            public override void Focus()
            {
                this.Image = "UI/layout/location/xa.png".AsmImg();
                base.Focus();
            }

            public override void Unfocus()
            {
                this.Image = "UI/layout/location/x.png".AsmImg();
                base.Unfocus();
            }

            public override void Click(PointerArgs args)
            {
                _locationWindow.Destroy();
                base.Click(args);
            }
        }

        private class ObjectScene : SceneControl<MapObject>, ITooltiped
        {
            ImageObject _border;

            public ObjectScene(MapObject component, MapObjectInfo info) : base(component)
            {
                this.Width = 72;
                this.Height = 72;

                this.Left = info.X * 76;
                this.Top = info.Y * 76;

                this.Image = $"UI/layout/location/b.png".AsmImg();
                this.AddChild(new ImageObject($"Icons/{component.Icon}.png"));
                _border = this.AddChild(new ImageObject($"UI/layout/location/r.png"));

                TooltipText = component.Name.AsDrawText();
            }

            public IDrawText TooltipText { get; set; }

            public bool ShowTooltip => true;

            public override void Focus()
            {
                _border.Image = $"UI/layout/location/a.png".AsmImg();
                base.Focus();
            }

            public override void Unfocus()
            {
                _border.Image = $"UI/layout/location/r.png".AsmImg();
                base.Unfocus();
            }
        }
    }
}
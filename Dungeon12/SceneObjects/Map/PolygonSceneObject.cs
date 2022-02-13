using Dungeon;
using Dungeon.Control;
using Dungeon.Control.Keys;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.Entities.Map;
using Dungeon12.SceneObjects.Base;
using System;

namespace Dungeon12.SceneObjects.Map
{
    public class PolygonSceneObject : SceneControl<Polygon>, ITooltiped
    {
        public override bool AbsolutePosition => true;

        ImageObject blackmask;

        private class Icon : SceneObject<Polygon>
        {
            public Icon(Polygon polygon) : base(polygon)
            {
                this.Width = 210;
                this.Height = 210;
            }

            public override bool Visible => Component.Icon.IsNotEmpty();

            public override string Image => $"Icons/{Component.Icon}".AsmImg();
        }

        public PolygonSceneObject(Polygon polygon, string defaultBackground, int index) : base(polygon, true)
        {
            this.Width = 210;
            this.Height = 210;

            var x = 0d;
            var y = 0;

            switch (index)
            {
                case 0:
                    x = 259;
                    y = -10;
                    break;
                case 1:
                    x = 353;
                    y = 157;
                    break;
                case 2:
                    x = 261;
                    y = 321;
                    break;
                case 3:
                    x = 76;
                    y = 318;
                    break;
                case 4:
                    x = -17;
                    y = 157;
                    break;
                case 5:
                    x = 74;
                    y = -10;
                    break;
                default:
                    break; //throw new ArgumentException("Only 0-5 indexes available!");
            }

            this.Left = x;
            this.Top = y;

            this.AddChild(new ImageObject(defaultBackground.AsmRes()));

            this.AddChild(blackmask = new ImageObject("Tiles/blackmask.png".AsmImg())
            {
                Opacity = .3,
                Visible = false
            });

            this.AddChildCenter(new Icon(polygon));
        }

        public override bool AllKeysHandle => true;

        public override void Focus()
        {
            blackmask.Visible = true;
        }

        public override void Unfocus()
        {
            blackmask.Visible = false;
        }

        public override void Click(PointerArgs args)
        {
            if (!string.IsNullOrWhiteSpace(Component.Function))
            {
                Global.Game.Polygon = Component;
                Global.ExecuteFunction(this.Layer, Component.Function, Component.ObjectId);
            }
            base.Click(args);
        }

        public void RefreshTooltip() { }

        private IDrawText tooltiptext;
        public IDrawText TooltipText
        {
            get
            {
                if (tooltiptext == null)
                {
                    tooltiptext = Component.Name.AsDrawText().Gabriela().InSize(12);
                }

                return tooltiptext;
            }
        }

        public bool ShowTooltip => Component != null && Component.IsNotEmpty;

        public Tooltip CustomTooltipObject => null;
    }
}
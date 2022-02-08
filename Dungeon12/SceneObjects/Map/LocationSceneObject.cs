using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.Entities.Map;
using Dungeon12.SceneObjects.Base;

namespace Dungeon12.SceneObjects.Map
{
    public class LocationSceneObject : SceneControl<Location>, ITooltiped
    {
        private ImageObject Background;

        private ImageObject Object;

        private ImageObject Character;

        private ImageObject Fog;

        private ImageObject Selection;

        public LocationSceneObject(Location location) : base(location, true)
        {
            Width = 126;
            Height = 126;

            Left = location.Position.X;
            Top = location.Position.Y;

            CacheAvailable = false;

            var icon = location.Polygon == null || location.Polygon.Icon == null
                ? location.BackgroundImage.AsmRes()
                : $"Icons/{location.Polygon.Icon}".AsmImg();

            Background = new ImageObject(icon)
            {
                Width = 126,
                Height = 126
            };

            //Object = new ImageObject(location.ObjectImage.AsmRes())
            //{
            //    Width = location.Size.X,
            //    Height = location.Size.Y,
            //    CacheAvailable = false
            //};

            Fog = new ImageObject("Tiles/fog.png".AsmImg())
            {
                Width = 160,
                Height = 160,
                Left = -20,
                Top = -20,
                CacheAvailable = false
            };

            Selection = new ImageObject("Tiles/empty_invert.png".AsmImg())
            {
                Width = 126,
                Height = 126,
                Visible = false
            };

            this.AddChild(Background);
            this.AddChild(Selection);
            //this.AddChild(Object);

            if (!location.IsOpen)
                this.AddChild(Fog);
        }

        public override void UpdateSceneObject(GameTimeLoop gameTime)
        {
            if (Component.IsOpen)
                this.Fog.Visible = false;
            base.UpdateSceneObject(gameTime);
        }

        private IDrawText tooltiptext;
        public IDrawText TooltipText
        {
            get
            {
                if (tooltiptext == null)
                {
                    tooltiptext = "Стол с бумагами".AsDrawText().Gabriela().InSize(12);
                }

                return tooltiptext;
            }
        }

        public bool ShowTooltip => Component.IsOpen;

        public Tooltip CustomTooltipObject => null;

        public override void Focus()
        {
            if (Component.IsOpen)
            {
                Selection.Visible = true;
                if (Global.Helps.IsEnabled)
                    Global.Helps.StepClick();
            }
        }

        private ExploreSceneObject exploreSceneObject;

        public override void Click(PointerArgs args)
        {
            if (Component.IsOpen && Component.IsActivable)
            {
                if (Global.Helps.IsEnabled)
                {
                    Global.Helps.StepActivate();
                }

                this.Layer.AddObject(exploreSceneObject = new ExploreSceneObject(this.Component));
                //Global.Freezer.World = exploreSceneObject;
#warning commented freeze
            }
        }

        public override void Unfocus()
        {
            if (Component.IsOpen)
            {
                Global.Helps.StepFocus();
                Selection.Visible = false;
            }
        }

        public void RefreshTooltip() { }
    }
}

using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.View.Interfaces;
using Dungeon12.ECS.Components;
using Dungeon12.Entities.Map;

namespace Dungeon12.SceneObjects.Map
{
    public class LocationSceneObject : SceneControl<Location>, ITooltiped
    {
        private ImageObject Background;

        private ImageObject Object;

        private ImageObject Character;

        private ImageObject Fog;

        private ImageObject Selection;

        HintScenarioSceneObject Hints;

        public LocationSceneObject(Location location, HintScenarioSceneObject hints) : base(location, true)
        {
            Hints = hints;

            Width = location.Size.X;
            Height = location.Size.Y;
            Left = location.Position.X;
            Top = location.Position.Y;

            CacheAvailable = false;

            Background = new ImageObject(location.BackgroundImage.AsmRes())
            {
                Width = location.Size.X,
                Height = location.Size.Y,
                CacheAvailable = false
            };

            Object = new ImageObject(location.ObjectImage.AsmRes())
            {
                Width = location.Size.X,
                Height = location.Size.Y,
                CacheAvailable = false
            };

            Fog = new ImageObject("Tiles/fog.png".AsmImg())
            {
                Width = 300,
                Height = 300,
                Left = -45,
                Top = -45,
                CacheAvailable = false
            };

            Selection = new ImageObject("Tiles/empty_invert.png".AsmImg())
            {
                Width = location.Size.X,
                Height = location.Size.Y,
                Visible = false
            };

            this.AddChild(Background);
            this.AddChild(Selection);
            this.AddChild(Object);

            if (!location.IsOpen)
                this.AddChild(Fog);
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

        public override void Focus()
        {
            if (Component.IsOpen)
            {
                Selection.Visible = true;
                Hints.StepClick();
            }
        }

        private ExploreSceneObject exploreSceneObject;

        public override void Click(PointerArgs args)
        {
            if (Component.IsOpen)
            {
                if (Hints.IsEnabled)
                {
                    Hints.StepActivate();
                }

                this.Layer.AddObject(exploreSceneObject = new ExploreSceneObject(this.Component, Hints));
                Global.Freezer.World = exploreSceneObject;
            }
        }

        public override void Unfocus()
        {
            if (Component.IsOpen)
            {
                Hints.StepFocus();
                Selection.Visible = false;
            }
        }
    }
}

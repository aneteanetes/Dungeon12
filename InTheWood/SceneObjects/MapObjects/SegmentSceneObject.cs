using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.Types;
using InTheWood.Entities.MapScreen;
using System.Linq;

namespace InTheWood.SceneObjects.MapObjects
{
    public class SegmentSceneObject : ControlSceneObject<Segment>
    {
        public const double TilesetOffset = 2;
        public const double TileWidth = 120;
        public const double TileHeight = 140;

        ImageControl mask;
        LightObject light;
        MapSceneObject map;

        public override bool CacheAvailable => false;

        public SegmentSceneObject(Segment component, MapSceneObject map=default) : base(component, true)
        {
            this.map = map;
            this.Width = TileWidth;
            this.Height = TileHeight;
            mask = new ImageControl("Sprites/segmentselect.png".AsmRes());
            mask.Visible = false;
            //this.AddChild(new ImageControl("Sprites/bordersegment.png".AsmRes()));
            //this.AddChild(mask);

            this.light = new LightObject();
            this.light.Visible = false;
            this.AddChild(light);
        }

        private class LightObject : EmptySceneObject
        {
            public LightObject()
            {
                this.Width = TileWidth;
                this.Height = TileHeight;

                this.Light = new Light()
                {
                    Color = DrawColor.WhiteSmoke,
                    Range = 250,
                    //Type= Dungeon.View.Interfaces.LightType.Texture,
                    //Image="Sprites/customlight.png".AsmRes()
                };
            }
        }

        public override void Click(PointerArgs args)
        {
            if (args.MouseButton == Dungeon.Control.Pointer.MouseButton.Left)
            {
                if (MapStage != MapStage.Gameplay && !Component.IsEdge)
                    return;
                    
                if (MapStage== MapStage.Gameplay && map.Component.GetNeighbors(Component).Count(s => s.Status == MapStatus.Friendly) < 2)
                    return;

                Component.Status = MapStatus.Friendly;

                mask.Visible = false;
                Component.Immune = true;
                this.map.Component.Turn();
                this.light.Visible = !this.light.Visible;
            }
            base.Click(args);
        }

        public override void Focus()
        {
            if (IsNotAvailableOnFirstStep)
                return;
            mask.Visible = true;
            base.Focus();
        }

        private MapStage MapStage => map.Component.Stage;

        private bool IsNotAvailableOnFirstStep => MapStage == MapStage.Initial && !Component.IsEdge;

        public override void Unfocus()
        {
            mask.Visible = false;
            base.Unfocus();
        }

        public override string Image => "Images/hexes.png".AsmRes();
            //"Sprites/hexagonTerrain_sheet.png".AsmRes();

        public override Rectangle ImageRegion
        {
            get
            {
                var rect = new Rectangle(0, 0, TileWidth, TileHeight);

                switch (Component.Status)
                {
                    case MapStatus.Neutral:
                        rect.X = 120;
                        //rect.X = 0 * (TileWidth + TilesetOffset);
                        //rect.Y = 12 * (TileHeight + TilesetOffset);
                        break;
                    case MapStatus.Friendly:
                        rect.X = 240;
                        //rect.X = 5 * (TileWidth + TilesetOffset);
                        //rect.Y = 1 * (TileHeight + TilesetOffset);
                        break;
                    case MapStatus.Hostile:
                        //rect.X = 3 * (TileWidth + TilesetOffset);
                        //rect.Y = 8 * (TileHeight + TilesetOffset);
                        break;
                    default:
                        break;
                }

                return rect;
            }
        }
    }
}
using Dungeon;
using Dungeon.Control;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon.Types;
using InTheWood.Entities.MapScreen;

namespace InTheWood.SceneObjects.MapObjects
{
    public class SegmentSceneObject : ControlSceneObject<Segment>
    {
        public const double TilesetOffset = 2;
        public const double TileWidth = 120;
        public const double TileHeight = 140;

        ImageControl mask;

        public SegmentSceneObject(Segment component) : base(component, true)
        {
            this.Width = TileWidth;
            this.Height = TileHeight;
            mask = new ImageControl("Sprites/segmentselect.png".AsmRes());
            mask.Visible = false;
            this.AddChild(new ImageControl("Sprites/bordersegment.png".AsmRes()));
            this.AddChild(mask);
        }

        public override void Click(PointerArgs args)
        {
            base.Click(args);
        }

        public override void Focus()
        {
            mask.Visible = true;
            base.Focus();
        }

        public override void Unfocus()
        {
            mask.Visible = false;
            base.Unfocus();
        }

        public override string Image => "Sprites/hexagonTerrain_sheet.png".AsmRes();

        public override Rectangle ImageRegion
        {
            get
            {
                var rect = new Rectangle(0, 0, TileWidth, TileHeight);

                switch (Component.Status)
                {
                    case MapStatus.Neutral:
                        rect.X = 2 * (TileWidth + TilesetOffset);
                        rect.Y = 3 * (TileHeight + TilesetOffset);
                        break;
                    case MapStatus.Friendly:
                        rect.X = 5 * (TileWidth + TilesetOffset);
                        rect.Y = 1 * (TileHeight + TilesetOffset);
                        break;
                    case MapStatus.Hostile:
                        rect.X = 3 * (TileWidth + TilesetOffset);
                        rect.Y = 8 * (TileHeight + TilesetOffset);
                        break;
                    default:
                        break;
                }

                return rect;
            }
        }
    }
}
using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using InTheWood.Entities.MapScreen;

namespace InTheWood.SceneObjects.MapObjects
{
    public class SectorSceneObject : ControlSceneObject<Sector>
    {
        public SectorSceneObject(Sector component) : base(component, true)
        {
            this.Width = SegmentSceneObject.TileWidth * 3;
            this.Height = SegmentSceneObject.TileHeight * 3;

            this.InitSegments();

            this.AddChild(new ImageControl("Sprites/border.png".AsmRes()));
        }

        private void InitSegments()
        {
            var oneHeightPosition = SegmentSceneObject.TileHeight * 0.75;
            var oneWithHalf = SegmentSceneObject.TileWidth / 2;

            this.AddChild(new SegmentSceneObject(Component.Segments[0])
            {
                Left = SegmentSceneObject.TileWidth,
                Top = oneHeightPosition
            });

            this.AddChild(new SegmentSceneObject(Component.Segments[1])
            {
                Left = oneWithHalf
            });

            this.AddChild(new SegmentSceneObject(Component.Segments[2])
            {
                Left = SegmentSceneObject.TileWidth + oneWithHalf
            });

            this.AddChild(new SegmentSceneObject(Component.Segments[3])
            {
                Left = SegmentSceneObject.TileWidth *2,
                Top= oneHeightPosition
            });

            this.AddChild(new SegmentSceneObject(Component.Segments[4])
            {
                Left = SegmentSceneObject.TileWidth + oneWithHalf,
                Top = oneHeightPosition * 2
            });

            this.AddChild(new SegmentSceneObject(Component.Segments[5])
            {
                Left = oneWithHalf,
                Top = oneHeightPosition * 2
            });

            this.AddChild(new SegmentSceneObject(Component.Segments[6])
            {
                Top = oneHeightPosition
            });
        }
    }
}
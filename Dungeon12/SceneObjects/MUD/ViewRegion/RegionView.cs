using Dungeon12.Entities.Map;

namespace Dungeon12.SceneObjects.MUD.ViewRegion
{
    internal class RegionView : SceneControl<Region>
    {
        public RegionView(Region region) : base(region)
        {
            Width = 400;
            Height = 400;

            this.AddBorder();

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    this.AddChild(new RegionViewTile(new Location() { Name=Guid.NewGuid().ToString() })
                    {
                        Left= x*50 +12,
                        Top= y*50 +12,
                    });
                }
            }
        }
    }
}
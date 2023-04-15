using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon12.Entities.Map;
using Dungeon12.SceneObjects.MUD.ViewRegion;
using Dungeon12.SceneObjects.RegionScreen;

namespace Dungeon12.SceneObjects.MUD
{
    internal class LocationPreviewImg : SceneObject<Location>
    {
        public override void Throw(Exception ex)
        {
            throw ex;
        }

        public LocationPreviewImg(Location component) : base(component)
        {
            this.Width=400;
            this.Height=400;

            this.AddBorderBack();

            this.AddChild(new ImageObject($"Locations/{component.Region.MapId}/{component.ObjectImage}")
            {
                Left=5,
                Top=5,
                Width=390,
                Height=390
            });

            this.AddChild(new AreaTitle(component.Name));
        }
    }
}

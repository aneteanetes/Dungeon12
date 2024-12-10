using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Nabunassar.Entities.Map;
using Nabunassar.SceneObjects.MUD.ViewRegion;
using Nabunassar.SceneObjects.RegionScreen;

namespace Nabunassar.SceneObjects.MUD
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

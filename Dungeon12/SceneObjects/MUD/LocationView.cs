using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon12.Entities.Map;
using Dungeon12.SceneObjects.MUD.ViewRegion;

namespace Dungeon12.SceneObjects.MUD
{
    internal class LocationView : SceneObject<Location>
    {
        public LocationView(Location component) : base(component)
        {
            this.Width=400;
            this.Height=400;

            this.AddBorder();

            this.AddChild(new ImageObject($"Locations/{component.Region.MapId}/{component.ObjectImage}")
            {
                Left=5,
                Top=5,
                Width=390,
                Height=390
            });
        }
    }
}

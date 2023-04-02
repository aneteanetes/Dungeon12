using Dungeon;
using Dungeon12.Entities.Map;
using Dungeon12.SceneObjects.RegionScreen;

namespace Dungeon12.SceneObjects.MUD
{
    internal class LocationPanel : SceneControl<Location>
    {
        public override void Throw(Exception ex)
        {
            throw ex;
        }

        public LocationPanel(Location location) :base(location)
        {
            this.Width=1120;
            this.Height=800;

            this.Image = $"Backgrounds/Regions/"+location.BackgroundImage;

            this.AddBorder();

            this.AddChildCenter(new LocationTitle(location), vertical: false);
        }
    }
}

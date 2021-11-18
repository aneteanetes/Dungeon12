using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon12.Entities.Map;

namespace Dungeon12.SceneObjects.Map
{
    public class LocationSceneObject : SceneObject<Location>
    {
        private ImageObject Background;

        private ImageObject Object;

        private ImageObject Character;

        private ImageObject Fog;

        public LocationSceneObject(Location location) : base(location, true)
        {
            Width = location.Size.X;
            Height = location.Size.Y;
            CacheAvailable = false;

            Background = new ImageObject(location.Background.AsmRes())
            {
                Width = location.Size.X,
                Height = location.Size.Y,
                Left = location.Position.X,
                Top = location.Position.Y,
                CacheAvailable = false
            };

            Object = new ImageObject(location.Object.AsmRes())
            {
                Width = location.Size.X,
                Height = location.Size.Y,
                Left = location.Position.X,
                Top = location.Position.Y,
                CacheAvailable = false
            };

            Fog = new ImageObject("Tiles/fog.png".AsmImg())
            {
                Width = 300,
                Height = 300,
                Left = location.Position.X - 45,
                Top = location.Position.Y - 45,
                CacheAvailable = false
            };

            this.AddChild(Background);
            this.AddChild(Object);

            if (!location.IsOpen)
                this.AddChild(Fog);
        }
    }
}

using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon12.Entities.Map;

namespace Dungeon12.SceneObjects.Map
{
    public class RegionSceneObject : SceneControl<Region>
    {
        public RegionSceneObject(Region component) : base(component, true)
        {
            Width = component.Size.X;
            Height = component.Size.Y;
            CacheAvailable = false;

            foreach (var line in component.Lines)
            {
                this.AddChild(new ImageObject("Backgrounds/line.png".AsmImg())
                {
                    Width = 210,
                    Height = 210,
                    Left = line.X,
                    Top = line.Y,
                    CacheAvailable=false
                });
            }

            foreach (var location in component.Locations)
            {
                var locsceneobj = new LocationSceneObject(location);
                this.AddChild(locsceneobj);
            }
        }
    }
}

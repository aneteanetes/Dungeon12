using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon12.Entities.Map;

namespace Dungeon12.SceneObjects.Map
{
    public class StaticRegionSceneObject : SceneControl<Region>
    {
        public StaticRegionSceneObject(Region component) : base(component, true)
        {
            Width = 1085;
            Height = 826;
            CacheAvailable = false;

            foreach (var line in component.Lines)
            {
                this.AddChild(new ImageObject("Backgrounds/line.png".AsmImg())
                {
                    Width = 126,
                    Height = 126,
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

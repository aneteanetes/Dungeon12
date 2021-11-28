namespace Dungeon12.SceneObjects.UserInterface
{
    using Dungeon;
    using Dungeon.Drawing.SceneObjects;
    using Dungeon.Resources;
    using Dungeon.SceneObjects;
    using Dungeon12.Entities;
    using Dungeon12.Entities.Zones;
    using Dungeon12.SceneObjects.UserInterface.Common;
    using Dungeon12.SceneObjects.UserInterface.OriginSelect;
    using System.Linq;

    public class OriginSelectSceneObject : SceneControl<Hero>
    {
        public OriginSelectSceneObject(Hero component):base(component)
        {
            this.Width = Global.Resolution.Width;
            this.Height = Global.Resolution.Height;

            var back = this.AddChildCenter(new ImageObject("Maps/mainlandback.png".AsmImg())
            {
                Width=1000,
                Height=629
            });
            var map = this.AddChildCenter(new ImageObject("Maps/mapbord.png".AsmImg())
            {
                Width = 1058,
                Height = 684
            });

            var desc = new AreaDescription()
            {
                Left = map.Left + 768,
                Top = map.Top + 227
            };

            var zones = typeof(Zones).All<Zones>().Select(z => ResourceLoader.LoadJson<Zone>($"Objects/MapZones/{z}.json".AsmRes()));

            foreach (var zone in zones)
            {
                this.AddChildCenter(new AreaObj(zone,desc));
            }

            desc.Refresh(zones.FirstOrDefault(z => z.ObjectId == "fi"));
            this.AddChild(desc);

            void Close()
            {
                this.Destroy?.Invoke();
            }

            this.AddChild(new MapCloseButton()
            {
                Left = back.Left + back.Width - 20,
                Top = 70,
                OnClick= Close
            });
        }
    }
}
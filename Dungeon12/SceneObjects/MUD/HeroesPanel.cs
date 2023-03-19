using Dungeon.SceneObjects;
using Dungeon12.Entities;
using Dungeon12.SceneObjects.HeroPanelObjs;

namespace Dungeon12.SceneObjects.MUD
{
    internal class HeroesPanel : SceneControl<Party>
    {
        public HeroesPanel(Party component) : base(component, true)
        {
            this.Width=1520;
            this.Height=250;

            this.AddBorder();

            var leftOffset = 15d;
            var topOffset = 10;

            var p1 = this.AddChild(new HeroPanel(component.Hero1)
            {
                Left=leftOffset,
                Top=topOffset,
            });

            //var p2 = this.AddChild(new HeroPanel(component.Hero2)
            //{
            //    Left = p1.LeftMax+leftOffset,
            //    Top=topOffset,
            //});

            //var p3 = this.AddChild(new HeroPanel(component.Hero3)
            //{
            //    Left = p2.LeftMax+leftOffset,
            //    Top=topOffset,
            //});

            //var p4 = this.AddChild(new HeroPanel(component.Hero4)
            //{
            //    Left = p3.LeftMax+leftOffset,
            //    Top=topOffset,
            //});
        }
    }
}

using Dungeon.Control;
using Dungeon.SceneObjects;
using Nabunassar.Entities;
using Nabunassar.SceneObjects.HeroPanelObjs;

namespace Nabunassar.SceneObjects.MUD
{
    internal class HeroesPanel : SceneControl<Party>
    {
        public override void Throw(Exception ex)
        {
            throw ex;
        }

        public HeroesPanel(Party component) : base(component)
        {
            this.Width=1520;
            this.Height=250;

            this.AddBorderBack();

            var leftOffset = 15d;
            var topOffset = 12.5;

            var p1 = this.AddChild(new HeroPanel(component.First,true)
            {
                Left=leftOffset,
                Top=topOffset,
            });

            var p2 = this.AddChild(new HeroPanel(component.Second, true)
            {
                Left = p1.LeftMax+leftOffset,
                Top=topOffset,
            });

            var p3 = this.AddChild(new HeroPanel(component.Third, true)
            {
                Left = p2.LeftMax+leftOffset,
                Top=topOffset,
            });

            var p4 = this.AddChild(new HeroPanel(component.Fourth, true)
            {
                Left = p3.LeftMax+leftOffset,
                Top=topOffset,
            });
        }

        public override void Click(PointerArgs args)
        {
        }
    }
}

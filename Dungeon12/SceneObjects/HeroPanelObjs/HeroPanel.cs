using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon12.Entities;

namespace Dungeon12.SceneObjects.HeroPanelObjs
{
    internal class HeroPanel : SceneControl<Hero>
    {
        public HeroPanel(Hero component, bool isPeaceful=false) : base(component)
        {
            this.Width = 360;
            this.Height = 230;

            this.Image = "UI/char/back.png".AsmImg();

            this.AddChild(new ImageObject(component.Avatar)
            {
                Width = 87,
                Height = 131,
                Top = 95,
                Left = 4
            });

            var name = this.AddTextCenter(component.Name.AsDrawText().Gabriela().InColor(Global.CommonColor).InSize(20));
            name.Left = 105;
            name.Top = 95;

            var abilLeft = 0;

            this.AddChild(new ValueBar(component, true)
            {
                Left=110,
                Top=168
            });
            this.AddChild(new ValueBar(component, false)
            {
                Left=110,
                Top=197
            });

            int i = 1;

            foreach (var abil in component.Abilities)
            {
                this.AddChild(new AbilityItemBig(Component, abil, isPeaceful ? i : 0)
                {
                    Left = abilLeft
                });                

                abilLeft += 92;
                i++;
            }
        }

        public override void Drawing()
        {
            base.Drawing();
        }
    }
}

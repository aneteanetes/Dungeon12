using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon12.Entities;

namespace Dungeon12.SceneObjects.HeroPanelObjs
{
    public class HeroPanel : SceneControl<Hero>
    {
        public HeroPanel(Hero component) : base(component)
        {
            this.Width = 254;
            this.Height = 165;

            this.Image = "UI/char/back.png".AsmImg();

            this.AddChild(new ImageObject(component.Avatar)
            {
                Width = 61,
                Height = 92,
                Top = 69,
                Left = 3
            });

            var name = this.AddTextCenter(component.Name.AsDrawText().Gabriela().InColor(Global.CommonColor).InSize(14));
            name.Left = 76;
            name.Top = 67;

            var abilLeft = 0;

            foreach (var abil in component.Abilities)
            {
                this.AddChild(new AbilityItem(Component, abil)
                {
                    Left = abilLeft
                });

                abilLeft += 65;
            }
        }
    }
}

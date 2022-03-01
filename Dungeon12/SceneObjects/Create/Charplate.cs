using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon12.Entities;

namespace Dungeon12.SceneObjects.Create
{
    public class Charplate : SceneControl<Hero>
    {
        public Charplate(Hero component) : base(component)
        {
            Width = 400;
            Height = 750;
            this.Image = "UI/start/back.png".AsmImg();

            this.AddChild(new ImageObject("UI/start/class.png")
            {
                Left = 20,
                Top = 27
            });

            this.AddChild(new ImageObject("UI/start/nameback.png")
            {
                Left = 21,
                Top = 234
            });

            this.AddChild(new ImageObject("UI/start/skills.png")
            {
                Left = 23,
                Top = 288
            });

            this.AddChild(new ImageObject("UI/start/abils.png")
            {
                Left = 20,
                Top = 546
            });

            this.AddChild(new ClassButton(Component, Entities.Enums.Archetype.Warrior)
            {
                Left = 226,
                Top = 57
            });

            this.AddChild(new ClassButton(Component, Entities.Enums.Archetype.Mage)
            {
                Left = 302,
                Top = 57
            });

            this.AddChild(new ClassButton(Component, Entities.Enums.Archetype.Thief)
            {
                Left = 226,
                Top = 134
            });

            this.AddChild(new ClassButton(Component, Entities.Enums.Archetype.Priest)
            {
                Left = 302,
                Top = 134
            });
        }
    }
}

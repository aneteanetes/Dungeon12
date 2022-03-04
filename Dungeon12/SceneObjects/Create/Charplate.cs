using Dungeon;
using Dungeon.Drawing.SceneObjects;
using Dungeon.SceneObjects;
using Dungeon12.Entities;
using Dungeon12.Entities.Enums;

namespace Dungeon12.SceneObjects.Create
{
    public class Charplate : SceneControl<Hero>
    {
        TextInputControl textInput;

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

            Component.Sex = Component.Class.Sex(1);

            this.AddChild(new AvatarSelector(Component)
            {
                Left = 40,
                Top = 45
            });

            this.AddChild(new ImageObject("UI/start/nameback.png")
            {
                Left = 21,
                Top = 234
            });
            textInput = this.AddChild(
                new TextInputControl(
                    " ".AsDrawText().InSize(16).Gabriela().InColor(Global.CommonColor), 
                    20,
                    true, 
                    autofocus: false,
                    invisibleBack:true,
                    placeholder: Global.Strings.EnterCharacterName.AsDrawText().InSize(16).Gabriela().InColor(Global.CommonColor),
                    carrige:true));

            textInput.OnEnter += value =>
            {
                if(value.IsNotEmpty())
                {
                    Component.Name = value;
                }
            };

            textInput.Top = 240;
            textInput.Left = 50;

            this.AddChild(new ImageObject("UI/start/skills.png")
            {
                Left = 23,
                Top = 288
            });

            this.AddChild(new SkillsList(Component)
            {
                Top = 288,
                Left = 80
            });

            this.AddChild(new ImageObject("UI/start/abils.png")
            {
                Left = 20,
                Top = 546
            });

            this.AddChild(new Abilities(Component)
            {
                Left = 40,
                Top = 600
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

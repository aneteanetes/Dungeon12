using Dungeon;
using Dungeon.SceneObjects;
using Dungeon12.Entities;
using Dungeon12.SceneObjects.Map;
using Dungeon12.SceneObjects.UserInterface.CraftSelect;

namespace Dungeon12.SceneObjects.UserInterface.ConfirmHero
{
    public class ConfirmHeroSceneObject : SceneControl<Hero>
    {
        public ConfirmHeroSceneObject(Hero component) : base(component)
        {
            Global.Freezer.Freeze(this);
            this.Destroy += () => Global.Freezer.Unfreeze();

            this.Width = 500;
            this.Height = 300;
            this.Image = "Other/confirmwin.png".AsmImg();

            var titletext = "Подтверждение создания персонажа"
                .AsDrawText()
                .Gabriela()
                .InSize(20)
                .InColor(System.ConsoleColor.White);

            var title = this.AddTextCenter(titletext, false);
            title.Top = 25;

            var descText = "Вы уверены что готовы завершить создание персонажа?\r\nВы не сможете изменить свой выбор в дальнейшем."
                .AsDrawText()
                .Gabriela()
                .InSize(15)
                .WithWordWrap()
                .InColor(System.ConsoleColor.White);

            var description = this.AddTextCenter(descText, true, false);
            description.Top = 105;

            this.AddChild(new CraftOptButton()
            {
                Left = 104,
                Top = 206,
                PerPixelCollision = true,
                OnClick = Confirm
            });

            this.AddChild(new CraftOptButton(true)
            {
                Left = 332,
                Top = 202,
                PerPixelCollision = true,
                OnClick = Close
            });
        }

        private void Confirm()
        {
            this.Destroy();
            Global.Helps.Hide();
            Global.Helps.ChangeState(HintStates.HeroCreated);
            CellsSceneObject.Active.Close();
            Global.Game.Location.IsActivable = false;
            Global.Game.Location.Reveal();
            //Global.Game.HeroPlate1.ReLoad(Global.Game.Party.Hero1);
        }

        private void Close()
        {
            this.Destroy();
        }
    }
}

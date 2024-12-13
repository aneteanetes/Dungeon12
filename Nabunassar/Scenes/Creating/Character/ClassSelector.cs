using Dungeon.Scenes;
using Nabunassar.Entities;
using Nabunassar.Entities.Characters;
using Nabunassar.Entities.Enums;
using Nabunassar.SceneObjects.Base;
using Nabunassar.SceneObjects.UserInterface.Common;

namespace Nabunassar.Scenes.Creating.Character
{
    internal class ClassSelector : CreatePart
    {
        public ClassSelector(Hero component) : base(component)
        {
            Width = 325;
            Height = 700;
            Top = 300;
            Left = 50;

            this.AddBorderMapBack(new BorderConfiguration()
            {
                ImagesPath = "UI/bordermin/panel-border-022.png",
                Size = 16,
                Padding = 2
            });

            var title = AddTextCenter(Global.Strings["ClassChoose"].ToString().DefaultTxt(20));

            title.Top = 20;

            var top = 65;

            typeof(Archetype).All<Archetype>().ForEach(arch =>
            {
                var clsBtn = new ClassicButton(Global.Strings[arch.ToString()], 200, 40, 18)
                {
                    Top = top,
                    Disabled = false,
                    OnClick = () =>
                    {
                        component.Archetype = arch;
                        Global.Game.Creation.Hint = Global.Strings["Guide"][arch.ToString()];
                    }
                };

                AddChildCenter(clsBtn);
                clsBtn.Top = top;
                top += 52;
            });
        }

        public override bool Updatable => true;

        public override void Update(GameTimeLoop gameTime)
        {
            base.Update(gameTime);
        }
    }
}

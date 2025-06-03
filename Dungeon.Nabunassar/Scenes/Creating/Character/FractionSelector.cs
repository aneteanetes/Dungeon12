using Dungeon.Scenes;
using Nabunassar.Entities;
using Nabunassar.Entities.Characters;
using Nabunassar.Entities.Enums;
using Nabunassar.SceneObjects.Base;
using Nabunassar.SceneObjects.UserInterface.Common;

namespace Nabunassar.Scenes.Creating.Character
{
    internal class FractionSelector : CreatePart
    {
        public FractionSelector(Hero component) : base(component, Global.Strings["guide"]["fraction"])
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

            var title = AddTextCenter(Global.Strings["FractionChoose"].ToString().DefaultTxt(20));

            title.Top = 20;

            var top = 65;

            typeof(Fraction).All<Fraction>().Skip(1).ForEach(frac =>
            {
                var clsBtn = new ClassicButton(Global.Strings[frac.ToString()], 200, 40, 18)
                {
                    Top = top,
                    Disabled = false,
                    OnClick = () =>
                    {
                        this.Cube.Next.Visible = true;
                        component.Fraction = frac;
                        Global.Game.Creation.Hint = Global.Strings["Guide"][frac.ToString()];
                    }
                };

                AddChildCenter(clsBtn);
                clsBtn.Top = top;
                top += 52;
            });
        }
    }
}

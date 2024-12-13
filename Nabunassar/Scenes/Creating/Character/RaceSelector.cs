using Dungeon.Scenes;
using Nabunassar.Entities;
using Nabunassar.Entities.Characters;
using Nabunassar.SceneObjects.Base;
using Nabunassar.SceneObjects.UserInterface.Common;

namespace Nabunassar.Scenes.Creating.Character
{
    internal class RaceSelector : CreatePart
    {
        public RaceSelector(Hero component) : base(component)
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

            var title = AddTextCenter(Global.Strings["RaceChoose"].ToString().DefaultTxt(20));

            title.Top = 20;

            var top = 65;

            typeof(Race).All<Race>().ForEach(race =>
            {
                var raceBtn = new ClassicButton(Global.Strings[race.ToString()], 200, 40, 18)
                {
                    Top = top,
                    Disabled = false,
                    OnClick = () =>
                    {
                        this.Cube.Next.Visible = true;
                        component.Race = race;
                        Global.Game.Creation.Hint = Global.Strings["Guide"][race.ToString()];
                    }
                };

                AddChildCenter(raceBtn);
                raceBtn.Top = top;
                top += 52;
            });
        }
    }
}

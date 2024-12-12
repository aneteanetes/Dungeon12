using Dungeon.Scenes;
using Nabunassar.Entities;
using Nabunassar.Entities.Characters;
using Nabunassar.SceneObjects.Base;
using Nabunassar.SceneObjects.UserInterface.Common;

namespace Nabunassar.Scenes.Creating.Heroes
{
    internal class RaceSelector : SceneControl<Hero>
    {
        public RaceSelector(Hero component) : base(component)
        {
            this.Width = 325;
            this.Height = 700;

            this.AddBorderMapBack(new BorderConfiguration()
            {
                ImagesPath = "UI/bordermin/panel-border-022.png",
                Size = 16,
                Padding = 2
            });

            var title = this.AddTextCenter(Global.Strings["RaceChoose"].ToString().DefaultTxt(20));

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
                        component.Race = race;
                        Global.Game.Creation.Hint = Global.Strings["Guide"][race.ToString()];
                    }
                };

                this.AddChildCenter(raceBtn);
                raceBtn.Top = top;
                top += 52;
            });
        }
    }
}

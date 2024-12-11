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
            this.Width = 300;
            this.Height = 700;

            this.AddBorderMapBack(new BorderConfiguration()
            {
                ImagesPath = "UI/bordermin/panel-border-022.png",
                Size = 16,
                Padding = 2
            });

            var top = 50;

            typeof(Race).All<Race>().ForEach(race =>
            {
                var raceBtn = new ClassicButton(Global.Strings[race.ToString()], 200, 40, 18)
                {
                    Top = top,
                    Disabled = false,
                    OnClick = () =>
                    {
                        component.Race = race;
                    }
                };

                this.AddChildCenter(raceBtn);
                raceBtn.Top = top;
                top += 50;
            });
        }
    }
}

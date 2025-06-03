using Dungeon.SceneObjects;
using Nabunassar.Entities;
using Nabunassar.Entities.Characters;
using System.ComponentModel;

namespace Nabunassar.Scenes.Creating.Character
{
    internal class HeroCreatingBackground : SceneObject<Hero>
    {
        public HeroCreatingBackground(Hero component) : base(component)
        {
            Width = DungeonGlobal.Resolution.Width;
            Height = DungeonGlobal.Resolution.Height;

            _race = component.Race;
            SetBackgroundByRace(Race.Muitu);
        }

        private Race? _race;

        public override bool Updatable => true;

        public override void Update(GameTimeLoop gameTime)
        {
            if (Component.Race != _race)
            {
                SetBackgroundByRace(Component.Race.Value);
                _race = Component.Race.Value;
            }
        }

        private void SetBackgroundByRace(Race race)
        {
            Image = $"Backgrounds/Races/{race.ToString()}.jpg".AsmImg();
        }
    }
}

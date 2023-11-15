using Dungeon12.Entities;
using Dungeon12.SceneObjects.Base;

namespace Dungeon12.SceneObjects.GlobalMap
{
    internal class PortraitHero : Border
    {
        public PortraitHero(Hero hero) : base(75, 115, 0)
        {
            this.Image = hero.Avatar;
        }
    }
}

using Nabunassar.Entities;
using Nabunassar.SceneObjects.Base;

namespace Nabunassar.SceneObjects.GlobalMap
{
    internal class PortraitHero : Border
    {
        public PortraitHero(Hero hero) : base(75, 115, 0)
        {
            this.Image = hero.PortraitImage;
        }
    }
}

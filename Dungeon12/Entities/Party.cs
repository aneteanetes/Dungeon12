using Dungeon12.Entities.Map;
using System.Collections.Generic;

namespace Dungeon12.Entities
{
    public class Party
    {
        public Hero Hero1 { get; set; }

        public Hero Hero2 { get; set; }

        public Hero Hero3 { get; set; }

        public Hero Hero4 { get; set; }

        public Food Food { get; set; } = new Food();

        public void Move(Location location)
        {
            if (Global.Game.Location == location)
                return;

            Global.Game.Location = location;
            if (!location.Region.Indoor)
            {
                List<Hero> restoreHeroes = new List<Hero>();

                if (Hero1.Hits != Hero1.MaxHits)
                    restoreHeroes.Add(Hero1);
                if (Hero2.Hits != Hero2.MaxHits)
                    restoreHeroes.Add(Hero2);
                if (Hero3.Hits != Hero3.MaxHits)
                    restoreHeroes.Add(Hero3);
                if (Hero4.Hits != Hero4.MaxHits)
                    restoreHeroes.Add(Hero4);

                if (restoreHeroes.Count > 0)
                    Food.Restore(restoreHeroes);
            }
        }

        public void Move(Region region)
        {
            if (Global.Game.Region == region)
                return;

            if (!region.Indoor)
            {
                if (Food.Value > 0)
                {
                    List<Hero> restoreHeroes = new List<Hero>();

                    if (Hero1.Hits != Hero1.MaxHits)
                        restoreHeroes.Add(Hero1);
                    if (Hero2.Hits != Hero2.MaxHits)
                        restoreHeroes.Add(Hero2);
                    if (Hero3.Hits != Hero3.MaxHits)
                        restoreHeroes.Add(Hero3);
                    if (Hero4.Hits != Hero4.MaxHits)
                        restoreHeroes.Add(Hero4);

                    if (restoreHeroes.Count > 0)
                        Food.Restore(restoreHeroes, 3);
                }
                else
                {
                    Hero1.Tires(1);
                    Hero2.Tires(1);
                    Hero3.Tires(1);
                    Hero4.Tires(1);
                }
            }
        }
    }
}
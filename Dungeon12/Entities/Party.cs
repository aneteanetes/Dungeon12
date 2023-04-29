using Dungeon12.Entities.Enums;
using Dungeon12.Entities.Map;
using Dungeon12.Entities.Turning;
using System.Collections;
using Region = Dungeon12.Entities.Map.Region;

namespace Dungeon12.Entities
{
    internal class Party : IEnumerable<Hero>
    {
        public Hero Hero1 { get; set; }

        public Hero Hero2 { get; set; }

        public Hero Hero3 { get; set; }

        public Hero Hero4 { get; set; }

        public Fraction Fraction { get; set; }

        public Hero[] Heroes => new Hero[] { Hero1, Hero2, Hero3, Hero4 };

        public bool PortalsActive => Heroes.Any(h => h.Archetype == Enums.Archetype.Mage);

        public Food Food { get; set; } = new Food();

        public Fame Fame { get; set; } = new Fame();

        public IEnumerator<Hero> GetEnumerator()
        {
            yield return Hero1;
            yield return Hero2;
            yield return Hero3;
            yield return Hero4;
        }

        public void Move(Location location)
        {
            if (Global.Game.Location == location)
                return;

            Global.Game.Location = location;
            if (!location.Region.Indoor)
            {
                List<Hero> restoreHeroes = new List<Hero>();

                if (!Hero1.Hp.ValuesEquals())
                    restoreHeroes.Add(Hero1);
                if (!Hero2.Hp.ValuesEquals())
                    restoreHeroes.Add(Hero2);
                if (!Hero3.Hp.ValuesEquals())
                    restoreHeroes.Add(Hero3);
                if (!Hero4.Hp.ValuesEquals())
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

                    if (!Hero1.Hp.ValuesEquals())
                        restoreHeroes.Add(Hero1);
                    if (!Hero2.Hp.ValuesEquals())
                        restoreHeroes.Add(Hero2);
                    if (!Hero3.Hp.ValuesEquals())
                        restoreHeroes.Add(Hero3);
                    if (!Hero4.Hp.ValuesEquals())
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

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
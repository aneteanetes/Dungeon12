using Dungeon.View;
using Dungeon12.Entities.Abilities;
using Dungeon12.Entities.Enums;
using Dungeon12.Entities.Perks;
using System;
using System.Collections.Generic;

namespace Dungeon12.Entities
{
    public class Hero 
    {
        private Archetype _class;
        public Archetype Class
        {
            get
            {
                return _class;
            }
            set
            {
                ClassChange?.Invoke(_class, value);
                _class = value;
            }
        }

        public void BindSkills()
        {
            switch (Class)
            {
                case Archetype.Warrior:
                    Landscape = Eating = Repair = Weaponcraft = 1;
                    break;
                case Archetype.Mage:
                    Portals = Attension = Spiritism = Alchemy = 1;
                    break;
                case Archetype.Thief:
                    Traps = Lockpicking = Stealing = Leatherwork = 1;
                    break;
                case Archetype.Priest:
                    Prayers = FoodStoring = Trade = Tailoring = 1;
                    break;
                default:
                    break;
            }
        }

        public Action<Archetype, Archetype> ClassChange;

        public Sex Sex { get; set; }

        /// <summary>
        /// Усталость в %
        /// </summary>
        public int Tire { get; set; }

        /// <summary>
        /// Утомить
        /// </summary>
        public void Tires(int percent)
        {
            if (Tire + percent > 50)
                Tire = 50;
            else
                Tire += percent;
        }

        public string Name { get; set; }

        public string Chip { get; set; }

        public int Level { get; set; } = 1;

        public int FreePoints { get; set; } = 2;

        public string Avatar { get; set; }

        public int Hits { get; set; }

        public int MaxHits { get; set; }

        public List<Ability> Abilities { get; set; } = new List<Ability>();

        public List<Ability> Effects { get; set; } = new List<Ability>();

        public void Heal(int hp)
        {
            if (Hits + hp > MaxHits)
                Hits = MaxHits;
            else
                Hits += hp;
        }

        public SpriteSheet WalkSpriteSheet { get; set; }

        public List<Perk> Perks { get; set; } = new List<Perk>();

        public Crafts? Profession { get; set; }

        public Fraction? Fraction { get; set; }

        public Spec? Spec { get; set; }

        //skills

        public int Landscape { get; set; }
        public int Eating { get; set; }
        public int Repair { get; set; }
        public int Weaponcraft { get; set; }

        public int Portals { get; set; }
        public int Attension { get; set; }
        public int Spiritism { get; set; }
        public int Alchemy { get; set; }

        public int Traps { get; set; }
        public int Lockpicking { get; set; }
        public int Stealing { get; set; }
        public int Leatherwork { get; set; }

        public int Prayers { get; set; }
        public int FoodStoring { get; set; }
        public int Trade { get; set; }
        public int Tailoring { get; set; }
    }
}
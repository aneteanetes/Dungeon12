using Dungeon.View;
using Dungeon12.Components;
using Dungeon12.Entities.Enums;
using Dungeon12.Entities.MapRelated;
using Dungeon12.Entities.Perks;
using System;
using System.Collections.Generic;

namespace Dungeon12.Entities
{
    public class Hero : IPhysical
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

        public void Heal(int hp)
        {
            if (Hits + hp > MaxHits)
                Hits = MaxHits;
            else
                Hits += hp;
        }

        public SpriteSheet WalkSpriteSheet { get; set; }

        public MapObject PhysicalObject { get; set; }

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
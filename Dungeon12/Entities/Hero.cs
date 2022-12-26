using Dungeon.View;
using Dungeon12.Entities.Abilities;
using Dungeon12.Entities.Enums;
using Dungeon12.Entities.Perks;
using System;
using System.Collections.Generic;

namespace Dungeon12.Entities
{
    internal class Hero : Battler
    {
        public bool IsSelected { get; set; }

        public int FreePoints { get; set; } = 0;

        public int Strength { get; set; } = 10;

        public int Agility { get; set; } = 10;

        public int Intellegence { get; set; } = 10;

        public int Stamina { get; set; } = 10;

        public Classes Class { get; set; }

        private Archetype _class;
        public Archetype Archetype
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
            switch (Archetype)
            {
                case Archetype.Warrior:
                    Landscape = Eating = Repair = Weaponcraft = 0;
                    break;
                case Archetype.Mage:
                    Portals = Attension = Spiritism = Alchemy = 0;
                    break;
                case Archetype.Thief:
                    Traps = Lockpicking = Stealing = Leatherwork = 0;
                    break;
                case Archetype.Priest:
                    Prayers = FoodStoring = Trade = Tailoring = 0;
                    break;
                default:
                    break;
            }
        }

        public int SkillValue(Skill skill) => skill switch
        {
            Skill.Landscape => Landscape,
            Skill.Eating => Eating,
            Skill.Repair => Repair,
            Skill.Smithing => Weaponcraft,
            Skill.Portals => Portals,
            Skill.Attension => Attension,
            Skill.Enchantment => Spiritism,
            Skill.Alchemy => Alchemy,
            Skill.Traps => Traps,
            Skill.Lockpicking => Lockpicking,
            Skill.Stealing => Stealing,
            Skill.Leatherwork => Leatherwork,
            Skill.Prayers => Prayers,
            Skill.FoodStoring => FoodStoring,
            Skill.Trade => Trade,
            Skill.Tailoring => Tailoring,
            _ => 0,
        };

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

        public string Avatar { get; set; }

        public List<Ability> Abilities { get; set; } = new List<Ability>();

        public List<Ability> Effects { get; set; } = new List<Ability>();

        public void Heal(int hp)
        {
            Hp.Add(hp);
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

        public Inventory Inventory { get; set; } = new Inventory();
    }
}
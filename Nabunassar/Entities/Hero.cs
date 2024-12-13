using Dungeon.SceneObjects.Grouping;
using Nabunassar.Entities.Abilities;
using Nabunassar.Entities.Characteristics;
using Nabunassar.Entities.Characters;
using Nabunassar.Entities.Enums;

namespace Nabunassar.Entities
{
    internal class Hero : Battler
    {
        public Hero()
        {
            PrimaryStats.Constitution = Rank.d6;
            PrimaryStats.Agility = Rank.d6;
            PrimaryStats.Intelligence = Rank.d6;
            PrimaryStats.Dialectics = Rank.d6;
        }

        public ObjectGroupProperty IsActive { get; set; } = new ObjectGroupProperty();

        public Race? Race { get; set; }

        public Fraction? Fraction { get; set; }

        public Archetype? Archetype { get; set; }

        public Primary PrimaryStats { get; set; } = new Primary();

        public Secondary SecondaryStats { get; set; }

        public Quad<Skill> Skills { get; set; }

        public Quad<Ability> Abilities { get; set; }

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

        public override string Image => Avatar;

        public string Avatar { get; set; }

        public Inventory Inventory { get; set; } = new Inventory();
    }
}
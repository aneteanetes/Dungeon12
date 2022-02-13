using Dungeon.View;
using Dungeon12.Components;
using Dungeon12.Entities.Enums;
using Dungeon12.Entities.MapRelated;
using Dungeon12.Entities.Perks;
using System.Collections.Generic;

namespace Dungeon12.Entities
{
    public class Hero : IPhysical
    {
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
    }
}
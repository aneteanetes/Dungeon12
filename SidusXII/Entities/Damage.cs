using SidusXII.Enums;
using System.Collections.Generic;

namespace SidusXII.Entities
{
    public class Damage
    {
        public int Min { get; set; }

        public int Max { get; set; }

        public Element ElementType { get; set; }

        public List<Damage> Parts { get; set; }

        public void Calculate(Defence defence)
        {

        }

        public static Damage operator +(Damage damage, Damage other)
        {
            if (other.ElementType == damage.ElementType)
                return damage;

            return damage;
        }
    }
}
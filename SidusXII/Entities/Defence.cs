using SidusXII.Enums;
using System.Collections.Generic;

namespace SidusXII.Entities
{
    public class Defence
    {
        public int Value { get; set; }

        public Element Type { get; set; }

        public List<Damage> Parts { get; set; }
    }
}
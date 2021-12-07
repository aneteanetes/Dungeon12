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
        public string Name { get; set; }

        public SpriteSheet WalkSpriteSheet { get; set; }

        public MapObject PhysicalObject { get; set; }

        public List<Perk> Perks { get; set; } = new List<Perk>();

        public Crafts? Profession { get; set; }

        public Fraction? Fraction { get; set; }
    }
}
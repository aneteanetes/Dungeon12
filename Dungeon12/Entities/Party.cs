using Dungeon;
using Dungeon.Entities.Animations;
using Dungeon12.Entities.MapRelated;
using System;

namespace Dungeon12.Entities
{
    public class Party : MapObject
    {
        public bool CantMove { get; set; }

        public double Speed => 1.5;

        public AnimationMap AnimationMap { get; set; }
            = new AnimationMap(new Dungeon.Types.Point(32, 32), "Classes/Warrior/sprite.png".AsmImg(), 3,time: TimeSpan.FromSeconds(3));
    }
}

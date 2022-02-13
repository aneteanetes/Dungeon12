using System;
using System.Collections.Generic;

namespace Dungeon12.Entities
{
    public class Food
    {
        public int Value { get; set; } = 0;

        public int Max { get; set; } = 5;

        public double Quality { get; set; } = 5;

        public void Restore(List<Hero> heroes, double multiplier = 1)
        {
            if (Value > 0)
            {
                Value--;
                var quality = (Quality / 100) * multiplier;
                heroes.ForEach(h =>
                {
                    h.Heal((int)(h.MaxHits * quality));
                });
            }
        }
    }
}
using Dungeon;
using Dungeon12.Items.Enums;
using System;

namespace Dungeon12.Items
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class GenerationAttribute : ValueAttribute
    {
        public int MinimumLevel { get; }

        public int GenerationMultipler { get; }

        public Stats[] AvailableStats { get; }

        public Type ItemType { get; set; }

        public Rarity Rarity { get; set; }

        public Stats Stat { get; set; }

        public int Chance { get; set; }

        public GenerationAttribute(int minLevel = 1, int statValueMultipler = 1, params Stats[] stats) : base(default)
        {
            MinimumLevel = minLevel;
            GenerationMultipler = statValueMultipler;
            AvailableStats = stats ?? new Stats[] { Stats.Health };
            this.Value = this;
        }
    }
}
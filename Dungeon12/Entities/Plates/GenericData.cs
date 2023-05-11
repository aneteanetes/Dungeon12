using Dungeon.Types;
using Dungeon12.Entities.Cooldowns;
using Dungeon12.Entities.Enums;
using Dungeon12.Entities.Runes;

namespace Dungeon12.Entities.Plates
{
    internal class GenericData
    {
        public Square SizeSettings { get; set; }

        /// <summary>
        /// Absolute
        /// </summary>
        public string Icon { get; set; }

        public string Title { get; set; }

        public string Subtype { get; set; }

        public string Rank { get; set; }

        public List<ResourceData> Resources { get; set; }

        public decimal Radius { get; set; }

        /// <summary>
        /// (may be area)
        /// </summary>
        public DurationData Duration { get; set; }

        public Cooldown Cooldown { get; set; }

        public int Charges { get; set; }

        public List<RequiredData> Requires { get; set; }

        public int RequiresLevel { get; set; }

        public Fraction Fraction { get; set; } = Fraction.Neutral;

        public string Text { get; set; }

        public Rune Rune { get; set; }
    }
}

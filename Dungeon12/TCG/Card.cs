using Dungeon12.TCG.Enums;

namespace Dungeon12.TCG
{
    internal class Card
    {
        public Types Type { get; set; }

        public Subtypes? Subtype { get; set; }

        public Region Region { get; set; }

        public int? Cost { get; set; }

        public int? Power { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Cброшена
        /// </summary>
        public bool IsDraft { get; set; }

        /// <summary>
        /// Активна
        /// </summary>
        public bool IsActive { get; set; }
    }
}
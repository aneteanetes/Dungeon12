namespace Dungeon12.Entities.Alive
{
    public class Damage
    {
        public DamageType Type { get; set; }

        public long Amount { get; set; }

        /// <summary>
        /// Проценты
        /// </summary>
        public long ArmorPenetration { get; set; }

        /// <summary>
        /// Проценты
        /// </summary>
        public long MagicPenetration { get; set; }
    }
}
using Dungeon.Drawing;

namespace Dungeon12.Items.Enums
{
    public enum Rarity
    {
        /// <summary>
        /// magenta
        /// </summary>
        [Color(255, 0, 255)]
        Quest = 0,

        /// <summary>
        /// Gray
        /// </summary>
        [Color(169,169,169)]
        Poor = 1,
        /// <summary>
        /// White
        /// </summary>
        [Color(255,255,255)]
        Common = 2,
        /// <summary>
        /// Blue
        /// </summary>
        [Color(65, 105, 225)]
        Uncommon = 4,
        /// <summary>
        /// Yellow
        /// </summary>
        [Color(255, 255, 0)]
        Rare = 7,
        /// <summary>
        /// Green
        /// </summary>
        [Color(124, 252, 0)]
        Set = 9,
        /// <summary>
        /// DarkMagenta
        /// </summary>
        [Color(199, 21, 133)]
        Epic = 10,
        /// <summary>
        /// Cyan
        /// </summary>
        [Color(32, 178, 170)]
        Legendary = 12,
        /// <summary>
        /// DarkYellow
        /// </summary>
        [Color(255, 215, 0)]
        Artefact = 15,
        /// <summary>
        /// Red
        /// </summary>
        [Color(255, 69, 0)]
        Fired = 20,
        /// <summary>
        /// Blue again
        /// </summary>
        [Color(135, 206, 235)]
        Watered = 21,
        /// <summary>
        /// dark cyan
        /// </summary>
        [Color(0, 139, 139)]
        Deck = 30
    }
}
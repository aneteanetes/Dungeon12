using Dungeon.Drawing;
using System.ComponentModel.DataAnnotations;

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
        [Generation(1,1,Stats.AttackDamage,Stats.Health,Stats.AbilityPower)]
        [Display(Name = "Бесполезный")]
        Poor = 1,
        /// <summary>
        /// White
        /// </summary>
        [Color(255,255,255)]
        [Generation(3, 1, Stats.AttackDamage, Stats.Health, Stats.AbilityPower, Stats.Barrier, Stats.Defence, Chance = 80)]
        [Display(Name = "Обычный")]
        Common = 2,
        /// <summary>
        /// Blue
        /// </summary>
        [Color(65, 105, 225)]
        [Generation(5, 2, Stats.AttackDamage, Stats.Health, Stats.AbilityPower, Stats.Barrier, Stats.Defence, Stats.Class, Chance =60)]
        [Display(Name = "Необычный")]
        Uncommon = 4,
        /// <summary>
        /// Yellow
        /// </summary>
        [Color(255, 255, 0)]
        [Generation(7, 2, Stats.AttackDamage, Stats.Health, Stats.AbilityPower, Stats.Barrier, Stats.Defence, Stats.Class, Chance = 40)]
        [Display(Name = "Редкий")]
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
        [Display(Name = "Эпический")]
        [Generation(10, 2, Stats.AttackDamage, Stats.Health, Stats.AbilityPower, Stats.Barrier, Stats.Defence, Stats.Class, Stats.Resource, Chance = 10)]
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
        Deck = 30,
        /// <summary>
        /// dark cyan
        /// </summary>
        [Color(188, 143, 143)]
        Resource = 40
    }
}
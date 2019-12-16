using Dungeon.Drawing;

namespace Dungeon12.Items.Enums
{
    using Dungeon.Drawing;
    using System;
    using System.ComponentModel.DataAnnotations;

    [Flags]
    public enum Stats
    {
        [Display(Name = "Здоровье")]
        [Generation(statValueMultipler: 10, Chance = 90)]
        [Color(205, 92, 92)]
        Health = 1,

        [Display(Name = "Ресурс")]
        [Generation(statValueMultipler: 1, Chance = 20)]
        Resource = 2,

        [Display(Name = "Сила Магии")]
        [Generation(statValueMultipler: 3, Chance = 40)]
        [Color(255, 51, 153)]
        AbilityPower = 3,

        [Display(Name = "Сила Атаки")]        
        [Generation(statValueMultipler: 3, Chance = 50)]
        [Color(224, 255, 255)]
        AttackDamage = 4,

        [Display(Name = "Защита")]
        [Generation(statValueMultipler: 2, Chance = 80)]
        [Color(0, 139, 139)]
        Defence = 5,

        [Display(Name = "Барьер")]
        [Generation(statValueMultipler: 2, Chance = 60)]
        [Color(139, 0, 139)]
        Barrier = 6,

        [Generation(statValueMultipler:1, Chance = 50)]
        Class = 7,

        None = 0
    }
}
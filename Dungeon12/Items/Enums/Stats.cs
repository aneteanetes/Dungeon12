namespace Dungeon12.Items.Enums
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [Flags]
    public enum Stats
    {
        [Display(Name = "Здоровье")]
        [Generation(statValueMultipler: 10, Chance = 90)]
        Health = 1,
        [Display(Name = "Ресурс")]
        [Generation(statValueMultipler: 1, Chance = 20)]
        Resource = 2,
        [Display(Name = "Сила Магии")]
        [Generation(statValueMultipler: 3, Chance = 40)]
        AbilityPower = 3,
        [Display(Name = "Сила Атаки")]
        [Generation(statValueMultipler: 3, Chance = 50)]
        AttackDamage = 4,
        [Display(Name = "Защита")]
        [Generation(statValueMultipler: 2, Chance = 80)]
        Defence = 5,
        [Display(Name = "Барьер")]
        [Generation(statValueMultipler: 2, Chance = 60)]
        Barrier = 6,
        [Generation(statValueMultipler:1, Chance = 50)]
        Class = 7,
        None = 0
    }
}
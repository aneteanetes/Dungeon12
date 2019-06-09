namespace Rogue.Abilities.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum AbilityActionAttribute
    {
        [Display(Name = "По времени")]
        DmgHealOnTime = 0,
        [Display(Name = "Мгновенный")]
        DmgHealInstant = 1,
        [Display(Name = "Особый")]
        Special = 2,
        [Display(Name = "Мгновенно")]
        EffectInstant = 8,
        [Display(Name = "По времени")]
        EffectOfTime = 9,
        DebuffStack = 10,
        [Display(Name = "С удержанием")]
        Hold = 11,
    }
}
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

        [Display(Name = "По времени")]
        DebuffStack = 10,

        [Display(Name = "С удержанием")]
        Hold = 11,

        [Display(Name = "Постоянно")]
        Passive = 11,
    }
}
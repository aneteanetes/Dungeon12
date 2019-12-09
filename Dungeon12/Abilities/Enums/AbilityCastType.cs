namespace Dungeon12.Abilities.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum AbilityCastType
    {
        [Display(Name = "Активный")]
        Active = 0,
        [Display(Name = "Пассивный")]
        Passive = 1
    }
}
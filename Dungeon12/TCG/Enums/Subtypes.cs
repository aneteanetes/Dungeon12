using System.ComponentModel.DataAnnotations;

namespace Dungeon12.TCG.Enums
{
    public enum Subtypes
    {
        [Display(Name = "Ближний бой")]
        Melee,

        [Display(Name = "Дальний бой")]
        Range,

        [Display(Name = "Защитник")]
        Defender,

        [Display(Name = "Большой")]
        Big,

        [Display(Name = "Маленький")]
        Small,

        [Display(Name = "Огненный")]
        Fire,

        [Display(Name = "Водный")]
        Water,

        [Display(Name = "Воздушный")]
        Air,

        [Display(Name = "Земной")]
        Earth,

        [Display(Name = "Яд")]
        Poison,

        [Display(Name = "Стратегия")]
        Strategy,

        [Display(Name = "Заклинание")]
        Spell
    }
}

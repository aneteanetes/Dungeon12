using System.ComponentModel.DataAnnotations;

namespace Dungeon12.Entities.Enums
{
    public enum Fraction
    {
        Neutral,

        [Display(Name = "Авангард")]
        Vanguard,

        [Display(Name = "Гильдия магов")]
        MageCircle,

        [Display(Name = "Наёмники")]
        ShadowGuild,

        [Display(Name = "Экзархат")]
        Exarchate,

        [Display(Name = "Культ проклятых")]
        DeathCult,
    }
}
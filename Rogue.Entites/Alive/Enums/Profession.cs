using System.ComponentModel.DataAnnotations;

namespace Rogue.Entites.Enums
{
    public enum Profession
    {
        [Display(Name = "Приключенец")]
        Common = 0,

        [Display(Name = "Шахтер")]
        Miner = 1,

        [Display(Name = "Собиратель")]
        Herbalist = 2,

        [Display(Name = "Кузнец")]
        Blacksmith = 10,

        [Display(Name = "Алхимик")]
        Alchemist = 20
    }
}
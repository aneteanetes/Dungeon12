using System.ComponentModel.DataAnnotations;

namespace Nabunassar.TCG.Enums
{
    public enum Types
    {
        [Display(Name = "Локация")]
        Land,

        [Display(Name = "Существо")]
        Creature,

        [Display(Name = "Способность")]
        Skill
    }
}

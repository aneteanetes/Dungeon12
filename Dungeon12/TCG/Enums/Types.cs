using System.ComponentModel.DataAnnotations;

namespace Dungeon12.TCG.Enums
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

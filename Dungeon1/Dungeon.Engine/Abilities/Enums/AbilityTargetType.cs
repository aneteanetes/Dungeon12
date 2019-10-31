namespace Dungeon.Abilities.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum AbilityTargetType
    {
        [Display(Name = "Требуется цель")]
        Target=0,

        [Display(Name = "Цель не требуется")]
        NonTarget = 1,

        [Display(Name = "Требуется несколько целей")]
        TwoTargets =2,

        [Display(Name = "Цель может не требоваться")]
        TargetAndNonTarget = 3,

        [Display(Name = "Цель сам персонаж")]
        SelfTarget = 4,

        [Display(Name = "Требуется дружественнаяя цель")]
        TargetFrendly = 5,
    }
}
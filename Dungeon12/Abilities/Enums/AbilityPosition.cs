namespace Dungeon12.Abilities.Enums
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public enum AbilityPosition
    {
        [Display(Name="Лев.")]
        Left = 0,
        [Display(Name = "Прав.")]
        Right = 1,
        [Display(Name = "Q")]
        Q = 2,
        [Display(Name = "E")]
        E = 3
    }
}

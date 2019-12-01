using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dungeon.Entities.Fractions
{
    public enum FractionLevel
    {
        [Display(Name = "Ненависть")]
        Hated = 5,

        [Display(Name = "Враждебность")]
        Hostile = 6,

        [Display(Name = "Неприязнь")]
        Unfriendly = 7,

        [Display(Name = "Равнодушие")]
        Neutral = 0,

        [Display(Name = "Дружелюбие")]
        Friendly = 9,

        [Display(Name = "Уважение")]
        Honored = 10,

        [Display(Name = "Почтение")]
        Revered = 11
    }
}
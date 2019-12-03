using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dungeon.Entities.Fractions
{
    public enum FractionLevel
    {
        /// <summary>
        /// Ненависть
        /// </summary>
        [Display(Name = "Ненависть")]
        Hated = 5,

        /// <summary>
        /// Враждебность
        /// </summary>
        [Display(Name = "Враждебность")]
        Hostile = 6,

        /// <summary>
        /// Неприязнь
        /// </summary>
        [Display(Name = "Неприязнь")]
        Unfriendly = 7,

        /// <summary>
        /// Равнодушие
        /// </summary>
        [Display(Name = "Равнодушие")]
        Neutral = 0,

        /// <summary>
        /// Дружелюбие
        /// </summary>
        [Display(Name = "Дружелюбие")]
        Friendly = 9,

        /// <summary>
        /// Уважение
        /// </summary>
        [Display(Name = "Уважение")]
        Honored = 10,

        /// <summary>
        /// Почтение
        /// </summary>
        [Display(Name = "Почтение")]
        Revered = 11
    }
}
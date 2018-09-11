using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Rogue.Entites.Alive
{
    public enum Resource
    {
        [Display(Name="Мана")]
        Mana=0,

        [Display(Name = "Кровь")]
        Blood =1,

        [Display(Name = "Кровь")]
        Judge = 2,

        Souls = 3,
        
        Poisons =4,

        Air = 5,

        Water =6,

        Earth=7,

        Fire =8,

        Soul = 9,

        Rage =10
    }
}

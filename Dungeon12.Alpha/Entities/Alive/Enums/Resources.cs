using System.ComponentModel.DataAnnotations;

namespace Dungeon12.Entities.Enums
{
    public enum Resource
    {
        [Display(Name="Мана")]
        Mana=0,

        [Display(Name = "Кровь")]
        Blood =1,

        [Display(Name = "Право")]
        Judge = 2,
        
        [Display(Name = "Дущи")]
        Souls = 3,

        [Display(Name = "Яды")]
        Poisons =4,

        [Display(Name = "Воздух")]
        Air = 5,

        [Display(Name = "Вода")]
        Water =6,

        [Display(Name = "Земля")]
        Earth =7,

        [Display(Name = "Огонь")]
        Fire =8,

        [Display(Name = "Дух")]
        Soul = 9,

        [Display(Name = "Ярость")]
        Rage =10
    }
}
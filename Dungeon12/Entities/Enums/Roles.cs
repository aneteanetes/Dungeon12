using System.ComponentModel.DataAnnotations;

namespace Dungeon12.Entities.Enums
{
    public enum Roles
    {
        [Display(Name = "Благословения")]
        Buff,

        [Display(Name = "Физический урон")]
        Damage,

        [Display(Name = "Проклятья")]
        Debuff,

        [Display(Name = "Исцеление")]
        Heal,

        [Display(Name = "Магический урон")]
        Magic,

        [Display(Name = "Дальний бой")]
        Range,

        [Display(Name = "Призыв существ")]
        Summon,

        [Display(Name = "Защита группы")]
        Tank,

        [Display(Name = "Ловушки")]
        Trap,

        [Display(Name = "Вампиризм")]
        Vampyr,
    }
}
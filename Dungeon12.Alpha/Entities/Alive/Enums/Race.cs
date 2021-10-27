using System.ComponentModel.DataAnnotations;

namespace Dungeon12
{
    /// <summary>
    /// Доступные расы для выбора
    /// </summary>
    public enum Race
    {
        [Display(Name="Человек")]
        Human = 1,

        [Display(Name = "Эльф")]
        Elf = 2,

        //[Display(Name = "Дворф")]
        //Dwarf = 4,

        [Display(Name = "Гном")]
        Gnome = 3,

        [Display(Name = "Орк")]
        Orc = 10,

        [Display(Name = "Тёмный эльф")]
        DarkElf = 6,

        //[Display(Name = "Мёртвый")]
        //Undead = 9,

        //[Display(Name = "Тролль")]
        //Troll = 8,

        [Display(Name = "Падший")]
        Fallen = 7,

        //[Display(Name = "Лунный эльф")]
        //MoonElf = 5
    }
}
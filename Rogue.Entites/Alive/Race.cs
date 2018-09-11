using System.ComponentModel.DataAnnotations;

namespace Rogue.Entites.Alive
{
    public enum Race
    {
        [Display(Name="Человек")]
        Human = 1,

        [Display(Name = "Лесной эльф")]
        Elf = 2,

        [Display(Name = "Дворф")]
        Dwarf = 4,

        [Display(Name = "Гном")]
        Gnome = 3,

        [Display(Name = "Орк")]
        Orc = 10,

        [Display(Name = "Тёмный эльф")]
        DarkElf = 6,

        [Display(Name = "Мёртвый")]
        Undead = 9,

        [Display(Name = "Тролль")]
        Troll = 8,

        [Display(Name = "Падший")]
        FallenAngel = 7,

        [Display(Name = "Лунный эльф")]
        MoonElf = 5
    }
}

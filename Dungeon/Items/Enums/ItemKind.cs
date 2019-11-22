using System.ComponentModel.DataAnnotations;

namespace Dungeon.Items.Enums
{
    public enum ItemKind
    {
        Key = 0,
        Potion = 1,
        Elixir = 2,
        [Display(Name = "Оружие")]
        Weapon = 3,
        [Display(Name = "Шлем")]
        Helm = 4,
        [Display(Name = "Броня")]
        Armor = 5,
        [Display(Name = "Обувь")]
        Boots = 6,
        [Display(Name = "Левая рука")]        
        OffHand = 7,
        Scroll = 8,
        Poison = 9,
        Resource = 10,
        Rune = 11,
        [Display(Name = "Предмет задания")]
        Quest =12
    }
}
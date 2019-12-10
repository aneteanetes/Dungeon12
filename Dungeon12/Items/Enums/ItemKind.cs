using System.ComponentModel.DataAnnotations;

namespace Dungeon12.Items.Enums
{
    public enum ItemKind
    {
        [Display(Name = "Ключ")]
        Key = 0,
        [Display(Name = "Зелье")]
        Potion = 1,
        [Display(Name = "Элексир")]
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
        [Display(Name = "Свиток")]
        Scroll = 8,
        [Display(Name = "Яд")]
        Poison = 9,
        [Display(Name = "Ресурс")]
        Resource = 10,
        [Display(Name = "Руна")]
        Rune = 11,
        [Display(Name = "Предмет задания")]
        Quest =12,
        [Display(Name = "Колода карт")]
        Deck =13
    }
}
namespace Dungeon.Entites.Alive.Enums
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Происхождение
    /// </summary>
    public enum Origins
    {
        [Display(Name = "Фермер")]
        Farmer = 0,

        [Display(Name = "Купец")]
        Merchant = 1,

        [Display(Name = "Отшельник")]
        Hermit = 2,

        [Display(Name = "Наёмник")]
        Mercenary = 3,

        [Display(Name = "Выживший")]
        Survivor = 4,

        [Display(Name = "Жертва")]
        Victim = 5
    }
}
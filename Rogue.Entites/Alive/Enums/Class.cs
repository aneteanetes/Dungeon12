using System.ComponentModel.DataAnnotations;

namespace Rogue.Entites.Enums
{
    public enum Class
    {
        [Display(Name = "Маг крови")]
        BloodMage = 1,

        [Display(Name = "Паладин")]
        Paladin = 9,

        [Display(Name = "Инквизитор")]
        Inquisitor = 7,

        [Display(Name = "Маг огня")]
        FireMage = 2,

        [Display(Name = "Убийца")]
        Assassin = 6,

        [Display(Name = "Шаман")]
        Shaman = 3,

        [Display(Name = "Некромант")]
        Nercomancer = 4,

        [Display(Name = "Монах")]
        Monk = 8,

        [Display(Name = "Алхимик")]
        Alchemist = 5,

        [Display(Name = "Воин")]
        Warrior = 10,

        [Display(Name = "Чернокнижник")]
        Warlock = 11,

        [Display(Name = "Воин молний")]
        LightningWarrior = 12,

        [Display(Name = "Валькирия")]
        Valkyrie = 14,

        [Display(Name = "Иллюзионист")]
        Illusionist = 13
    }
}
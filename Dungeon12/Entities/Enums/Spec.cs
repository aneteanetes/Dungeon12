using Dungeon;
using Dungeon12.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Dungeon12.Entities.Enums
{
    public enum Spec
    {
        [Display(Name = "Берсерк")]
        WarriorDamage, // сильные удары, добивания, никакой защиты

        [Display(Name = "Воитель")]
        [Value("Юно")]
        WarriorWarchief, // стратегические приёмы, урон в зависимости от обстоятельств

        [Display(Name = "Защитник")]
        WarriorProtector, // зищита себя, агро


        [Display(Name = "Колдун")]
        [Value("Итель")]
        MageAoe, // урон стихией по площади

        [Display(Name = "Маг")]
        MageSorcerer, // урон в одну цель

        [Display(Name = "Целитель")]
        MagePriest, // прямой и обычный хил


        [Display(Name = "Предводитель")]
        [Value("Зиморан")]
        MercenaryLeader, // промахи, скользящие удары, парирование, перехват ударов

        [Display(Name = "Убийца")]
        MercenaryAssassin, // ловушки, (дальний бой), + метки(яды), взрыв меток итд

        [Display(Name = "Головорез")]
        MercenaryOutlaw, // травмы (дебафы), травмы (CC)


        [Display(Name = "Тамплиер")]
        PaladinTemplar, // танк через чуть чуть постоянного хила 

        [Display(Name = "Адепт")]
        [Value("Илзру")]
        PaladinAdept, // прямой большой хил с затратами маны


        [Display(Name = "Инквизитор")]
        InquisitorJudge,  // утилити - призыв душ (перекачка), приговоры (дебафы)

        [Display(Name = "Экзорцист")]
        InquisitorTormentor, // урон от двух противоположных стихий, но лёгкая броня


        [Display(Name = "Некромант")]
        WarlockNecromancer, // призывает скелетов, духов, и кости

        [Display(Name = "Маг крови")]
        WarlockBloodMage, // доты, плюс хилит от дот (массово)

        [Display(Name = "Шаман вуду")]
        WarlockVoodoo // контроль союзников и врагов
    }
}
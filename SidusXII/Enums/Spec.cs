using Dungeon;
using SidusXII.Enums;
using System.Collections.Generic;

namespace SidusXII
{
    public class Spec : GameEnum
    {
        [Value("Рубака")]
        public static Spec WarriorDamage { get; set; } // сильные удары, добивания, никакой защиты

        [Value("Воитель")]
        public static Spec WarriorWarchief { get; set; } // стратегические приёмы, урон в зависимости от обстоятельств

        [Value("Защитник")]
        public static Spec WarriorProtector { get; set; } // зищита себя, агро


        [Value("Колдун")]
        public static Spec MageAoe { get; set; } // урон стихией по площади

        [Value("Маг")]
        public static Spec MageSorcerer { get; set; } // урон в одну цель

        [Value("Целитель")]
        public static Spec MagePriest { get; set; } // прямой и обычный хил


        [Value("Предводитель")]
        public static Spec MercenaryLeader { get; set; } // промахи, скользящие удары, парирование, перехват ударов

        [Value("Убийца")]
        public static Spec MercenaryAssassin { get; set; } // ловушки, (дальний бой), + метки(яды), взрыв меток итд

        [Value("Головорез")]
        public static Spec MercenaryOutlaw { get; set; } // травмы (дебафы), травмы (CC)


        [Value("Тамплиер")]
        public static Spec PaladinTemplar { get; set; } // танк через чуть чуть постоянного хила 

        [Value("Адепт")]
        public static Spec PaladinAdept { get; set; } // прямой большой хил с затратами маны


        [Value("Судья")]
        public static Spec InquisitorJudge { get; set; }  // утилити - призыв душ (перекачка), приговоры (дебафы)

        [Value("Истязатель")]
        public static Spec InquisitorTormentor { get; set; } // урон от двух противоположных стихий, но лёгкая броня


        [Value("Некромант")]
        public static Spec WarlockNecromancer { get; set; } // призывает скелетов, духов, и кости

        [Value("Маг крови")]
        public static Spec WarlockBloodMage { get; set; } // доты, плюс хилит от дот (массово)

        // танки - 3 воин, паладин, предводитель
        // хилеры - 3 целитель, адепт(пал), маг крови
        // урон - 8 прямой урон, урон по условиям, АОЕ, прямой маг., ловушки+серии ударов, урон от двух стихий сразу, призывы
        // утилити - 2 судья (дебафы/перекачка), головорез (дебафы, CC)
        public static IEnumerable<Spec> ByClass(Class @class) => @class.PropertyName switch
        {
            nameof(Class.Warrior) => new Spec[] { WarriorDamage, WarriorWarchief, WarriorProtector },
            nameof(Class.Mage) => new Spec[] { MageSorcerer, MageAoe, MagePriest },
            nameof(Class.Mercenary) => new Spec[] { MercenaryLeader, MercenaryAssassin, MercenaryOutlaw },
            nameof(Class.Paladin) => new Spec[] { PaladinTemplar, PaladinAdept },
            nameof(Class.Inquisitor) => new Spec[] { InquisitorTormentor, InquisitorJudge },
            nameof(Class.Warlock) => new Spec[] { WarlockNecromancer, WarlockBloodMage },
            _ => default,
        };
    }
}
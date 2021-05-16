using Dungeon;

namespace SidusXII.Enums
{
    public class Profession : GameEnum
    {
        [Value("Кузнечное дело")]
        public static Profession Blacksmith { get; set; } // собирание руды, выплавка предметов

        [Value("Плотник")]
        public static Profession Carpenter { get; set; } // собирание древесины, создание предметов

        [Value("Портной")]
        public static Profession Tailor { get; set; } // создание предметов из кожи, ниток (шерсти), и искусственных ниток (изобретения)

        [Value("Алхимик")]
        public static Profession Alchemist { get; set; } // собирание трав, создание зелий, "физческая магия, смешанный урон (урон частично поглащающийся защитой от элементов)" (химия)

        [Value("Изобретатель")]
        public static Profession Artificer { get; set; } // создание рецептов и предметов, из уже готового
    }
}
 
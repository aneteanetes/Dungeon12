using Dungeon;

namespace SidusXII.Enums
{
    public class Element : GameEnum
    {
        [Value("Физический")]
        public static Element Physical { get; set; } // весь физический (кинетический) урон

        [Value("Духовный")]
        public static Element Spirit { get; set; } // урон магии духа (воли)


        [Value("Тёмный")]
        public static Element Dark { get; set; } // урон тёмной магией (плохой, негативная энергия)

        [Value("Светлый")]
        public static Element Light { get; set; } // урон светлой магией (хороший, позитивная энергия)


        [Value("Природа")]
        public static Element Nature { get; set; } // урон магией природы "естественный"

        [Value("Разложение")]
        public static Element Decay { get; set; } // урон магией смерти "не естественный (нежить)"


        [Value("Огонь")]
        public static Element Fire { get; set; } // урон огнём

        [Value("Вода")]
        public static Element Water { get; set; } // урон водой (холодом, итд)


        [Value("Воздух")]
        public static Element Air { get; set; } // урон воздухом (молнии, ветра)

        [Value("Земля")]
        public static Element Earth { get; set; } // урон землёй (всегда смешанный с другой стихией)

        // Physical -> Earth -> Fire -> Nature -> Light -> Spirit -> Air -> Water -> Decay -> Dark -> Physical
    }
}
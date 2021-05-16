using Dungeon;

namespace SidusXII.Enums
{
    [Value("Раса")]
    public class Race : GameEnum
    {
        [Value("Человек")]
        public static Race Human { get; set; } // физический урон, средний | больше очков навыков

        [Value("Эльф")]
        public static Race Elf { get; set; } // высший (solar, свет+огонь), тёмный (тьма), лесной (природа), каждая атака сильнее на 0.2 (x10 ударов = x2 урон)

        [Value("Высший Эльф")]
        public static Race HighElf { get; set; }

        [Value("Тёмный Эльф")]
        public static Race DarkElf { get; set; }

        [Value("Лесной Эльф")]
        public static Race WoodElf { get; set; }

        [Value("Дворф")]
        public static Race Dwarf { get; set; } // физический урон, слабый, часть урона превращается в защиту

        [Value("Орк")]
        public static Race Orc { get; set; } // сильный физический урон, деморализация (минус к иниц/урону врага) | минус физ и маг защита

        [Value("Демон")]
        public static Race Demon { get; set; } // урон огнём, горение по времени (огнём)

        [Value("Падший")]
        public static Race Fallen { get; set; } // священный урон (holy), увеличенный урон по маг. защите

        [Value("Тролль")]
        public static Race Troll { get; set; } // урон на выбор от стихии, часть урона лечит (своя стихия лечит)

        [Value("Воздушный Тролль")]
        public static Race AirTroll { get; set; }

        [Value("Земляной Тролль")]
        public static Race EarthTroll { get; set; }

        [Value("Огненный Тролль")]
        public static Race FireTroll { get; set; }

        [Value("Водный Тролль")]
        public static Race WaterTroll { get; set; }

        [Value("Нежить")]
        public static Race Undead { get; set; } // урон разложением (death), разложение всегда наносит урон через физ защиту

        public Element Element { get; set; }

        public Race MainRace { get; set; }

        public bool HasSubRaces { get; set; }

        protected override void InitValue()
        {
            if (this == Elf || this == Troll)
                HasSubRaces = true;


            if (PropertyName != nameof(Elf) && PropertyName.Contains(nameof(Elf)))
            {
                MainRace = Race.Elf;
            }

            if (PropertyName != nameof(Troll) && PropertyName.Contains(nameof(Troll)))
            {
                MainRace = Race.Troll;
            }

            switch (PropertyName)
            {
                case nameof(AirTroll): Element = Element.Air; return;
                case nameof(EarthTroll): Element = Element.Earth; return;
                case nameof(FireTroll): Element = Element.Fire; return;
                case nameof(WaterTroll): Element = Element.Water; return;

                case nameof(HighElf): Element = Element.Light; return;
                case nameof(DarkElf): Element = Element.Dark; return;
                case nameof(WoodElf): Element = Element.Nature; return;

                case nameof(Human):
                case nameof(Demon):
                case nameof(Orc):
                case nameof(Troll):
                case nameof(Dwarf): Element = Element.Physical; return;

                case nameof(Fallen): Element = Element.Light; return;
                case nameof(Undead): Element = Element.Dark; return;
                default: return;
            }
        }
    }
}
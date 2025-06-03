using Nabunassar.Entities.Enums;

namespace Nabunassar.Entities.Characters
{
    public enum Race
    {
        /// <summary>
        /// Островитяне
        /// </summary>
        Muitu,
        /// <summary>
        /// Последователи Аваля
        /// </summary>
        Shamshu,
        /// <summary>
        /// Древесные
        /// </summary>
        Balatu,
        /// <summary>
        /// Кровавые
        /// </summary>
        Matu,
        /// <summary>
        /// Дети бога-кузнеца
        /// </summary>
        Kararu,
        /// <summary>
        /// Полуангелы
        /// </summary>
        Esmaru,
        /// <summary>
        /// Гномы
        /// </summary>
        Buru,
        /// <summary>
        /// Друиды
        /// </summary>
        Habash,
        /// <summary>
        /// Демоны
        /// </summary>
        Nahazu,
        /// <summary>
        /// Дети магии
        /// </summary>
        Usapu,
        /// <summary>
        /// Пепельные
        /// </summary>
        Shalmu,
        /// <summary>
        /// Серые
        /// </summary>
        Gray
    }

    public static class RaceExtensions
    {
        public static Element Element(this Race race) => race switch
        {
            Race.Muitu => Enums.Element.Water,
            Race.Shamshu => Enums.Element.Holy,
            Race.Balatu => Enums.Element.Earth,
            Race.Matu => Enums.Element.Blood,
            Race.Kararu => Enums.Element.Fire,
            Race.Esmaru => Enums.Element.Frost,
            Race.Buru => Enums.Element.Physical,
            Race.Habash => Enums.Element.Moonmagic,
            Race.Nahazu => Enums.Element.Wildmagic,
            Race.Usapu => Enums.Element.Magic,
            Race.Shalmu => Enums.Element.Darkmagic,
            Race.Gray => Enums.Element.Darkmagic,
            _ => Enums.Element.None,
        };
        public static Element Element(this Race? race) => race.Value.Element();
    }
}
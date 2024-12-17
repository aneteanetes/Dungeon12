using System;

namespace Nabunassar.Entities.Enums
{
    public enum Element
    {
        None=0,

        Spirit, // природная магия - магия смерти, некромантия, магия духа
        Mental, //Ментальный, психический

        /// <summary>
        /// Физический урон
        /// </summary>
        Physical, //физический, воздух, земля и вода тоже физическая

        /// <summary>
        /// Холод
        /// </summary>
        Frost,

        /// <summary>
        /// Магия
        /// </summary>
        Magic,

        /// <summary>
        /// Святая магия - радиация
        /// </summary>
        Holy, 

        /// <summary>
        /// тёмная магия (вуду)
        /// </summary>
        Darkmagic,

        /// <summary>
        /// кислота
        /// </summary>
        Acid,

        /// <summary>
        /// Вода
        /// </summary>
        Water,

        /// <summary>
        /// Огонь
        /// </summary>
        Fire,

        /// <summary>
        /// Земля
        /// </summary>
        Earth,

        /// <summary>
        /// Воздух
        /// </summary>
        Air,

        /// <summary>
        /// Кровь
        /// </summary>
        Blood,

        /// <summary>
        /// Лунная магия
        /// </summary>
        Moonmagic,

        /// <summary>
        /// Дикая магия
        /// </summary>
        Wildmagic,
    }

    class MyClass
    {
        public MyClass()
        {
        }
    }

    public static class ElementExtensions
    {
        public static string Display(this Element element)
        {
            switch (element)
            {
                case Element.Mental: return Global.Strings["ElementMental"];
                case Element.Physical: return Global.Strings["ElementPhysical"];
                case Element.Fire: return Global.Strings["ElementFire"];
                case Element.Frost: return Global.Strings["ElementFrost"];
                case Element.Magic:return Global.Strings["ElementMagical"];
                case Element.Spirit: return Global.Strings["ElementSpirit"];
                case Element.Holy: return Global.Strings["ElementHoly"];
                case Element.Darkmagic: return Global.Strings["ElementDark"];
                default: return "";
            }
        }
    }
}
namespace Dungeon12.Entities.Enums
{
    public enum Element
    {
        Mental, //Ментальный, психический
        Physical, //физический, воздух, земля и вода тоже физическая

        Fire, // огонь, тепло, магия огня
        Frost, //холод

        Magical, //магия, атсрал, пустота, вакуум
        Spirit, // природная магия - магия смерти, некромантия, магия духа

        HolyMagic, // священная магия (радиация)
        DarkMagic, // магия тьмы, ну просто магия ёпт
    }

    public static class ElementExtensions
    {
        public static string Display(this Element element)
        {
            switch (element)
            {
                case Element.Mental: return Global.Strings.ElementMental;
                case Element.Physical: return Global.Strings.ElementPhysical;
                case Element.Fire: return Global.Strings.ElementFire;
                case Element.Frost: return Global.Strings.ElementFrost;
                case Element.Magical:return Global.Strings.ElementMagical;
                case Element.Spirit: return Global.Strings.ElementSpirit;
                case Element.HolyMagic: return Global.Strings.ElementHoly;
                case Element.DarkMagic: return Global.Strings.ElementDark;
                default: return "";
            }
        }
    }
}

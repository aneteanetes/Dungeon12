namespace Dungeon12.Entities.Enums
{
    public enum Archetype
    {
        Warrior,
        Mage,
        Thief,
        Priest
    }

    public static class ArchetypeExtensions
    {
        public static string Display(this Archetype archetype) => archetype switch
        {
            Archetype.Warrior => Global.Strings.Warrior,
            Archetype.Mage => Global.Strings.Mage,
            Archetype.Thief => Global.Strings.Thief,
            Archetype.Priest => Global.Strings.Priest,
            _ => "",
        };
        public static string Short(this Archetype archetype) => archetype switch
        {
            Archetype.Warrior => "w",
            Archetype.Mage => "m",
            Archetype.Thief => "t",
            Archetype.Priest => "p",
            _ => "",
        };

        public static Sex Sex(this Archetype archetype, int number)
        {
            switch (archetype)
            {
                case Archetype.Warrior:
                case Archetype.Thief:
                    return number < 4 ? Enums.Sex.Female : Enums.Sex.Male;
                case Archetype.Mage:
                case Archetype.Priest:
                    return number < 4 ? Enums.Sex.Male : Enums.Sex.Female;
                default: return Enums.Sex.Female;
            }
        }
    }
}

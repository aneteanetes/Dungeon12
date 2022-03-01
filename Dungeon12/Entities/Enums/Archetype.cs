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
    }
}

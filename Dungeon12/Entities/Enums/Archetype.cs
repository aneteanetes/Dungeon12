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

        public static Skill[] Skills(this Archetype archetype) => archetype switch
        {
            Archetype.Warrior => new Skill[] { Skill.Landscape, Skill.Eating, Skill.Repair, Skill.Weaponcraft },
            Archetype.Mage => new Skill[] { Skill.Portals, Skill.Attension, Skill.Spiritism, Skill.Alchemy },
            Archetype.Thief => new Skill[] { Skill.Traps, Skill.Lockpicking, Skill.Stealing, Skill.Leatherwork },
            Archetype.Priest => new Skill[] { Skill.Prayers, Skill.FoodStoring, Skill.Trade, Skill.Tailoring },
            _ => System.Array.Empty<Skill>(),
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

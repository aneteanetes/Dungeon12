using Dungeon;

namespace Dungeon12.Entities.Enums
{
    public enum Skill
    {
        [Value("w")]
        Landscape,
        [Value("w")]
        Eating,
        [Value("w")]
        Repair,
        [Value("w")]
        Weaponcraft,

        [Value("m")]
        Portals,
        [Value("m")]
        Attension,
        [Value("m")]
        Spiritism,
        [Value("m")]
        Alchemy,

        [Value("t")]
        Traps,
        [Value("t")]
        Lockpicking,
        [Value("t")]
        Stealing,
        [Value("t")]
        Leatherwork,

        [Value("p")]
        Prayers,
        [Value("p")]
        FoodStoring,
        [Value("p")]
        Trade,
        [Value("p")]
        Tailoring
    }

    public static class SkillExtensions
    {
        public static string Display(this Skill skill) => skill switch
        {
            Skill.Landscape => Global.Strings.Landscape,
            Skill.Eating => Global.Strings.Eating,
            Skill.Repair => Global.Strings.Repair,
            Skill.Weaponcraft => Global.Strings.Weaponcraft,
            Skill.Portals => Global.Strings.Portals,
            Skill.Attension => Global.Strings.Attension,
            Skill.Spiritism => Global.Strings.Spiritism,
            Skill.Alchemy => Global.Strings.Alchemy,
            Skill.Traps => Global.Strings.Traps,
            Skill.Lockpicking => Global.Strings.Lockpicking,
            Skill.Stealing => Global.Strings.Stealing,
            Skill.Leatherwork => Global.Strings.Leatherwork,
            Skill.Prayers => Global.Strings.Prayers,
            Skill.FoodStoring => Global.Strings.FoodStoring,
            Skill.Trade => Global.Strings.Trade,
            Skill.Tailoring => Global.Strings.Tailoring,
            _ => "",
        };

        public static Archetype Class(this Skill skill) => skill switch
        {
            Skill.Landscape => Archetype.Warrior,
            Skill.Eating => Archetype.Warrior,
            Skill.Repair => Archetype.Warrior,
            Skill.Weaponcraft => Archetype.Warrior,
            Skill.Portals => Archetype.Mage,
            Skill.Attension => Archetype.Mage,
            Skill.Spiritism => Archetype.Mage,
            Skill.Alchemy => Archetype.Mage,
            Skill.Traps => Archetype.Thief,
            Skill.Lockpicking => Archetype.Thief,
            Skill.Stealing => Archetype.Thief,
            Skill.Leatherwork => Archetype.Thief,
            Skill.Prayers => Archetype.Priest,
            Skill.FoodStoring => Archetype.Priest,
            Skill.Trade => Archetype.Priest,
            Skill.Tailoring => Archetype.Priest,
            _ => Archetype.Priest,
        };
    }
}

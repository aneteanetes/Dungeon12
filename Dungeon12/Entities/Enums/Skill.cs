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
        Smithing,

        [Value("m")]
        Portals,
        [Value("m")]
        Attension,
        [Value("m")]
        Enchantment,
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
        public static string Display(this Skill skill) => Global.Strings[skill];

        public static Archetype Class(this Skill skill) => skill switch
        {
            Skill.Landscape => Archetype.Warrior,
            Skill.Eating => Archetype.Warrior,
            Skill.Repair => Archetype.Warrior,
            Skill.Smithing => Archetype.Warrior,
            Skill.Portals => Archetype.Mage,
            Skill.Attension => Archetype.Mage,
            Skill.Enchantment => Archetype.Mage,
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
//Проверки навыков дают однотипные задания:
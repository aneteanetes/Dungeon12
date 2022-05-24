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
        public static string Display(this Skill skill) => skill switch
        {
            Skill.Landscape => Global.Strings[nameof(Skill.Landscape)],
            Skill.Eating => Global.Strings[nameof(Skill.Eating)],
            Skill.Repair => Global.Strings[nameof(Skill.Repair)],
            Skill.Smithing => Global.Strings[nameof(Skill.Smithing)],
            Skill.Portals => Global.Strings[nameof(Skill.Portals)],
            Skill.Attension => Global.Strings[nameof(Skill.Attension)],
            Skill.Enchantment => Global.Strings[nameof(Skill.Enchantment)],
            Skill.Alchemy => Global.Strings[nameof(Skill.Alchemy)],
            Skill.Traps => Global.Strings[nameof(Skill.Traps)],
            Skill.Lockpicking => Global.Strings[nameof(Skill.Lockpicking)],
            Skill.Stealing => Global.Strings[nameof(Skill.Stealing)],
            Skill.Leatherwork => Global.Strings[nameof(Skill.Leatherwork)],
            Skill.Prayers => Global.Strings[nameof(Skill.Prayers)],
            Skill.FoodStoring => Global.Strings[nameof(Skill.FoodStoring)],
            Skill.Trade => Global.Strings[nameof(Skill.Trade)],
            Skill.Tailoring => Global.Strings[nameof(Skill.Tailoring)],
            _ => "",
        };

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
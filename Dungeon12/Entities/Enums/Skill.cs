using Dungeon;
using Dungeon.Localization;
using Dungeon.Types;
using Dungeon12.Entities.Plates;
using Dungeon12.SceneObjects.Base;

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


        internal static Duration Durations(this Skill skill) => skill switch
        {
            Skill.Landscape => Duration.Instant,
            Skill.Eating => Duration.Passive,
            Skill.Repair => Duration.Instant,
            Skill.Smithing => Duration.Craft,
            Skill.Portals => Duration.Instant,
            Skill.Attension => Duration.Passive,
            Skill.Enchantment => Duration.Instant,
            Skill.Alchemy => Duration.Craft,
            Skill.Traps => Duration.Instant,
            Skill.Lockpicking => Duration.Instant,
            Skill.Stealing => Duration.Instant,
            Skill.Leatherwork => Duration.Craft,
            Skill.Prayers => Duration.Instant,
            Skill.FoodStoring => Duration.Passive,
            Skill.Trade => Duration.Passive,
            Skill.Tailoring => Duration.Craft,
            _ => Duration.Instant,
        };



        internal static GenericData GenericData(this Skill skill)
        {
            return new GenericData()
            {
                Title = skill.Localized(),
                Icon = $"AbilitiesPeacefull/{skill}.tga",
                Rank = Ranks.Novice.Localized(),
                Subtype = "Skill".Localized(),
                Text= Global.Strings.Description[skill.ToString()],
                SizeSettings = new Square() { Width=308 },
                Duration=skill.Durations()
            };
        }
    }
}
//Проверки навыков дают однотипные задания:
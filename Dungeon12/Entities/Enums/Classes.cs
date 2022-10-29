using Dungeon;
using Dungeon12.Attributes;

namespace Dungeon12.Entities.Enums
{
    internal enum Classes
    {
        _default,

        //warriors
        Monk,
        Knight,
        Shaman,
        [EnumDot(7,2)]
        Warrior = 1,
        Templar,
        DeathKnight,

        //mages
        Vudu,
        Warlock,
        BloodMage,
        Necromancer,
        [EnumDot(0, 2)]
        Elementalist = 2,
        Fireworshiper,

        //rogues
        Bard,
        [EnumDot(22, 2)]
        Rogue = 3,
        Ranger,
        Assassin,
        Poisoner,
        Alchemist,

        //priests
        Adept,
        Druid,
        [EnumDot(8, 0)]
        Priest = 4,
        Paladin,
        Tormentor,
        Inquisitor,
    }
}

using Dungeon12.Entities.Abilities.Mage;
using Dungeon12.Entities.Abilities.Priest;
using Dungeon12.Entities.Abilities.Thief;
using Dungeon12.Entities.Abilities.Warrior;
using Dungeon12.Entities.Enums;

namespace Dungeon12.Entities.Abilities
{
    public class Ability
    {
        public virtual string Name { get; }

        public string ClassName => this.GetType().Name;

        public virtual Archetype Class { get; }

        public static Ability[] ByClass(Archetype archetype) => archetype switch
        {
            Archetype.Warrior => new Ability[] { new WarriorAttack(), new WarriorStand(), new WarriorThrow(), new WarriorWarcry() },
            Archetype.Mage => new Ability[] { new MageArrowAttack(), new MageAoe(), new MageShield(), new MageSummon() },
            Archetype.Thief => new Ability[] { new ThiefAttack(), new ThiefShadow(), new ThiefMark(), new ThiefStep() },
            Archetype.Priest => new Ability[] { new PriestAttack(), new PriestHeal(), new PriestHolyNova(), new PriestAngel() },
            _ => new Ability[0],
        };
    }
}

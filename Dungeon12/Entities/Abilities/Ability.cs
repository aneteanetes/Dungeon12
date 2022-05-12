using Dungeon12.Entities.Abilities.Mage;
using Dungeon12.Entities.Abilities.Priest;
using Dungeon12.Entities.Abilities.Thief;
using Dungeon12.Entities.Abilities.Warrior;
using Dungeon12.Entities.Enums;

namespace Dungeon12.Entities.Abilities
{
    public abstract class Ability
    {
        public Ability()
        {
            Name=Global.Strings[ClassName];
            Description=Global.Strings.Description[ClassName];
            Bind();
            TextParams = GetTextParams();
        }

        public string Name { get; protected set; }

        public string ClassName => this.GetType().Name;

        public virtual int Value { get; set; }

        public int Cooldown { get; set; } = -1;

        public string Description { get; protected set; }

        public AbilityArea Area { get; protected set; }

        public AbilRange UseRange { get; set; }

        public Element Element { get; set; }

        public Ability[] Buffs { get; set; }

        public Ability[] Debuffs { get; set; }

        public string[] TextParams { get; protected set; }

        public virtual Archetype Class { get; }

        /// <summary>
        /// Здесь надо присвоить Name, Description, Area, Element, Range, Cooldown
        /// </summary>
        public abstract void Bind();

        /// <summary>
        /// Получить параметры в виде массива строк:
        /// <para>
        /// Атака: 15
        /// </para>
        /// <para>
        /// Тип: Физический
        /// </para>
        /// </summary>
        /// <returns></returns>
        public abstract string[] GetTextParams();

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

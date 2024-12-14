using Nabunassar.Entities.Abilities.Mage;
using Nabunassar.Entities.Abilities.Priest;
using Nabunassar.Entities.Abilities.Thief;
using Nabunassar.Entities.Abilities.Warrior;
using Nabunassar.Entities.Enums;

namespace Nabunassar.Entities.Abilities
{
    internal abstract class Ability
    {
        public Ability()
        {
            Name=Nabunassar.Global.Strings[ClassName];
            Description= Nabunassar.Global.Strings.Description[ClassName];
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
        /// Здесь надо присвоить Title, Description, Area, Element, Range, Cooldown
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

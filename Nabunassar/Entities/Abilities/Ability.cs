using Nabunassar.Entities.Abilities.Mage;
using Nabunassar.Entities.Abilities.Priest;
using Nabunassar.Entities.Abilities.Thief;
using Nabunassar.Entities.Abilities.Warrior;
using Nabunassar.Entities.Combat;
using Nabunassar.Entities.Enums;
using Nabunassar.Entities.Stats.PrimaryStats;

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

        public string Icon { get; set; }

        public virtual Archetype Archetype { get; }

        public virtual bool IsRanked => true;

        public Rank Rank { get; set; }

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

        /// <summary>
        /// Может ли урон от способности быть "срезан" защитами или способностями. Правило работает только на **основной** урон.
        /// </summary>
        public virtual bool IsAvailableForCutting => true;

        /// <summary>
        /// Является ли способность бустером силы способностей
        /// </summary>
        public virtual bool IsAPBooser => false;

        /// <summary>
        /// Если способность - бустер, то она проверяет может ли быть улучшена этим бустером
        /// </summary>
        /// <param name="ability"></param>
        /// <returns></returns>
        public virtual bool IsApplicable(Ability ability) => false;

        /// <summary>
        /// При атаке
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        public virtual DamageRange OnAttack(DamageRange damage) => damage;

        /// <summary>
        /// При получении урона
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        public virtual DamageRange OnDamage(DamageRange damage) => damage;
    }
}

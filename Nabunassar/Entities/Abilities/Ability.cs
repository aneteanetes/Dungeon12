using Nabunassar.Entities.Abilities.Battle;
using Nabunassar.Entities.Abilities.Mage;
using Nabunassar.Entities.Abilities.Priest;
using Nabunassar.Entities.Abilities.Thief;
using Nabunassar.Entities.Abilities.Warrior;
using Nabunassar.Entities.Characters;
using Nabunassar.Entities.Combat;
using Nabunassar.Entities.Enums;
using Nabunassar.Entities.Stats.PrimaryStats;
using System.Security.AccessControl;

namespace Nabunassar.Entities.Abilities
{

    /// <summary>
    /// Кастомный jsonconverter будет сохранять название типа и Rank, не более
    /// </summary>
    internal abstract class Ability : PersonaBinded
    {
        public Ability()
        {
            Name=Nabunassar.Global.Strings[ClassName];
            Description= Nabunassar.Global.Strings.Description[ClassName];
            //Bind();
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

        public static List<Ability> GetRaceCombatAbilities(Persona persona)
        {
            var race = persona.Race.Value;
            var list = new List<Ability>();

            switch (race)
            {
                case Race.Muitu:
                case Race.Shamshu:
                case Race.Nahazu:
                case Race.Shalmu:
                    list.Add(GetAbility(race.ToString(), AbilitySource.Race));
                    list.Add(GetAbility(race.ToString(), AbilitySource.Race));
                    break;
                case Race.Matu:
                    if(persona.Sex== Sex.Female)
                    {
                        list.Add(GetAbility("Sinisat", AbilitySource.Race));
                    }

                    list.Add(GetAbility("Ketsoal", AbilitySource.Race));
                    break;
                case Race.Habash:
                    if ((int)persona.PrimaryStats.Constitution>= (int)Rank.d10)
                    {
                        list.Add(GetAbility("Habash", AbilitySource.Race));
                    }
                    if ((int)persona.PrimaryStats.Intelligence >= (int)Rank.d10)
                    {
                        list.Add(GetAbility("Isshar", AbilitySource.Race));
                    }
                    list.Add(GetAbility($"Shelib_{(persona.Sex== Sex.Male ? "m" : "f")}", AbilitySource.Race));
                    break;
                default:
                    break;
            }

            return list;
        }

        public static List<Ability> GetRaceGloballyAbilities(Persona persona)
        {
            var race = persona.Race.Value;
            var list = new List<Ability>();

            if (race is Race.Muitu or Race.Shalmu or Race.Nahazu or Race.Shamshu or Race.Habash)
                return [];

            if (race == Race.Matu && persona.Sex == Sex.Male)
                list.Add(GetAbility("Adamatu", AbilitySource.Race));

            list.Add(GetAbility(race.ToString(), AbilitySource.Race));

            return list;
        }

        public static List<Ability> GetClassGloballyAbilies(Archetype archetype)
        {
            var list = new List<Ability>();

            switch (archetype)
            {
                case Archetype.Warrior:
                    list.Add(GetAbility("Landscape", AbilitySource.Archetype, archetype));
                    break;
                case Archetype.Mage:
                    list.Add(GetAbility("Portals", AbilitySource.Archetype, archetype));
                    break;
                case Archetype.Thief:
                    list.Add(GetAbility("Attension", AbilitySource.Archetype, archetype));
                    break;
                case Archetype.Priest:
                    list.Add(GetAbility("Prayers", AbilitySource.Archetype, archetype));
                    break;
                case Archetype.None:
                    break;
                default:
                    break;
            }

            return list;
        }

        public static List<Ability> GetFractionAbilies(Persona persona)
        {
            var frac = persona.Fraction.Value;
            return
            [
                GetAbility(frac.ToString(), AbilitySource.Fraction),
                GetAbility($"{frac}.Globally", AbilitySource.Fraction)
            ];
        }

        public static List<Ability> GetClassCombatAbilies(Archetype archetype)
        {
            var list = new List<Ability>();

            switch (archetype)
            {
                case Archetype.Warrior:
                    list.Add(GetAbility("WarriorAttack", AbilitySource.Archetype, archetype));
                    list.Add(GetAbility("WarriorStand", AbilitySource.Archetype, archetype));
                    list.Add(GetAbility("WarriorThrow", AbilitySource.Archetype, archetype));
                    list.Add(GetAbility("WarriorWarCry", AbilitySource.Archetype, archetype));
                    break;
                case Archetype.Mage:
                    list.Add(GetAbility("MageArrowAttack", AbilitySource.Archetype, archetype));
                    list.Add(GetAbility("MageShield", AbilitySource.Archetype, archetype));
                    list.Add(GetAbility("MageAoe", AbilitySource.Archetype, archetype));
                    list.Add(GetAbility("MageSummon", AbilitySource.Archetype, archetype));
                    break;
                case Archetype.Thief:
                    list.Add(GetAbility("ThiefAttack", AbilitySource.Archetype, archetype));
                    list.Add(GetAbility("ThiefMark", AbilitySource.Archetype, archetype));
                    list.Add(GetAbility("ThiefShadow", AbilitySource.Archetype, archetype));
                    list.Add(GetAbility("ThiefStep", AbilitySource.Archetype, archetype));
                    break;
                case Archetype.Priest:
                    list.Add(GetAbility("PriestAttack", AbilitySource.Archetype, archetype));
                    list.Add(GetAbility("PriestHeal", AbilitySource.Archetype, archetype));
                    list.Add(GetAbility("PriestAngel", AbilitySource.Archetype, archetype));
                    list.Add(GetAbility("PriestHolyNova", AbilitySource.Archetype, archetype));
                    break;
                case Archetype.None:
                    break;
                default:
                    break;
            }

            return list;
        }

        private static Ability GetAbility(string name, AbilitySource source, Archetype archetype = Archetype.None)
        {
            var pathSource = source == AbilitySource.Archetype ? archetype.ToString() : source.ToString();

            return new BattleAbility() { Icon = $"Abilities/{pathSource}/{name}.tga", Name = Global.Strings["abilities"][name], Description = Global.Strings["abilities"][name]["desc"] };
        }
    }
}

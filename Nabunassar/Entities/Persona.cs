using Nabunassar.Entities.Abilities.Battle;
using Nabunassar.Entities.Characters;
using Nabunassar.Entities.Enums;
using Nabunassar.Entities.Objects;
using Nabunassar.Entities.Stats;
using Nabunassar.Entities.Stats.AdditionalStats;
using Nabunassar.Entities.Stats.OffensiveStats;
using Nabunassar.Entities.Stats.PrimaryStats;

namespace Nabunassar.Entities
{
    internal class Persona : GameObject
    {
        public Persona()
        {
        }

        public virtual void BindPersona()
        {
            Health.BindPersona(this);
            Offencive.BindPersona(this);
            Additional.BindPersona(this);
            Speed.BindPersona(this);
        }

        public string PortraitImage { get; set; }
        public override string Image => PortraitImage;

        public Sex Sex { get; set; }

        /// <summary>
        /// Раса
        /// </summary>
        public Race? Race { get; set; }

        /// <summary>
        /// Фракция
        /// </summary>
        public Fraction? Fraction { get; set; }

        /// <summary>
        /// Архетип класс
        /// </summary>
        public Archetype? Archetype { get; set; }

        /// <summary>
        /// Первичные характеристики
        /// </summary>
        public Primary PrimaryStats { get; set; } = new();

        /// <summary>
        /// Характеристики боя
        /// </summary>
        public Offensive Offencive { get; set; } = new();

        /// <summary>
        /// Дополнительные характеристики
        /// </summary>
        public Additional Additional { get; set; } = new();

        /// <summary>
        /// Здоровье
        /// </summary>
        public Health Health { get; set; } = new();

        /// <summary>
        /// Скорость
        /// </summary>
        public Speed Speed { get; set; } = new();

        /// <summary>
        /// Уровень
        /// </summary>
        public int Level { get; private set; } = 1;

        /// <summary>
        /// Очки воли
        /// </summary>
        public double WillPoints { get; set; }

        /// <summary>
        /// Способности в бою
        /// </summary>
        public BattleAbilities CombatAbilities { get; set; } = new();

        /// <summary>
        /// Инвентарь
        /// </summary>
        public Inventory Inventory { get; set; } = new Inventory();
    }
}

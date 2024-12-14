using Dungeon.SceneObjects.Grouping;
using Nabunassar.Entities.Abilities.Globally;
using Nabunassar.Entities.Stats.GlobalStats;

namespace Nabunassar.Entities
{
    internal class Hero : Persona
    {
        public ObjectGroupProperty IsActive { get; set; } = new ObjectGroupProperty();

        public override void BindPersona()
        {
            MapStats.BindPersona(this);
            base.BindPersona();
        }

        /// <summary>
        /// Таблица опыта
        /// </summary>
        public ExpTable ExpTable { get; set; } = new ExpTable();

        /// <summary>
        /// Характеристики перемещения
        /// </summary>
        public MapStats MapStats { get; set; }= new();

        /// <summary>
        /// Способности на глобальной карте
        /// </summary>
        public GlobalAbilities GlobalAbilities { get; set; } = new();
    }
}
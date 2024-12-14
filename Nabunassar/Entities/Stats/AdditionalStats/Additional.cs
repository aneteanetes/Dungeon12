using Nabunassar.Entities.Enums;

namespace Nabunassar.Entities.Stats.AdditionalStats
{
    /// <summary>
    /// Крит, сокрушительный и скользящий удар - механики бросков кубиков (базовый крит работает на рандом)
    /// </summary>
    internal class Additional : BaseStat
    {
        public Element DamageType { get; set; } = Element.Physical;

        public CritChance CritChance { get; set; } = new();

        public SpellReflection SpellReflection { get; set; } = new();

        public DodgeChance DodgeChance { get; set; } = new();

        public BlockChance BlockChance { get; set; } = new();

        public ParryChance ParryChance { get; set; } = new();

        public override void BindPersona(Persona persona)
        {
            CritChance.BindPersona(persona);
            SpellReflection.BindPersona(persona);
            DodgeChance.BindPersona(persona);
            BlockChance.BindPersona(persona);
            ParryChance.BindPersona(persona);
        }
    }
}
